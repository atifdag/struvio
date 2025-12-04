namespace Struvio.Persistence.Configurations;

/// <summary>
/// Permission varlığı için Entity Framework Core yapılandırması.
/// Yetki tablosu yapısı, ilişkiler ve indeksler burada tanımlanır.
/// </summary>
internal class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    /// <summary>
    /// Permission varlığı için veritabanı yapılandırmasını uygular.
    /// </summary>
    /// <param name="builder">Varlık yapılandırma oluşturucu</param>
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(PersistenceConstants.Max256Lenght).IsRequired(true);

        builder.Property(x => x.ControllerName)
            .HasMaxLength(PersistenceConstants.Max256Lenght)
            .IsRequired(false);

        builder.Property(x => x.ActionName)
            .HasMaxLength(PersistenceConstants.Max256Lenght)
            .IsRequired(false);

        builder.Property(x => x.Path)
            .HasMaxLength(PersistenceConstants.Max256Lenght)
            .IsRequired(false);

        builder.HasOne(o => o.Creator)
            .WithMany(m => m.CreatedPermissions)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.LastModifier)
            .WithMany(m => m.LastModifiedPermissions)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.Code).IsUnique(true);

        builder.HasIndex(x => x.SequenceNumber);

        builder.HasIndex(x => x.IsApproved);

    }
}
