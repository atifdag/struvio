namespace Struvio.Persistence.Configurations;

/// <summary>
/// Language varlığı için Entity Framework Core yapılandırması.
/// Dil tablosu yapısı ve indeksler burada tanımlanır.
/// </summary>
internal class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    /// <summary>
    /// Language varlığı için veritabanı yapılandırmasını uygular.
    /// </summary>
    /// <param name="builder">Varlık yapılandırma oluşturucu</param>
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("Languages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(PersistenceConstants.Max256Lenght).IsRequired(true);

        builder.Property(x => x.ShortCode)
           .HasMaxLength(PersistenceConstants.Max256Lenght);

        builder.Property(x => x.Name)
            .HasMaxLength(PersistenceConstants.Max256Lenght);

        builder.HasIndex(x => x.Code).IsUnique(true);

        builder.HasIndex(x => x.SequenceNumber);

        builder.HasIndex(x => x.IsApproved);

    }
}
