using DresscaCMS.Announcement.ApplicationCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DresscaCMS.Announcement.Infrastructures.Configurations;

/// <summary>
///  <see cref="Announcement"/> テーブルエンティティの構成を提供します。
/// </summary>
internal class AnnouncementConfiguration : IEntityTypeConfiguration<Entities.Announcement>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Entities.Announcement> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // お知らせメッセージ
        builder.ToTable(
           "Announcements",
           table => table.HasCheckConstraint("CK_Announcement_DisplayPriority", "[DisplayPriority] IN (1, 2, 3, 4)"));

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Category)
                .HasMaxLength(128);

        builder.Property(e => e.PostDateTime)
                .IsRequired();

        builder.Property(e => e.ExpireDateTime);
        builder.Property(e => e.DisplayPriority)
                .IsRequired();

        builder.Property(e => e.CreatedAt)
                .IsRequired();

        builder.Property(e => e.ChangedAt)
                .IsRequired();

        builder.Property(e => e.IsDeleted)
                .IsRequired();

        // 1:N Announcements – AnnouncementContents
        builder.HasMany(e => e.Contents)
                .WithOne(c => c.Announcement)
                .HasForeignKey(c => c.AnnouncementId)
                .OnDelete(DeleteBehavior.Cascade);

        // 1:N Announcements – AnnouncementHistory
        builder.HasMany(e => e.Histories)
                .WithOne(h => h.Announcement)
                .HasForeignKey(h => h.AnnouncementId)
                .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-000000000001"), Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Critical, CreatedAt = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-000000000002"), Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 2, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 2, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.High, CreatedAt = new DateTimeOffset(2025, 1, 2, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 2, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-000000000003"), Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 3, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 3, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Medium, CreatedAt = new DateTimeOffset(2025, 1, 3, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 3, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-000000000004"), Category = "イベント", PostDateTime = new DateTimeOffset(2025, 1, 4, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 4, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Low, CreatedAt = new DateTimeOffset(2025, 1, 4, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 4, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-000000000005"), Category = "イベント", PostDateTime = new DateTimeOffset(2025, 1, 5, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 5, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Critical, CreatedAt = new DateTimeOffset(2025, 1, 5, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 5, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-000000000006"), Category = "更新", PostDateTime = new DateTimeOffset(2025, 1, 6, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 6, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.High, CreatedAt = new DateTimeOffset(2025, 1, 6, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 6, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-000000000007"), Category = "更新", PostDateTime = new DateTimeOffset(2025, 1, 7, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 7, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Medium, CreatedAt = new DateTimeOffset(2025, 1, 7, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 7, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-000000000008"), Category = "重要", PostDateTime = new DateTimeOffset(2025, 1, 8, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 8, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Low, CreatedAt = new DateTimeOffset(2025, 1, 8, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 8, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-000000000009"), Category = "重要", PostDateTime = new DateTimeOffset(2025, 1, 9, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 9, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Critical, CreatedAt = new DateTimeOffset(2025, 1, 9, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 9, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-00000000000a"), Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 10, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 10, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Medium, CreatedAt = new DateTimeOffset(2025, 1, 10, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 10, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-00000000000b"), Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 11, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 11, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.High, CreatedAt = new DateTimeOffset(2025, 1, 11, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 11, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-00000000000c"), Category = "イベント", PostDateTime = new DateTimeOffset(2025, 1, 12, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 12, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Low, CreatedAt = new DateTimeOffset(2025, 1, 12, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 12, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-00000000000d"), Category = "イベント", PostDateTime = new DateTimeOffset(2025, 1, 13, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 13, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Critical, CreatedAt = new DateTimeOffset(2025, 1, 13, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 13, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-00000000000e"), Category = "更新", PostDateTime = new DateTimeOffset(2025, 1, 14, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 14, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Medium, CreatedAt = new DateTimeOffset(2025, 1, 14, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 14, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-00000000000f"), Category = "更新", PostDateTime = new DateTimeOffset(2025, 1, 15, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 15, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.High, CreatedAt = new DateTimeOffset(2025, 1, 15, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 15, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-000000000010"), Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 16, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 16, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Low, CreatedAt = new DateTimeOffset(2025, 1, 16, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 16, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-000000000011"), Category = "重要", PostDateTime = new DateTimeOffset(2025, 1, 17, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 17, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Critical, CreatedAt = new DateTimeOffset(2025, 1, 17, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 17, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-000000000012"), Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 18, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 18, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Medium, CreatedAt = new DateTimeOffset(2025, 1, 18, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 18, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-000000000013"), Category = "イベント", PostDateTime = new DateTimeOffset(2025, 1, 19, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 19, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.High, CreatedAt = new DateTimeOffset(2025, 1, 19, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 19, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-000000000014"), Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 20, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 20, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Medium, CreatedAt = new DateTimeOffset(2025, 1, 20, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 20, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-000000000015"), Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 21, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 21, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Low, CreatedAt = new DateTimeOffset(2025, 1, 21, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 21, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-000000000016"), Category = "一般", PostDateTime = new DateTimeOffset(2018, 1, 21, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2019, 1, 1, 21, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Critical, CreatedAt = new DateTimeOffset(2010, 1, 21, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2011, 1, 21, 0, 0, 0, TimeSpan.Zero), IsDeleted = true },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-000000000017"), Category = "テスト", PostDateTime = new DateTimeOffset(2030, 1, 21, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2100, 1, 1, 21, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Low, CreatedAt = new DateTimeOffset(2025, 1, 21, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 21, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Announcement { Id = Guid.Parse("0193ae97-b800-7001-8001-000000000018"), Category = "テスト", PostDateTime = new DateTimeOffset(2030, 1, 21, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2100, 1, 1, 21, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Low, CreatedAt = new DateTimeOffset(2025, 1, 21, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 21, 0, 0, 0, TimeSpan.Zero), IsDeleted = false });
    }
}
