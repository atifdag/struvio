namespace Struvio.Persistence.Configurations;
internal class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("Languages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(DomainConstants.Max256Lenght).IsRequired(true);

        builder.Property(x => x.ShortCode)
           .HasMaxLength(DomainConstants.Max256Lenght);

        builder.Property(x => x.Name)
            .HasMaxLength(DomainConstants.Max256Lenght);

        builder.HasIndex(x => x.Code).IsUnique(true);

        builder.HasIndex(x => x.SequenceNumber);

        builder.HasIndex(x => x.IsApproved);

    }
}
