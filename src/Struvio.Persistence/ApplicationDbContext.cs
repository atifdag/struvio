namespace Struvio.Persistence;

/// <summary>
/// Uygulama veritabanı bağlamı. Entity Framework Core DbContext sınıfından türetilir.
/// Varlık yapılandırmaları ve değişiklik takibi (audit) işlemlerini yönetir.
/// </summary>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, IdentityUserClaim<Guid>, UserOrganizationRoleLine,
    IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>(options)
{
    // IIdentityContext'i cache'le (her GetService çağrısı maliyetli)
    private IIdentityContext? _cachedIdentityContext;

    /// <summary>
    /// Mevcut oturumdaki kimlik doğrulanmış kullanıcıyı getirir.
    /// Performans optimizasyonu: AsNoTracking kullanılarak tracking overhead'i kaldırıldı.
    /// </summary>
    /// <param name="cancellationToken">İptal belirteci</param>
    /// <returns>Kimlik doğrulanmış kullanıcı</returns>
    /// <exception cref="IdentityNotFoundException">Kullanıcı bulunamazsa fırlatılır</exception>
    public async Task<ApplicationUser> IdentityUserAsync(CancellationToken cancellationToken = default)
    {
        // IIdentityContext'i cache'ten al (GetService maliyetini önle)
        _cachedIdentityContext ??= this.GetService<IIdentityContext>();
        var userId = _cachedIdentityContext.GetUserId();
        
        // AsNoTracking: Audit işlemlerinde kullanıcı tracking'e gerek yok (memory tasarrufu)
        // AsSplitQuery: 3 Include için tek JOIN yerine 4 ayrı query (daha hızlı, özellikle çok satır olunca)
        var user = await Set<ApplicationUser>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Language)
            .Include(x => x.Organization)
            .Include(x => x.Person)
            .FirstOrDefaultAsync(x => x.IsApproved && x.Id == userId, cancellationToken);
        
        return user ?? throw new IdentityNotFoundException();
    }

    /// <summary>
    /// Değişiklikleri veritabanına kaydeder. Kayıt öncesi audit işlemlerini otomatik olarak gerçekleştirir.
    /// </summary>
    /// <param name="cancellationToken">İptal belirteci</param>
    /// <returns>Etkilenen kayıt sayısı</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Değişiklikleri veritabanına göndermeden önce yakalıyoruz.
        await OnBeforeSaveChanges(cancellationToken);

        // Standart kaydetme işlemini yürütüyoruz.
        return await base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Model oluşturma sırasında varlık yapılandırmalarını uygular.
    /// Assembly'deki tüm IEntityTypeConfiguration implementasyonlarını otomatik olarak bulur ve uygular.
    /// Performans optimizasyonu: Reflection overhead'i minimize eder.
    /// </summary>
    /// <param name="modelBuilder">Model oluşturucu</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // EF Core'un optimize edilmiş metodunu kullan
        // Bu metod internal olarak cache kullanır ve reflection overhead'i minimize eder
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    /// <summary>
    /// Kaydetme işleminden önce çağrılan metod. 
    /// Varlık değişikliklerini izler, audit bilgilerini günceller ve geçmiş kayıtları oluşturur.
    /// </summary>
    /// <param name="cancellationToken">İptal belirteci</param>
    private async Task OnBeforeSaveChanges(CancellationToken cancellationToken = default)
    {
        ChangeTracker.DetectChanges();

        // Kullanıcıyı bir kez çek ve tüm işlemlerde (Add/Modify/Delete) cache'le (N+1 query problemi önlenir)
        ApplicationUser? identityUser = null;

        foreach (var entry in ChangeTracker.Entries().Where(t => t.State == EntityState.Added).Select(t => t.Entity))
        {
            if (entry is not IHistoryEntity historyEntity)
            {
                if (entry is IEntity iEntity)
                {
                    iEntity.Id = Guid.CreateVersion7();
                }
            }
     
            else
            {
                // İlk seferinde çek, sonra cache'ten kullan
                identityUser ??= await IdentityUserAsync(cancellationToken);

                historyEntity.CreationTime = DateTime.UtcNow;

                historyEntity.LastModificationTime = DateTime.UtcNow;

                historyEntity.Creator = identityUser;

                historyEntity.LastModifier = identityUser;

                historyEntity.Version = 1;

                if (entry is IOrganizationalEntity organizationalEntity)
                {
                    organizationalEntity.Organization = identityUser.Organization;
                }
            }


        }

        foreach (var (entity, changedData) in ChangeTracker.Entries().Where(p => p.State == EntityState.Modified).ToDictionary(
            e => e.Entity,
            e => e.EntityChangeDetection()
        ))
        {
            //Değişen alan olmasa bile toplu kayıtta modifed true geldiği için böyle bir önlem alındı. 
            if (changedData is null || entity is not IHistoryEntity track) continue;
            
            // İşlemi yapan kullanıcıyı ilk seferinde çek, sonra cache'ten kullan
            identityUser ??= await IdentityUserAsync(cancellationToken);

            // Geçmiş tablosuna kayıt bilgileri değişen entity'den alınıyor
            History history = new()
            {
                Id = Guid.CreateVersion7(),

                // entity'nin satır Id'si
                RowId = track.Id,

                // entity'nin adı
                EntityName = entity.GetType().Name,

                // satır sürüm numarası
                Version = track.Version,

                // entity verileri JsonDocument'e çevriliyor
                Data = entity.ToCreateHistoryAsJson(),

                // entity'nin değişen verileri JsonDocument'e çevriliyor (cache'ten)
                ChangedData = changedData,

                // İşlemi yapan kullanıcı
                UserId = identityUser?.Id ?? (entity.GetType().Name == nameof(ApplicationUser) ? track.Id : Guid.Empty),

                // işlem zamanı
                TransactionTime = DateTime.UtcNow
            };

            // Geçmiş tablosuna kayıt giriliyor
            Add(history);

            // işlem zamanı
            track.LastModificationTime = DateTime.UtcNow;

            // İşlemi yapan kullanıcı (cache'ten)
            track.LastModifier = identityUser!;

            var version = track.Version;

            // değişen entity'nin satır sürüm numarası 1 artırılıyor
            track.Version = version + 1;
        }

        // Silme işlemleri
        foreach (var entity in ChangeTracker.Entries().Where(t => t.State == EntityState.Deleted).Select(t => t.Entity))
        {
            if (entity is not IHistoryEntity track) continue;

            // Kullanıcıyı cache'ten kullan (yukarıda zaten çekilmişse tekrar çekilmez)
            identityUser ??= await IdentityUserAsync(cancellationToken);

            // Geçmiş tablosuna kayıt bilgileri silinecek olan entity'den alınıyor
            History history = new()
            {
                Id = Guid.CreateVersion7(),

                // entity'nin satır Id'si
                RowId = track.Id,

                // Silindi mi?
                IsDeleted = true,

                // entity'nin adı
                EntityName = entity.GetType().Name,

                // satır sürüm numarası
                Version = track.Version,

                // İşlemi yapan kullanıcı (cache'ten)
                UserId = identityUser?.Id ?? (entity.GetType().Name == nameof(ApplicationUser) ? track.Id : Guid.Empty),

                // işlem zamanı
                TransactionTime = DateTime.UtcNow,

                // silinecek entity verileri JsonDocument'e çevriliyor
                Data = entity.ToCreateHistoryAsJson()
            };

            // Geçmiş tablosuna kayıt giriliyor
            Add(history);
        }
    }
}
