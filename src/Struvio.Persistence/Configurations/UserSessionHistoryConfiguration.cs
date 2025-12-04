namespace Struvio.Persistence.Configurations;

/// <summary>
/// UserSessionHistory varlığı için Entity Framework Core yapılandırması.
/// Kullanıcı oturum geçmişi tablosu yapısı burada tanımlanır.
/// </summary>
internal class UserSessionHistoryConfiguration : IEntityTypeConfiguration<UserSessionHistory>
{
    /// <summary>
    /// UserSessionHistory varlığı için veritabanı yapılandırmasını uygular.
    /// </summary>
    /// <param name="builder">Varlık yapılandırma oluşturucu</param>
    public void Configure(EntityTypeBuilder<UserSessionHistory> builder)
    {
        builder.ToTable("UserSessionHistories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.IpAddress)
            .HasMaxLength(PersistenceConstants.Max256Lenght).IsRequired(false);


        builder.Property(x => x.AgentInfo)
            .HasMaxLength(PersistenceConstants.Max2000Lenght).IsRequired(false);

    }
}
