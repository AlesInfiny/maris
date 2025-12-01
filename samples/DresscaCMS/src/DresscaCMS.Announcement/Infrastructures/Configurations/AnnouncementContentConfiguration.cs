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
            new AnnouncementContent { Id = Guid.Parse("22222222-2222-2222-2222-222222222201"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111111"), LanguageCode = "ja", Title = "お知らせ 1", Message = "内容 1", LinkedUrl = "https://maris.alesinfiny.org/", RowVersion = [] },
            new AnnouncementContent { Id = Guid.Parse("22222222-2222-2222-2222-222222222202"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111112"), LanguageCode = "ja", Title = "お知らせ 2", Message = "内容 2", LinkedUrl = "https://maris.alesinfiny.org/", RowVersion = [] },
            new AnnouncementContent { Id = Guid.Parse("22222222-2222-2222-2222-222222222203"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111113"), LanguageCode = "ja", Title = "お知らせ 3", Message = "内容 3", LinkedUrl = "https://maris.alesinfiny.org/", RowVersion = [] },
            new AnnouncementContent { Id = Guid.Parse("22222222-2222-2222-2222-222222222204"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111114"), LanguageCode = "ja", Title = "お知らせ 4", Message = "内容 4", LinkedUrl = "https://maris.alesinfiny.org/", RowVersion = [] },
            new AnnouncementContent { Id = Guid.Parse("22222222-2222-2222-2222-222222222205"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111115"), LanguageCode = "ja", Title = "お知らせ 5", Message = "内容 5", LinkedUrl = "https://maris.alesinfiny.org/", RowVersion = [] },
            new AnnouncementContent { Id = Guid.Parse("22222222-2222-2222-2222-222222222206"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111116"), LanguageCode = "ja", Title = "お知らせ 6", Message = "内容 6", LinkedUrl = "https://maris.alesinfiny.org/", RowVersion = [] },
            new AnnouncementContent { Id = Guid.Parse("22222222-2222-2222-2222-222222222207"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111117"), LanguageCode = "ja", Title = "お知らせ 7", Message = "内容 7", LinkedUrl = "https://maris.alesinfiny.org/", RowVersion = [] },
            new AnnouncementContent { Id = Guid.Parse("22222222-2222-2222-2222-222222222208"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111118"), LanguageCode = "ja", Title = "お知らせ 8", Message = "内容 8", LinkedUrl = "https://maris.alesinfiny.org/", RowVersion = [] },
            new AnnouncementContent { Id = Guid.Parse("22222222-2222-2222-2222-222222222209"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111119"), LanguageCode = "ja", Title = "お知らせ 9", Message = "内容 9", LinkedUrl = "https://maris.alesinfiny.org/", RowVersion = [] },
            new AnnouncementContent { Id = Guid.Parse("22222222-2222-2222-2222-222222222210"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111120"), LanguageCode = "ja", Title = "お知らせ 10", Message = "内容 10", LinkedUrl = "https://maris.alesinfiny.org/", RowVersion = [] },
            new AnnouncementContent { Id = Guid.Parse("22222222-2222-2222-2222-222222222211"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111121"), LanguageCode = "ja", Title = "お知らせ 11", Message = "内容 11", LinkedUrl = "https://maris.alesinfiny.org/", RowVersion = [] },
            new AnnouncementContent { Id = Guid.Parse("22222222-2222-2222-2222-222222222212"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111122"), LanguageCode = "ja", Title = "お知らせ 12", Message = "内容 12", LinkedUrl = "https://maris.alesinfiny.org/", RowVersion = [] },
            new AnnouncementContent { Id = Guid.Parse("22222222-2222-2222-2222-222222222213"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111123"), LanguageCode = "ja", Title = "お知らせ 13", Message = "内容 13", LinkedUrl = "https://maris.alesinfiny.org/", RowVersion = [] },
            new AnnouncementContent { Id = Guid.Parse("22222222-2222-2222-2222-222222222214"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111124"), LanguageCode = "ja", Title = "お知らせ 14", Message = "内容 14", LinkedUrl = "https://maris.alesinfiny.org/", RowVersion = [] },
            new AnnouncementContent { Id = Guid.Parse("22222222-2222-2222-2222-222222222215"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111125"), LanguageCode = "ja", Title = "お知らせ 15", Message = "内容 15", LinkedUrl = "https://maris.alesinfiny.org/", RowVersion = [] },
            new AnnouncementContent { Id = Guid.Parse("22222222-2222-2222-2222-222222222216"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111126"), LanguageCode = "ja", Title = "お知らせ 16", Message = "内容 16", LinkedUrl = "https://maris.alesinfiny.org/", RowVersion = [] },
            new AnnouncementContent { Id = Guid.Parse("22222222-2222-2222-2222-222222222217"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111127"), LanguageCode = "ja", Title = "お知らせ 17", Message = "内容 17", LinkedUrl = "https://maris.alesinfiny.org/", RowVersion = [] },
            new AnnouncementContent { Id = Guid.Parse("22222222-2222-2222-2222-222222222218"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111128"), LanguageCode = "ja", Title = "お知らせ 18", Message = "内容 18", LinkedUrl = "https://maris.alesinfiny.org/", RowVersion = [] },
            new AnnouncementContent { Id = Guid.Parse("22222222-2222-2222-2222-222222222219"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111129"), LanguageCode = "ja", Title = "お知らせ 19", Message = "内容 19", LinkedUrl = "https://maris.alesinfiny.org/", RowVersion = [] },
            new AnnouncementContent { Id = Guid.Parse("22222222-2222-2222-2222-222222222220"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111130"), LanguageCode = "ja", Title = "お知らせ 20", Message = "内容 20", LinkedUrl = "https://maris.alesinfiny.org/", RowVersion = [] },
            new AnnouncementContent { Id = Guid.Parse("22222222-2222-2222-2222-222222222221"), AnnouncementId = Guid.Parse("11111111-1111-1111-1111-111111111131"), LanguageCode = "ja", Title = "お知らせ 21", Message = "内容 21", LinkedUrl = "https://maris.alesinfiny.org/", RowVersion = [] },
            new AnnouncementContent { Id = Guid.Parse("29999999-2222-2222-2222-222222222222"), AnnouncementId = Guid.Parse("19999999-1111-1111-1111-111111111111"), LanguageCode = "ja", Title = "お知らせ 削除済み", Message = "内容 削除済み", LinkedUrl = "https://maris.alesinfiny.org/", RowVersion = [] });
    }
}
