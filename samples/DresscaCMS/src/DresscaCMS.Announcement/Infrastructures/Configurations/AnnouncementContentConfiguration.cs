using DresscaCMS.Announcement.Infrastructures.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DresscaCMS.Announcement.Infrastructures.Configurations;

internal class AnnouncementContentConfiguration : IEntityTypeConfiguration<AnnouncementContent>
{

    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<AnnouncementContent> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("AnnouncementContent");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired();

        builder.Property(e => e.AnnouncementId)
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

        builder.Property(e => e.LinkedUrl)
            .HasMaxLength(1024);

        builder.Property(e => e.RowVersion)
            .IsRowVersion();

        builder.HasData(
        [

        ]);
    }
}
