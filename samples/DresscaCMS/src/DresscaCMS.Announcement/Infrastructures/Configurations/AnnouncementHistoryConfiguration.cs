using DresscaCMS.Announcement.ApplicationCore.Enumerations;
using DresscaCMS.Announcement.Infrastructures.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DresscaCMS.Announcement.Infrastructures.Configurations;

/// <summary>
///  <see cref="AnnouncementHistory"/> テーブルエンティティの構成を提供します。
/// </summary>
internal class AnnouncementHistoryConfiguration : IEntityTypeConfiguration<AnnouncementHistory>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<AnnouncementHistory> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(
            "AnnouncementHistory",
            table =>
            {
                table.HasCheckConstraint("CK_AnnouncementHistory_DisplayPriority", "[DisplayPriority] IN (1, 2, 3, 4)");
                table.HasCheckConstraint("CK_AnnouncementHistory_OperationType", "[OperationType] IN (0, 1, 2,3)");
            });

        builder.Property(e => e.Id)
            .IsRequired();

        builder.Property(e => e.AnnouncementId)
            .IsRequired();

        builder.Property(e => e.ChangedBy)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.OperationType)
            .IsRequired();

        builder.Property(e => e.Category)
            .HasMaxLength(128);

        builder.Property(e => e.PostDateTime)
            .IsRequired();

        builder.Property(e => e.ExpireDateTime);

        builder.Property(e => e.DisplayPriority)
            .IsRequired();

        builder.HasMany(e => e.Contents)
            .WithOne(c => c.AnnouncementHistory)
            .HasForeignKey(c => c.AnnouncementHistoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            new AnnouncementHistory
            {
                Id = Guid.Parse("39999999-3333-3333-3333-333333333333"),
                AnnouncementId = Guid.Parse("19999999-1111-1111-1111-111111111111"),
                ChangedBy = "system",
                CreatedAt = new DateTimeOffset(2011, 1, 21, 0, 0, 0, TimeSpan.Zero),
                OperationType = OperationType.Delete,
                Category = "一般",
                PostDateTime = new DateTimeOffset(2018, 1, 21, 0, 0, 0, TimeSpan.Zero),
                ExpireDateTime = new DateTimeOffset(2019, 1, 1, 21, 0, 0, TimeSpan.Zero),
                DisplayPriority = DisplayPriority.Critical,
            });
    }
}
