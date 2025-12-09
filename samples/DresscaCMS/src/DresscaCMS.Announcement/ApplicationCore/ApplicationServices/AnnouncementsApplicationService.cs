using DresscaCMS.Announcement.ApplicationCore.RepositoryInterfaces;
using DresscaCMS.Announcement.Resources;
using Microsoft.Extensions.Logging;

namespace DresscaCMS.Announcement.ApplicationCore.ApplicationServices;

/// <summary>
///  お知らせメッセージに関するユーズケースを提供するアプリケーションサービスです。
/// </summary>
public class AnnouncementsApplicationService
{
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

        // 言語コードごとのタイトル掲載の優先順位を取得します。
        var languageOrder = LanguagePriorityProvider.GetLanguageOrderMap();

        IReadOnlyCollection<Infrastructures.Entities.Announcement> titleSelectedAnnouncements =
            announcements
                .Select(a =>
                {
                    var selectedContent = a.Contents?
                        .OrderBy(c =>
                            languageOrder.TryGetValue(c.LanguageCode, out var priority)
                                ? priority
                                : int.MaxValue)
                        .FirstOrDefault();

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
                            ? Array.Empty<Infrastructures.Entities.AnnouncementContent>()
                            : new List<Infrastructures.Entities.AnnouncementContent> { selectedContent },
                    };
                })
                .ToArray();

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
            LogMessages.AnnouncementsApplicationService_GetPagedAnnouncementsEnd,
            result.PageNumber,
            result.PageSize);

        return result;
    }
}
