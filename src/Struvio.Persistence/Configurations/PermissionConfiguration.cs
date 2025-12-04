namespace Struvio.Persistence.Configurations;
internal class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
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
