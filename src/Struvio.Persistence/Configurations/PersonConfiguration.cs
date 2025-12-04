namespace Struvio.Persistence.Configurations;

/// <summary>
/// Person varlığı için Entity Framework Core yapılandırması.
/// Kişi tablosu yapısı ve indeksler burada tanımlanır.
/// </summary>
internal class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    /// <summary>
    /// Person varlığı için veritabanı yapılandırmasını uygular.
    /// </summary>
    /// <param name="builder">Varlık yapılandırma oluşturucu</param>
    public void Configure(EntityTypeBuilder<Person> builder)
    {

        builder.ToTable("Persons");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.IdentityNumber)
            .HasMaxLength(PersistenceConstants.Max256Lenght).IsRequired(true);

        builder.Property(x => x.FirstName)
            .HasMaxLength(PersistenceConstants.Max256Lenght);

        builder.Property(x => x.LastName).IsRequired()
            .HasMaxLength(PersistenceConstants.Max256Lenght);

        builder.HasIndex(x => x.IdentityNumber).IsUnique(true);

        builder.HasIndex(x => x.IsApproved);

    }
}
