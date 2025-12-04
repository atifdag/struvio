namespace Struvio.Persistence.Configurations;
internal class UserSessionHistoryConfiguration : IEntityTypeConfiguration<UserSessionHistory>
{
    public void Configure(EntityTypeBuilder<UserSessionHistory> builder)
    {
        builder.ToTable("UserSessionHistories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.IpAddress)
            .HasMaxLength(DomainConstants.Max256Lenght).IsRequired(false);


        builder.Property(x => x.AgentInfo)
            .HasMaxLength(DomainConstants.Max2000Lenght).IsRequired(false);

    }
}
