namespace Struvio.Persistence.Configurations;

/// <summary>
/// UserOrganizationRoleLine varlığı için Entity Framework Core yapılandırması.
/// Kullanıcı-Organizasyon-Rol ilişki tablosu yapısı ve ilişkiler burada tanımlanır.
/// </summary>
internal class UserOrganizationRoleLineConfiguration : IEntityTypeConfiguration<UserOrganizationRoleLine>
{
    /// <summary>
    /// UserOrganizationRoleLine varlığı için veritabanı yapılandırmasını uygular.
    /// </summary>
    /// <param name="builder">Varlık yapılandırma oluşturucu</param>
    public void Configure(EntityTypeBuilder<UserOrganizationRoleLine> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasOne(o => o.User)
            .WithMany(m => m.UserOrganizationRoleLines)
            .HasForeignKey(x => x.UserId).IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Organization)
            .WithMany(m => m.UserOrganizationRoleLines)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Role)
            .WithMany(m => m.UserOrganizationRoleLines)
            .HasForeignKey(x => x.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
