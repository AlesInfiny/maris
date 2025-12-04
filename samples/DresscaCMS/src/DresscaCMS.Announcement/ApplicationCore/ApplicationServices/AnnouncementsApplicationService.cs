using DresscaCMS.Announcement.ApplicationCore.RepositoryInterfaces;
using DresscaCMS.Announcement.Resources;
using Microsoft.Extensions.Logging;

namespace DresscaCMS.Announcement.ApplicationCore.ApplicationServices;

/// <summary>
///  お知らせメッセージに関するユーズケースを提供するアプリケーションサービスです。
/// </summary>
public class AnnouncementsApplicationService
{
    private static readonly string[] LanguagePriority =
        [
            "ja", "en", "zh", "es", // 規定の優先順
        ];

    private readonly IAnnouncementsRepository announcementsRepository;
    private readonly ILogger<AnnouncementsApplicationService> logger;

    /// <summary>
    ///  <see cref="AnnouncementsApplicationService"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="announcementsRepository">お知らせメッセージリポジトリ。</param>
    /// <param name="logger">ロガー。</param>
    public AnnouncementsApplicationService(
        IAnnouncementsRepository announcementsRepository,
        ILogger<AnnouncementsApplicationService> logger)
    {
        this.announcementsRepository = announcementsRepository;
        this.logger = logger;
    }

    /// <summary>
    ///  ページング付きでお知らせメッセージ一覧を取得します。
    /// </summary>
    /// <param name="pageNumber">取得するページ番号（1始まり）。</param>
    /// <param name="pageSize">1ページあたりの件数。</param>
    /// <param name="cancellationToken">キャンセル用トークン。</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<GetPagedAnnouncementsResult> GetPagedAnnouncementsAsync(
        int? pageNumber,
        int? pageSize,
        CancellationToken cancellationToken = default)
    {
        // ------------------------------
        // 業務開始処理
        // ------------------------------

        // 取得するページ番号が未指定か0以下の場合は1ページ目を設定します。
        var validatedPageNumber = Math.Max(1, pageNumber ?? 1);

        // 1ページあたりの件数が未指定か、10未満または201以上の場合は20件を設定します。
        var validatedPageSize = (pageSize is >= 10 and <= 200) ? pageSize.Value : 20;

        this.logger.LogInformation(
            LogMessages.AnnouncementsApplicationService_GetPagedAnnouncementsStart,
            validatedPageNumber,
            validatedPageSize);

        // ------------------------------
        // 業務メイン処理
        // ------------------------------

        // 論理削除されていないレコードの件数を取得します。
        var totalCount = await this.announcementsRepository.CountNotDeletedAsync(cancellationToken);

        // 総件数 0 件なら、お知らせメッセージの総件数に 0 を設定した戻り値を返してメソッドを終了します。
        if (totalCount == 0)
        {
            return
                new GetPagedAnnouncementsResult
                {
                    TotalCount = 0,
                };
        }

        // 最後のページ番号を計算します。
        int lastPageNumber = (totalCount + validatedPageSize - 1) / validatedPageSize;

        // pageNumber が最後のページより大きい場合は 1 ページ目に戻ります。
        if (validatedPageNumber > lastPageNumber)
        {
            validatedPageNumber = 1;
        }

        // 論理削除されていないお知らせメッセージ一覧を取得します。
        var announcements = await this.announcementsRepository
            .FindByPageNumberAndPageSizeAsync(validatedPageNumber, validatedPageSize, cancellationToken);

        // 掲載開始日時の降順でソートする
        var sortedByPostDatetimeAnnouncements = announcements
                            .OrderByDescending(a => a.PostDateTime)
                            .ToArray();

        // 業務ルールの言語コード優先順に従ってタイトルを取得します。
        var languagePriority = LanguagePriority.Distinct() // 重複を除く
        .ToArray();

        IReadOnlyCollection<Infrastructures.Entities.Announcement> titleSelectedAnnouncements = [.. sortedByPostDatetimeAnnouncements
            .Select(a =>
        {
            // 優先リストに含まれるコンテンツを優先順位で選択
            var selectedContent = a.Contents?
                .Where(c => languagePriority.Contains(c.LanguageCode))
                .OrderBy(c => Array.IndexOf(languagePriority, c.LanguageCode))
                .FirstOrDefault()
                ?? a.Contents?.FirstOrDefault(); // フォールバック

            // 新しい Announcement を作成して Contents を選択済みの 1 件のみにする
            return new Infrastructures.Entities.Announcement
            {
                Id = a.Id,
                Category = a.Category,
                PostDateTime = a.PostDateTime,
                ExpireDateTime = a.ExpireDateTime,
                DisplayPriority = a.DisplayPriority,
                IsDeleted = a.IsDeleted,
                CreatedAt = a.CreatedAt,
                ChangedAt = a.ChangedAt,
                Contents = selectedContent is null
                    ? []
                    : new List<Infrastructures.Entities.AnnouncementContent> { selectedContent },
            };
        })];

        // 表示開始件数・終了件数を計算します。
        int displayFrom = ((validatedPageNumber - 1) * validatedPageSize) + 1;
        int displayTo = Math.Min(validatedPageNumber * validatedPageSize, totalCount);

        // ------------------------------
        // 業務終了処理
        // ------------------------------
        var result = new GetPagedAnnouncementsResult
        {
            PageNumber = validatedPageNumber,
            PageSize = validatedPageSize,
            TotalCount = totalCount,
            LastPageNumber = lastPageNumber,
            DisplayFrom = displayFrom,
            DisplayTo = displayTo,
            Announcements = titleSelectedAnnouncements,
        };

        this.logger.LogInformation(
            LogMessages.AnnouncementsApplicationService_GetPagedAnnoucementeEnd,
            result.PageNumber,
            result.PageSize);

        return result;
    }
}
