namespace Struvio.Persistence.Configurations;
internal class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable("UserSessions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.IpAddress)
            .HasMaxLength(DomainConstants.Max256Lenght).IsRequired(false);


        builder.Property(x => x.AgentInfo)
            .HasMaxLength(DomainConstants.Max2000Lenght).IsRequired(false);

        builder.HasOne(o => o.Creator)
            .WithMany(m => m.CreatedUserSessions)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
