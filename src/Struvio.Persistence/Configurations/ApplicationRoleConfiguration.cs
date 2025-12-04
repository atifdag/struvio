namespace Struvio.Persistence.Configurations;

/// <summary>
/// ApplicationRole varlığı için Entity Framework Core yapılandırması.
/// Tablo adı, alanlar, ilişkiler ve indeksler burada tanımlanır.
/// </summary>
internal class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    /// <summary>
    /// ApplicationRole varlığı için veritabanı yapılandırmasını uygular.
    /// </summary>
    /// <param name="builder">Varlık yapılandırma oluşturucu</param>
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(PersistenceConstants.Max256Lenght).IsRequired(true);

        builder.HasOne(o => o.Organization)
            .WithMany(m => m.Roles)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Creator)
            .WithMany(m => m.CreatedRoles)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.LastModifier)
            .WithMany(m => m.LastModifiedRoles)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.Code).IsUnique(true);

        builder.HasIndex(x => x.SequenceNumber);

        builder.HasIndex(x => x.IsApproved);

    }
}
