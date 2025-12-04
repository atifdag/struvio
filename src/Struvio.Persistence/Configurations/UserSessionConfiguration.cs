namespace Struvio.Persistence.Configurations;

/// <summary>
/// UserSession varlığı için Entity Framework Core yapılandırması.
/// Kullanıcı oturumu tablosu yapısı ve ilişkiler burada tanımlanır.
/// </summary>
internal class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    /// <summary>
    /// UserSession varlığı için veritabanı yapılandırmasını uygular.
    /// </summary>
    /// <param name="builder">Varlık yapılandırma oluşturucu</param>
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable("UserSessions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.IpAddress)
            .HasMaxLength(PersistenceConstants.Max256Lenght).IsRequired(false);


        builder.Property(x => x.AgentInfo)
            .HasMaxLength(PersistenceConstants.Max2000Lenght).IsRequired(false);

        builder.HasOne(o => o.Creator)
            .WithMany(m => m.CreatedUserSessions)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
