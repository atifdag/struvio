namespace Struvio.Persistence.Configurations;

/// <summary>
/// Organization varlığı için Entity Framework Core yapılandırması.
/// Organizasyon tablosu yapısı, alanlar ve indeksler burada tanımlanır.
/// </summary>
internal class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    /// <summary>
    /// Organization varlığı için veritabanı yapılandırmasını uygular.
    /// </summary>
    /// <param name="builder">Varlık yapılandırma oluşturucu</param>
    public void Configure(EntityTypeBuilder<Organization> builder)
    {

        builder.ToTable("Organizations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(PersistenceConstants.Max256Lenght);

        builder.Property(x => x.Description)
            .HasMaxLength(PersistenceConstants.Max2000Lenght)
            .IsRequired(false);

        builder.Property(x => x.Code)
            .HasMaxLength(PersistenceConstants.Max256Lenght).IsRequired(true);

        builder.Property(x => x.ApiKey)
            .HasMaxLength(PersistenceConstants.Max256Lenght);

        builder.Property(x => x.ApiPassword)
            .HasMaxLength(PersistenceConstants.Max256Lenght);

        builder.HasIndex(x => x.Code).IsUnique(true);

        builder.HasIndex(x => x.ApiKey).IsUnique(true);

        builder.HasIndex(x => x.SequenceNumber);

        builder.HasIndex(x => x.IsApproved);

    }
}
