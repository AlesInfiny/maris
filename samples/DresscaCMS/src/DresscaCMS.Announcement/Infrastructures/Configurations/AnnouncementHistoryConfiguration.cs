using DresscaCMS.Announcement.ApplicationCore;
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
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-000000000001"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000001"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Critical, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-000000000002"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000002"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 2, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 2, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 2, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.High, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-000000000003"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000003"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 3, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 3, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 3, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Medium, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-000000000004"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000004"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 4, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "イベント", PostDateTime = new DateTimeOffset(2025, 1, 4, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 4, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Low, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-000000000005"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000005"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 5, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "イベント", PostDateTime = new DateTimeOffset(2025, 1, 5, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 5, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Critical, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-000000000006"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000006"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 6, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "更新", PostDateTime = new DateTimeOffset(2025, 1, 6, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 6, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.High, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-000000000007"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000007"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 7, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "更新", PostDateTime = new DateTimeOffset(2025, 1, 7, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 7, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Medium, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-000000000008"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000008"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 8, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "重要", PostDateTime = new DateTimeOffset(2025, 1, 8, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 8, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Low, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-000000000009"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000009"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 9, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "重要", PostDateTime = new DateTimeOffset(2025, 1, 9, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 9, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Critical, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-00000000000a"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-00000000000a"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 10, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 10, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 10, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Medium, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-00000000000b"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-00000000000b"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 11, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 11, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 11, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.High, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-00000000000c"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-00000000000c"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 12, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "イベント", PostDateTime = new DateTimeOffset(2025, 1, 12, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 12, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Low, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-00000000000d"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-00000000000d"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 13, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "イベント", PostDateTime = new DateTimeOffset(2025, 1, 13, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 13, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Critical, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-00000000000e"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-00000000000e"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 14, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "更新", PostDateTime = new DateTimeOffset(2025, 1, 14, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 14, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Medium, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-00000000000f"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-00000000000f"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 15, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "更新", PostDateTime = new DateTimeOffset(2025, 1, 15, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 15, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.High, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-000000000010"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000010"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 16, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 16, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 16, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Low, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-000000000011"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000011"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 17, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "重要", PostDateTime = new DateTimeOffset(2025, 1, 17, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 17, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Critical, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-000000000012"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000012"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 18, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 18, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 18, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Medium, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-000000000013"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000013"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 19, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "イベント", PostDateTime = new DateTimeOffset(2025, 1, 19, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 19, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.High, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-000000000014"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000014"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 20, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 20, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 20, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Medium, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-000000000015"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000015"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 21, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 21, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 21, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Low, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-000000000016"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000017"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 21, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "テスト", PostDateTime = new DateTimeOffset(2025, 1, 21, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 21, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Low, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-000000000017"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000018"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 21, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "テスト", PostDateTime = new DateTimeOffset(2025, 1, 21, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 21, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Low, },
            new AnnouncementHistory { Id = Guid.Parse("0193ae97-b800-7003-8001-000000000018"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000016"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2011, 1, 21, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Delete, Category = "一般", PostDateTime = new DateTimeOffset(2018, 1, 21, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2019, 1, 1, 21, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Critical, });
    }
}
