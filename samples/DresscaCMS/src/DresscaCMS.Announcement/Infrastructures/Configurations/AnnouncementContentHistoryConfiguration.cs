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
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444401"), AnnouncementHistoryId = Guid.Parse("33333333-3333-3333-3333-333333333301"), LanguageCode = "ja", Title = "お知らせ 1", Message = "内容 1", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444402"), AnnouncementHistoryId = Guid.Parse("33333333-3333-3333-3333-333333333302"), LanguageCode = "ja", Title = "お知らせ 2", Message = "内容 2", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444403"), AnnouncementHistoryId = Guid.Parse("33333333-3333-3333-3333-333333333303"), LanguageCode = "ja", Title = "お知らせ 3", Message = "内容 3", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444404"), AnnouncementHistoryId = Guid.Parse("33333333-3333-3333-3333-333333333304"), LanguageCode = "ja", Title = "お知らせ 4", Message = "内容 4", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444405"), AnnouncementHistoryId = Guid.Parse("33333333-3333-3333-3333-333333333305"), LanguageCode = "ja", Title = "お知らせ 5", Message = "内容 5", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444406"), AnnouncementHistoryId = Guid.Parse("33333333-3333-3333-3333-333333333306"), LanguageCode = "ja", Title = "お知らせ 6", Message = "内容 6", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444407"), AnnouncementHistoryId = Guid.Parse("33333333-3333-3333-3333-333333333307"), LanguageCode = "ja", Title = "お知らせ 7", Message = "内容 7", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444408"), AnnouncementHistoryId = Guid.Parse("33333333-3333-3333-3333-333333333308"), LanguageCode = "ja", Title = "お知らせ 8", Message = "内容 8", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444409"), AnnouncementHistoryId = Guid.Parse("33333333-3333-3333-3333-333333333309"), LanguageCode = "ja", Title = "お知らせ 9", Message = "内容 9", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444410"), AnnouncementHistoryId = Guid.Parse("33333333-3333-3333-3333-333333333310"), LanguageCode = "ja", Title = "お知らせ 10", Message = "内容 10", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444411"), AnnouncementHistoryId = Guid.Parse("33333333-3333-3333-3333-333333333311"), LanguageCode = "ja", Title = "お知らせ 11", Message = "内容 11", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444412"), AnnouncementHistoryId = Guid.Parse("33333333-3333-3333-3333-333333333312"), LanguageCode = "ja", Title = "お知らせ 12", Message = "内容 12", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444413"), AnnouncementHistoryId = Guid.Parse("33333333-3333-3333-3333-333333333313"), LanguageCode = "ja", Title = "お知らせ 13", Message = "内容 13", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444414"), AnnouncementHistoryId = Guid.Parse("33333333-3333-3333-3333-333333333314"), LanguageCode = "ja", Title = "お知らせ 14", Message = "内容 14", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444415"), AnnouncementHistoryId = Guid.Parse("33333333-3333-3333-3333-333333333315"), LanguageCode = "ja", Title = "お知らせ 15", Message = "内容 15", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444416"), AnnouncementHistoryId = Guid.Parse("33333333-3333-3333-3333-333333333316"), LanguageCode = "ja", Title = "お知らせ 16", Message = "内容 16", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444417"), AnnouncementHistoryId = Guid.Parse("33333333-3333-3333-3333-333333333317"), LanguageCode = "ja", Title = "お知らせ 17", Message = "内容 17", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444418"), AnnouncementHistoryId = Guid.Parse("33333333-3333-3333-3333-333333333318"), LanguageCode = "es", Title = "Anuncio 18", Message = "Detalles 18", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444419"), AnnouncementHistoryId = Guid.Parse("33333333-3333-3333-3333-333333333319"), LanguageCode = "zh", Title = "公告 19", Message = "详情 19", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444420"), AnnouncementHistoryId = Guid.Parse("33333333-3333-3333-3333-333333333320"), LanguageCode = "en", Title = "Notice 20", Message = "Details 20", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444421"), AnnouncementHistoryId = Guid.Parse("33333333-3333-3333-3333-333333333321"), LanguageCode = "ja", Title = "お知らせ 21", Message = "内容 21", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444422"), AnnouncementHistoryId = Guid.Parse("33333333-4444-3333-3333-333333333301"), LanguageCode = "en", Title = "英語 English", Message = "内容", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444423"), AnnouncementHistoryId = Guid.Parse("33333333-4444-3333-3333-333333333301"), LanguageCode = "es", Title = "フランス語 français", Message = "内容", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444424"), AnnouncementHistoryId = Guid.Parse("33333333-5555-3333-3333-333333333301"), LanguageCode = "ja", Title = "日本語", Message = "内容", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444425"), AnnouncementHistoryId = Guid.Parse("33333333-5555-3333-3333-333333333301"), LanguageCode = "en", Title = "英語 English", Message = "内容", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444426"), AnnouncementHistoryId = Guid.Parse("33333333-5555-3333-3333-333333333301"), LanguageCode = "zh", Title = "中国語 中文", Message = "内容", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("44444444-4444-4444-4444-444444444427"), AnnouncementHistoryId = Guid.Parse("33333333-5555-3333-3333-333333333301"), LanguageCode = "es", Title = "スペイン語 español", Message = "内容", LinkedUrl = "https://maris.alesinfiny.org/", },
            new AnnouncementContentHistory { Id = Guid.Parse("49999999-4444-4444-4444-444444444444"), AnnouncementHistoryId = Guid.Parse("39999999-3333-3333-3333-333333333333"), LanguageCode = "ja", Title = "お知らせ 削除済み", Message = "内容 削除済み", LinkedUrl = "https://maris.alesinfiny.org/", });
    }
}
