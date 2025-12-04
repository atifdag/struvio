namespace Struvio.Persistence.Configurations;
internal class RolePermissionLineConfiguration : IEntityTypeConfiguration<RolePermissionLine>
{
    public void Configure(EntityTypeBuilder<RolePermissionLine> builder)
    {
        builder.ToTable("RolePermissionLines");

        builder.HasKey(x => x.Id);

        builder.HasOne(o => o.Role)
            .WithMany(m => m.RolePermissionLines)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Permission)
            .WithMany(m => m.RolePermissionLines)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Creator)
            .WithMany(m => m.CreatedRolePermissionLines)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.LastModifier)
            .WithMany(m => m.LastModifiedRolePermissionLines)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
