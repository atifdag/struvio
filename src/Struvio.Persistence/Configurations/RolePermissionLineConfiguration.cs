namespace Struvio.Persistence.Configurations;

/// <summary>
/// RolePermissionLine varlığı için Entity Framework Core yapılandırması.
/// Rol-Yetki ilişki tablosu yapısı ve ilişkiler burada tanımlanır.
/// </summary>
internal class RolePermissionLineConfiguration : IEntityTypeConfiguration<RolePermissionLine>
{
    /// <summary>
    /// RolePermissionLine varlığı için veritabanı yapılandırmasını uygular.
    /// </summary>
    /// <param name="builder">Varlık yapılandırma oluşturucu</param>
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
