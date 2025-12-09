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

        // 掲載開始日時の降順でソートする
        var sortedByPostDatetimeAnnouncements = announcements
                            .OrderByDescending(a => a.PostDateTime)
                            .ToArray();

        // 言語コード → 優先順位（0,1,2,...）のマップを作成
        var languageOrder = LanguagePriorityProvider.GetLanguageOrderMap();

        IReadOnlyCollection<Infrastructures.Entities.Announcement> titleSelectedAnnouncements =
            sortedByPostDatetimeAnnouncements
                .Select(a =>
                {
                    var selectedContent = a.Contents?
                        .OrderBy(c =>
                            languageOrder.TryGetValue(c.LanguageCode, out var priority)
                                ? priority
                                : int.MaxValue) // 優先リストにない言語は最後に回す
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
                            ? new List<Infrastructures.Entities.AnnouncementContent>()
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

    /// <summary>
    ///  指定された ID のお知らせメッセージを、コンテンツおよび履歴と共に取得します。
    /// </summary>
    /// <param name="announcementId">お知らせメッセージ ID。</param>
    /// <param name="cancellationToken">キャンセル用トークン。</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<GetAnnouncementAndHistoriesResult> GetAnnouncementAndHistoriesByIdAsync(
        Guid announcementId,
        CancellationToken cancellationToken = default)
    {
        // ------------------------------
        // 業務開始処理
        // ------------------------------
        this.logger.LogInformation(
            "お知らせメッセージ ID: {announcementId} のお知らせメッセージと履歴を取得します。",
            announcementId);

        // ------------------------------
        // 業務メイン処理
        // ------------------------------
        var announcement = await this.announcementsRepository
            .FindByIdWithContentsAndHistoriesAsync(announcementId, cancellationToken);

        // ------------------------------
        // 業務終了処理
        // ------------------------------
        this.logger.LogInformation(
            "お知らせメッセージ ID: {announcementId} のお知らせメッセージと履歴を取得しました。",
            announcementId);

        return new GetAnnouncementAndHistoriesResult
        {
            Announcement = announcement,
        };
    }

    /// <summary>
    ///  お知らせメッセージとお知らせコンテンツを削除します。
    /// </summary>
    /// <param name="announcement">削除するお知らせメッセージ。</param>
    /// <param name="changedBy">変更者のユーザー名。</param>
    /// <param name="cancellationToken">キャンセル用トークン。</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task DeleteAnnouncementAndContentAsync(
        Infrastructures.Entities.Announcement announcement,
        string changedBy,
        CancellationToken cancellationToken = default)
    {
        // ------------------------------
        // 業務開始処理
        // ------------------------------
        ArgumentNullException.ThrowIfNull(announcement);
        ArgumentException.ThrowIfNullOrWhiteSpace(changedBy);

        this.logger.LogInformation(
            "お知らせメッセージ ID: {announcementId} を削除します。",
            announcement.Id);

        // ------------------------------
        // 業務メイン処理
        // ------------------------------

        // お知らせメッセージを論理削除
        announcement.IsDeleted = true;
        announcement.ChangedAt = DateTimeOffset.Now;

        var updateCount = await this.announcementsRepository
            .UpdateAnnouncementAsync(announcement, cancellationToken);

        if (updateCount == 0)
        {
            throw new InvalidOperationException("お知らせメッセージの削除に失敗しました。");
        }

        // お知らせコンテンツを削除
        await this.announcementsRepository
            .DeleteAnnouncementContentsAsync(announcement.Id, cancellationToken);

        // お知らせメッセージ履歴を作成
        var history = new Infrastructures.Entities.AnnouncementHistory
        {
            Id = Guid.NewGuid(),
            AnnouncementId = announcement.Id,
            ChangedBy = changedBy,
            CreatedAt = DateTimeOffset.Now,
            OperationType = OperationType.Delete,
            Category = announcement.Category,
            PostDateTime = announcement.PostDateTime,
            ExpireDateTime = announcement.ExpireDateTime,
            DisplayPriority = announcement.DisplayPriority,
        };

        await this.announcementsRepository
            .CreateAnnouncementHistoryAsync(history, cancellationToken);

        // ------------------------------
        // 業務終了処理
        // ------------------------------
        this.logger.LogInformation(
            "お知らせメッセージ ID: {announcementId} を削除しました。",
            announcement.Id);
    }
}
