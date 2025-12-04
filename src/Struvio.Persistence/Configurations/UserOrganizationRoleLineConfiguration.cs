namespace Struvio.Persistence.Configurations;
internal class UserOrganizationRoleLineConfiguration : IEntityTypeConfiguration<UserOrganizationRoleLine>
{
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
