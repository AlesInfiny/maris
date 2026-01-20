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
            new AnnouncementHistory { Id = Guid.Parse("33333333-3333-3333-3333-333333333301"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111111"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Critical, },
            new AnnouncementHistory { Id = Guid.Parse("33333333-3333-3333-3333-333333333302"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111112"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 2, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 2, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 2, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.High, },
            new AnnouncementHistory { Id = Guid.Parse("33333333-3333-3333-3333-333333333303"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111113"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 3, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 3, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 3, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Medium, },
            new AnnouncementHistory { Id = Guid.Parse("33333333-3333-3333-3333-333333333304"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111114"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 4, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "イベント", PostDateTime = new DateTimeOffset(2025, 1, 4, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 4, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Low, },
            new AnnouncementHistory { Id = Guid.Parse("33333333-3333-3333-3333-333333333305"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111115"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 5, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "イベント", PostDateTime = new DateTimeOffset(2025, 1, 5, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 5, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Critical, },
            new AnnouncementHistory { Id = Guid.Parse("33333333-3333-3333-3333-333333333306"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111116"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 6, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "更新", PostDateTime = new DateTimeOffset(2025, 1, 6, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 6, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.High, },
            new AnnouncementHistory { Id = Guid.Parse("33333333-3333-3333-3333-333333333307"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111117"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 7, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "更新", PostDateTime = new DateTimeOffset(2025, 1, 7, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 7, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Medium, },
            new AnnouncementHistory { Id = Guid.Parse("33333333-3333-3333-3333-333333333308"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111118"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 8, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "重要", PostDateTime = new DateTimeOffset(2025, 1, 8, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 8, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Low, },
            new AnnouncementHistory { Id = Guid.Parse("33333333-3333-3333-3333-333333333309"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111119"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 9, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "重要", PostDateTime = new DateTimeOffset(2025, 1, 9, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 9, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Critical, },
            new AnnouncementHistory { Id = Guid.Parse("33333333-3333-3333-3333-333333333310"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111120"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 10, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 10, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 10, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Medium, },
            new AnnouncementHistory { Id = Guid.Parse("33333333-3333-3333-3333-333333333311"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111121"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 11, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 11, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 11, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.High, },
            new AnnouncementHistory { Id = Guid.Parse("33333333-3333-3333-3333-333333333312"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111122"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 12, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "イベント", PostDateTime = new DateTimeOffset(2025, 1, 12, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 12, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Low, },
            new AnnouncementHistory { Id = Guid.Parse("33333333-3333-3333-3333-333333333313"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111123"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 13, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "イベント", PostDateTime = new DateTimeOffset(2025, 1, 13, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 13, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Critical, },
            new AnnouncementHistory { Id = Guid.Parse("33333333-3333-3333-3333-333333333314"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111124"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 14, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "更新", PostDateTime = new DateTimeOffset(2025, 1, 14, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 14, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Medium, },
            new AnnouncementHistory { Id = Guid.Parse("33333333-3333-3333-3333-333333333315"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111125"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 15, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "更新", PostDateTime = new DateTimeOffset(2025, 1, 15, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 15, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.High, },
            new AnnouncementHistory { Id = Guid.Parse("33333333-3333-3333-3333-333333333316"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111126"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 16, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 16, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 16, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Low, },
            new AnnouncementHistory { Id = Guid.Parse("33333333-3333-3333-3333-333333333317"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111127"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 17, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "重要", PostDateTime = new DateTimeOffset(2025, 1, 17, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 17, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Critical, },
            new AnnouncementHistory { Id = Guid.Parse("33333333-3333-3333-3333-333333333318"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111128"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 18, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 18, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 18, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Medium, },
            new AnnouncementHistory { Id = Guid.Parse("33333333-3333-3333-3333-333333333319"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111129"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 19, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "イベント", PostDateTime = new DateTimeOffset(2025, 1, 19, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 19, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.High, },
            new AnnouncementHistory { Id = Guid.Parse("33333333-3333-3333-3333-333333333320"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111130"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 20, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 20, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 20, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Medium, },
            new AnnouncementHistory { Id = Guid.Parse("33333333-3333-3333-3333-333333333321"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111131"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 21, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 21, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 21, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Low, },
            new AnnouncementHistory { Id = Guid.Parse("33333333-4444-3333-3333-333333333301"), AnnouncementId = Guid.Parse("11111111-2222-1111-1111-111111111111"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 21, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "テスト", PostDateTime = new DateTimeOffset(2025, 1, 21, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 21, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Low, },
            new AnnouncementHistory { Id = Guid.Parse("33333333-5555-3333-3333-333333333301"), AnnouncementId = Guid.Parse("11111111-3333-1111-1111-111111111111"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2025, 1, 21, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Create, Category = "テスト", PostDateTime = new DateTimeOffset(2025, 1, 21, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 21, 0, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Low, },
            new AnnouncementHistory { Id = Guid.Parse("39999999-3333-3333-3333-333333333333"), AnnouncementId = Guid.Parse("19999999-1111-1111-1111-111111111111"), ChangedBy = "system", CreatedAt = new DateTimeOffset(2011, 1, 21, 0, 0, 0, TimeSpan.Zero), OperationType = OperationType.Delete, Category = "一般", PostDateTime = new DateTimeOffset(2018, 1, 21, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2019, 1, 1, 21, 0, 0, TimeSpan.Zero), DisplayPriority = DisplayPriority.Critical, });
    }
}
