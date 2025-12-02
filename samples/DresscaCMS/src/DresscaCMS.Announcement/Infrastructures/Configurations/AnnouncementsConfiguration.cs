using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DresscaCMS.Announcement.Infrastructures.Configurations;

/// <summary>
///  <see cref="Announcement"/> テーブルエンティティの構成を提供します。
/// </summary>
internal class AnnouncementsConfiguration : IEntityTypeConfiguration<Entities.Annoucements>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Entities.Annoucements> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // お知らせメッセージ
        builder.ToTable("Announcements");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
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
            new Entities.Annoucements { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero), DisplayPriority = 1, CreatedAt = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Annoucements { Id = Guid.Parse("11111111-1111-1111-1111-111111111112"), Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 2, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 2, 0, 0, 0, TimeSpan.Zero), DisplayPriority = 2, CreatedAt = new DateTimeOffset(2025, 1, 2, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 2, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Annoucements { Id = Guid.Parse("11111111-1111-1111-1111-111111111113"), Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 3, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 3, 0, 0, 0, TimeSpan.Zero), DisplayPriority = 3, CreatedAt = new DateTimeOffset(2025, 1, 3, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 3, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Annoucements { Id = Guid.Parse("11111111-1111-1111-1111-111111111114"), Category = "イベント", PostDateTime = new DateTimeOffset(2025, 1, 4, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 4, 0, 0, 0, TimeSpan.Zero), DisplayPriority = 4, CreatedAt = new DateTimeOffset(2025, 1, 4, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 4, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Annoucements { Id = Guid.Parse("11111111-1111-1111-1111-111111111115"), Category = "イベント", PostDateTime = new DateTimeOffset(2025, 1, 5, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 5, 0, 0, 0, TimeSpan.Zero), DisplayPriority = 5, CreatedAt = new DateTimeOffset(2025, 1, 5, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 5, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Annoucements { Id = Guid.Parse("11111111-1111-1111-1111-111111111116"), Category = "更新", PostDateTime = new DateTimeOffset(2025, 1, 6, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 6, 0, 0, TimeSpan.Zero), DisplayPriority = 6, CreatedAt = new DateTimeOffset(2025, 1, 6, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 6, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Annoucements { Id = Guid.Parse("11111111-1111-1111-1111-111111111117"), Category = "更新", PostDateTime = new DateTimeOffset(2025, 1, 7, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 7, 0, 0, TimeSpan.Zero), DisplayPriority = 7, CreatedAt = new DateTimeOffset(2025, 1, 7, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 7, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Annoucements { Id = Guid.Parse("11111111-1111-1111-1111-111111111118"), Category = "重要", PostDateTime = new DateTimeOffset(2025, 1, 8, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 8, 0, 0, TimeSpan.Zero), DisplayPriority = 8, CreatedAt = new DateTimeOffset(2025, 1, 8, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 8, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Annoucements { Id = Guid.Parse("11111111-1111-1111-1111-111111111119"), Category = "重要", PostDateTime = new DateTimeOffset(2025, 1, 9, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 9, 0, 0, TimeSpan.Zero), DisplayPriority = 9, CreatedAt = new DateTimeOffset(2025, 1, 9, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 9, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Annoucements { Id = Guid.Parse("11111111-1111-1111-1111-111111111120"), Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 10, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 10, 0, 0, TimeSpan.Zero), DisplayPriority = 10, CreatedAt = new DateTimeOffset(2025, 1, 10, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 10, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Annoucements { Id = Guid.Parse("11111111-1111-1111-1111-111111111121"), Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 11, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 11, 0, 0, TimeSpan.Zero), DisplayPriority = 11, CreatedAt = new DateTimeOffset(2025, 1, 11, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 11, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Annoucements { Id = Guid.Parse("11111111-1111-1111-1111-111111111122"), Category = "イベント", PostDateTime = new DateTimeOffset(2025, 1, 12, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 12, 0, 0, 0, TimeSpan.Zero), DisplayPriority = 12, CreatedAt = new DateTimeOffset(2025, 1, 12, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 12, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Annoucements { Id = Guid.Parse("11111111-1111-1111-1111-111111111123"), Category = "イベント", PostDateTime = new DateTimeOffset(2025, 1, 13, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 13, 0, 0, 0, TimeSpan.Zero), DisplayPriority = 13, CreatedAt = new DateTimeOffset(2025, 1, 13, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 13, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Annoucements { Id = Guid.Parse("11111111-1111-1111-1111-111111111124"), Category = "更新", PostDateTime = new DateTimeOffset(2025, 1, 14, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 14, 0, 0, TimeSpan.Zero), DisplayPriority = 14, CreatedAt = new DateTimeOffset(2025, 1, 14, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 14, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Annoucements { Id = Guid.Parse("11111111-1111-1111-1111-111111111125"), Category = "更新", PostDateTime = new DateTimeOffset(2025, 1, 15, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 15, 0, 0, TimeSpan.Zero), DisplayPriority = 15, CreatedAt = new DateTimeOffset(2025, 1, 15, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 15, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Annoucements { Id = Guid.Parse("11111111-1111-1111-1111-111111111126"), Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 16, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 16, 0, 0, TimeSpan.Zero), DisplayPriority = 16, CreatedAt = new DateTimeOffset(2025, 1, 16, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 16, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Annoucements { Id = Guid.Parse("11111111-1111-1111-1111-111111111127"), Category = "重要", PostDateTime = new DateTimeOffset(2025, 1, 17, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 17, 0, 0, TimeSpan.Zero), DisplayPriority = 17, CreatedAt = new DateTimeOffset(2025, 1, 17, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 17, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Annoucements { Id = Guid.Parse("11111111-1111-1111-1111-111111111128"), Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 18, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 18, 0, 0, TimeSpan.Zero), DisplayPriority = 18, CreatedAt = new DateTimeOffset(2025, 1, 18, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 18, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Annoucements { Id = Guid.Parse("11111111-1111-1111-1111-111111111129"), Category = "イベント", PostDateTime = new DateTimeOffset(2025, 1, 19, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 19, 0, 0, 0, TimeSpan.Zero), DisplayPriority = 19, CreatedAt = new DateTimeOffset(2025, 1, 19, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 19, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Annoucements { Id = Guid.Parse("11111111-1111-1111-1111-111111111130"), Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 20, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 20, 0, 0, TimeSpan.Zero), DisplayPriority = 20, CreatedAt = new DateTimeOffset(2025, 1, 20, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 20, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Annoucements { Id = Guid.Parse("11111111-1111-1111-1111-111111111131"), Category = "一般", PostDateTime = new DateTimeOffset(2025, 1, 21, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2026, 1, 1, 21, 0, 0, TimeSpan.Zero), DisplayPriority = 21, CreatedAt = new DateTimeOffset(2025, 1, 21, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2025, 1, 21, 0, 0, 0, TimeSpan.Zero), IsDeleted = false },
            new Entities.Annoucements { Id = Guid.Parse("19999999-1111-1111-1111-111111111111"), Category = "一般", PostDateTime = new DateTimeOffset(2018, 1, 21, 0, 0, 0, TimeSpan.Zero), ExpireDateTime = new DateTimeOffset(2019, 1, 1, 21, 0, 0, TimeSpan.Zero), DisplayPriority = 11, CreatedAt = new DateTimeOffset(2010, 1, 21, 0, 0, 0, TimeSpan.Zero), ChangedAt = new DateTimeOffset(2011, 1, 21, 0, 0, 0, TimeSpan.Zero), IsDeleted = true });
    }
}
