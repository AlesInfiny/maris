using DresscaCMS.Announcement.Infrastructures.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DresscaCMS.Announcement.Infrastructures.Configurations;

/// <summary>
///  <see cref="AnnouncementContentHistory"/> テーブルエンティティの構成を提供します。
/// </summary>
internal class AnnouncementContentHistoryConfiguration : IEntityTypeConfiguration<AnnouncementContentHistory>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<AnnouncementContentHistory> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("AnnouncementContentHistory");

        builder.HasKey(e => e.Id);

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

        builder.Property(e => e.LinkedUrl)
            .HasMaxLength(1024);

        builder.HasData(
            new AnnouncementContentHistory
            {
                Id = Guid.Parse("49999999-4444-4444-4444-444444444444"),
                AnnouncementHistoryId = Guid.Parse("39999999-3333-3333-3333-333333333333"),
                LanguageCode = "ja",
                Title = "お知らせ 削除済み",
                Message = "内容 削除済み",
                LinkedUrl = "https://maris.alesinfiny.org/",
            });
    }
}
