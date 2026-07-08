using DresscaCMS.Announcement.Infrastructures.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DresscaCMS.Announcement.Infrastructures.Configurations;

/// <summary>
/// <see cref="AnnouncementContent"/> テーブルエンティティの構成を提供します。
/// </summary>
internal class AnnouncementContentConfiguration : IEntityTypeConfiguration<AnnouncementContent>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<AnnouncementContent> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("AnnouncementContents");

        builder.HasKey(e => e.Id);

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

        builder.HasData(
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-000000000001"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000001"), LanguageCode = "ja", Title = "お知らせ 1", Message = "内容 1", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-000000000002"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000002"), LanguageCode = "ja", Title = "お知らせ 2", Message = "内容 2", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-000000000003"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000003"), LanguageCode = "ja", Title = "お知らせ 3", Message = "内容 3", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-000000000004"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000004"), LanguageCode = "ja", Title = "お知らせ 4", Message = "内容 4", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-000000000005"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000005"), LanguageCode = "ja", Title = "お知らせ 5", Message = "内容 5", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-000000000006"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000006"), LanguageCode = "ja", Title = "お知らせ 6", Message = "内容 6", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-000000000007"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000007"), LanguageCode = "ja", Title = "お知らせ 7", Message = "内容 7", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-000000000008"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000008"), LanguageCode = "ja", Title = "お知らせ 8", Message = "内容 8", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-000000000009"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000009"), LanguageCode = "ja", Title = "お知らせ 9", Message = "内容 9", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-00000000000a"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-00000000000a"), LanguageCode = "ja", Title = "お知らせ 10", Message = "内容 10", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-00000000000b"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-00000000000b"), LanguageCode = "ja", Title = "お知らせ 11", Message = "内容 11", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-00000000000c"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-00000000000c"), LanguageCode = "ja", Title = "お知らせ 12", Message = "内容 12", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-00000000000d"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-00000000000d"), LanguageCode = "ja", Title = "お知らせ 13", Message = "内容 13", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-00000000000e"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-00000000000e"), LanguageCode = "ja", Title = "お知らせ 14", Message = "内容 14", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-00000000000f"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-00000000000f"), LanguageCode = "ja", Title = "お知らせ 15", Message = "内容 15", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-000000000010"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000010"), LanguageCode = "ja", Title = "お知らせ 16", Message = "内容 16", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-000000000011"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000011"), LanguageCode = "ja", Title = "お知らせ 17", Message = "内容 17", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-000000000012"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000012"), LanguageCode = "es", Title = "Anuncio 18", Message = "Detalles 18", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-000000000013"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000013"), LanguageCode = "zh", Title = "公告 19", Message = "详情 19", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-000000000014"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000014"), LanguageCode = "en", Title = "Notice 20", Message = "Details 20", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-000000000015"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000015"), LanguageCode = "ja", Title = "お知らせ 21", Message = "内容 21", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-000000000016"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000016"), LanguageCode = "ja", Title = "お知らせ 削除済み", Message = "内容 削除済み", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-000000000017"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000017"), LanguageCode = "en", Title = "英語 English", Message = "内容", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-000000000018"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000017"), LanguageCode = "es", Title = "スペイン語 español", Message = "内容", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-000000000019"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000018"), LanguageCode = "ja", Title = "日本語", Message = "内容", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-00000000001a"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000018"), LanguageCode = "en", Title = "英語 English", Message = "内容", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-00000000001b"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000018"), LanguageCode = "zh", Title = "中国語 中文", Message = "内容", LinkedUrl = "https://maris.alesinfiny.org/" },
            new AnnouncementContent { Id = Guid.Parse("0193ae97-b800-7002-8001-00000000001c"), AnnouncementId = Guid.Parse("0193ae97-b800-7001-8001-000000000018"), LanguageCode = "es", Title = "スペイン語 español", Message = "内容", LinkedUrl = "https://maris.alesinfiny.org/" });
    }
}
