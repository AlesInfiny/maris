using DresscaCMS.Announcement.Infrastructures.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DresscaCMS.Announcement.Infrastructures.Configurations;

internal class AnnouncementHistoryConfiguration : IEntityTypeConfiguration<AnnouncementHistory>
{
    public void Configure(EntityTypeBuilder<AnnouncementHistory> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("AnnouncementHistory");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired();

        builder.Property(e => e.AnnouncementId)
            .IsRequired();

        builder.Property(e => e.Category)
            .HasMaxLength(128);

        builder.Property(e => e.PostDateTime)
            .IsRequired();

        builder.Property(e => e.ExpireDateTime);

        builder.Property(e => e.DisplayPriority)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.ChangedBy)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(e => e.OperationType)
            .IsRequired();

        builder.HasMany(e => e.Contents)
            .WithOne(c => c.AnnouncementHistory)
            .HasForeignKey(c => c.AnnouncementHistoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
        [

        ]);
    }
}
