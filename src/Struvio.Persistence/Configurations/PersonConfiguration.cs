namespace Struvio.Persistence.Configurations;
internal class PersonConfiguration : IEntityTypeConfiguration<Person>
{
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
