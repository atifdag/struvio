namespace Struvio.Persistence.Configurations;
internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserName)
            .HasMaxLength(PersistenceConstants.Max50Lenght).IsRequired(true);

        builder.Property(x => x.Email)
           .HasMaxLength(PersistenceConstants.Max256Lenght)
           .IsRequired(true);

        builder.Property(x => x.SecurityStamp)
    .HasMaxLength(PersistenceConstants.Max512Lenght).IsRequired(false);

        builder.Property(x => x.ConcurrencyStamp)
.HasMaxLength(PersistenceConstants.Max512Lenght)
.IsRequired(false);

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(PersistenceConstants.Max50Lenght)
            .IsRequired(true);

        builder.HasOne(o => o.Person)
             .WithOne(x => x.User);

        builder.HasOne(o => o.Organization)
            .WithMany(m => m.Users)
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Language)
          .WithMany(m => m.Users)
          .HasForeignKey(x => x.LanguageId)
          .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Creator)
            .WithMany(m => m.CreatedUsers)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.LastModifier)
            .WithMany(m => m.LastModifiedUsers)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(PersistenceConstants.Max11Lenght);

        builder.HasIndex(x => x.UserName).IsUnique(true);

        builder.HasIndex(x => x.Email).IsUnique(true);

        builder.HasIndex(x => x.PhoneNumber).IsUnique(true);

        builder.HasIndex(x => x.SequenceNumber);

        builder.HasIndex(x => x.IsApproved);

    }
}
