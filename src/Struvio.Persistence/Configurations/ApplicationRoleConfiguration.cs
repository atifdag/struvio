namespace Struvio.Persistence.Configurations;
internal class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
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
