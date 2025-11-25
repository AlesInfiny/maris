using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DresscaCMS.EfInfrastructure.Configurations;

internal class AnnouncementContentHistoryConfiguration : IEntityTypeConfiguration<AnnouncementContentHistory>
{
    public void Configure(EntityTypeBuilder<AnnouncementContentHistory> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("AnnouncementContentHistory");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired();

        builder.Property(e => e.AnnouncementHistoryId)
            .IsRequired();

        builder.Property(e => e.LanguageCode)
            .IsRequired()
            .HasMaxLength(8);

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(e => e.Message)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(e => e.LinkUrl)
            .HasMaxLength(1024);

        builder.HasData(
        [

        ]);
    }
}
