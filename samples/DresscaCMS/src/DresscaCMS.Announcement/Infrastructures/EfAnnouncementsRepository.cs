using System.Linq;
using DresscaCMS.Announcement.ApplicationCore.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DresscaCMS.Announcement.Infrastructures;

/// <summary>
///  お知らせメッセージの取得を行う EF Core リポジトリ実装です。
/// </summary>
internal class EfAnnouncementsRepository : IAnnouncementsRepository
{
    private readonly IDbContextFactory<AnnouncementDbContext> dbContextFactory;

    /// <summary>
    ///  データアクセスに使用する <see cref="IDbContextFactory{AnnouncementDbContext}"/> を指定して
    ///  <see cref="EfAnnouncementsRepository"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="dbContextFactory">データアクセスに使用する <see cref="IDbContextFactory{AnnouncementDbContext}"/> オブジェクト。</param>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="dbContextFactory"/> が <see langword="null" />です。
    /// </exception>
    public EfAnnouncementsRepository(IDbContextFactory<AnnouncementDbContext> dbContextFactory)
        => this.dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));

    /// <summary>
    ///  ページ番号とページサイズを指定して、論理削除されていないお知らせメッセージを取得します。
    /// </summary>
    /// <param name="pageNumber">取得するページ番号。</param>
    /// <param name="pageSize">1ページあたりの取得件数。</param>
    /// <param name="cancellationToken">キャンセル用トークン。</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<IReadOnlyCollection<Entities.Announcement>> FindByPageNumberAndPageSizeAsync(
      int pageNumber,
      int pageSize,
      CancellationToken cancellationToken)
    {
        await using var dbContext = await this.dbContextFactory.CreateDbContextAsync(cancellationToken);

        var query = dbContext.Announcements
            .Where(a => !a.IsDeleted)
            .Where(a => a.Contents.Any())
            .Include(a => a.Contents)
            .OrderByDescending(a => a.PostDateTime);

        var announcements = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return announcements;
    }

    /// <summary>
    ///  論理削除されていないお知らせメッセージの総件数を取得します。
    /// </summary>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<int> CountNotDeletedAsync(CancellationToken cancellationToken)
    {
        await using var dbContext = await this.dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await dbContext.Announcements
            .Where(x => !x.IsDeleted)
            .CountAsync(cancellationToken);
    }

    /// <summary>
    ///  お知らせメッセージを登録します。
    /// </summary>
    /// <param name="announcement">登録するお知らせメッセージ。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>登録したお知らせメッセージの ID。</returns>
    public async Task<Guid> CreateAnnouncementAsync(
        Entities.Announcement announcement,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await this.dbContextFactory.CreateDbContextAsync(cancellationToken);

        // ID を生成
        announcement.Id = Guid.NewGuid();

        // タイムスタンプを設定
        var now = DateTimeOffset.UtcNow;
        announcement.CreatedAt = now;
        announcement.ChangedAt = now;
        announcement.IsDeleted = false;

        dbContext.Announcements.Add(announcement);
        await dbContext.SaveChangesAsync(cancellationToken);

        return announcement.Id;
    }

    /// <summary>
    ///  お知らせコンテンツを登録します。
    /// </summary>
    /// <param name="content">登録するお知らせコンテンツ。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    public async Task CreateAnnouncementContentAsync(
        Entities.AnnouncementContent content,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await this.dbContextFactory.CreateDbContextAsync(cancellationToken);

        // ID を生成
        content.Id = Guid.NewGuid();

        dbContext.AnnouncementContents.Add(content);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    ///  お知らせメッセージ履歴を登録します。
    /// </summary>
    /// <param name="history">登録するお知らせメッセージ履歴。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    public async Task CreateAnnouncementHistoryAsync(
        Entities.AnnouncementHistory history,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await this.dbContextFactory.CreateDbContextAsync(cancellationToken);

        dbContext.AnnouncementHistories.Add(history);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    ///  お知らせコンテンツ履歴を登録します。
    /// </summary>
    /// <param name="contentHistory">登録するお知らせコンテンツ履歴。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    public async Task CreateAnnouncementContentHistoryAsync(
        Entities.AnnouncementContentHistory contentHistory,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await this.dbContextFactory.CreateDbContextAsync(cancellationToken);

        dbContext.AnnouncementContentHistories.Add(contentHistory);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    ///  お知らせメッセージ ID を指定して、お知らせメッセージ、お知らせコンテンツ、更新履歴を取得します。
    /// </summary>
    /// <param name="announcementId">お知らせメッセージ ID。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>お知らせメッセージ、お知らせコンテンツ、更新履歴を含む結果。お知らせメッセージが存在しない場合は null。</returns>
    public async Task<ApplicationCore.GetAnnouncementWithHistoriesResult?> FindByAnnouncementWithContentAsync(
        Guid announcementId,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await this.dbContextFactory.CreateDbContextAsync(cancellationToken);

        // お知らせメッセージを取得（論理削除されていないもの）
        var announcement = await dbContext.Announcements
            .Where(a => a.Id == announcementId && !a.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        if (announcement == null)
        {
            return null;
        }

        // 言語コードの優先順位を取得
        var languageOrder = ApplicationCore.LanguagePriorityProvider.GetLanguageOrderMap();

        // お知らせコンテンツを取得（まずデータベースから取得してからクライアント側でソート）
        var contentsFromDb = await dbContext.AnnouncementContents
            .Where(c => c.AnnouncementId == announcementId)
            .ToListAsync(cancellationToken);

        // クライアント側で言語コードの優先順にソート
        var contents = contentsFromDb
            .OrderBy(c => languageOrder.ContainsKey(c.LanguageCode)
                ? languageOrder[c.LanguageCode]
                : int.MaxValue)
            .ToList();

        // お知らせメッセージ更新履歴を取得（作成日時の降順）
        var histories = await dbContext.AnnouncementHistories
            .Where(h => h.AnnouncementId == announcementId)
            .Include(h => h.Contents)
            .OrderByDescending(h => h.CreatedAt)
            .ToListAsync(cancellationToken);

        // 各履歴のコンテンツを言語コードの優先順でソート（クライアント側）
        foreach (var history in histories)
        {
            history.Contents = history.Contents
                .OrderBy(c => languageOrder.ContainsKey(c.LanguageCode)
                    ? languageOrder[c.LanguageCode]
                    : int.MaxValue)
                .ToList();
        }

        return new ApplicationCore.GetAnnouncementWithHistoriesResult
        {
            Announcement = announcement,
            Contents = contents,
            Histories = histories,
        };
    }

    /// <summary>
    ///  お知らせメッセージを更新します。
    /// </summary>
    /// <param name="announcement">更新するお知らせメッセージ。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    public async Task UpdateAnnouncementAsync(
        Entities.Announcement announcement,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await this.dbContextFactory.CreateDbContextAsync(cancellationToken);

        var existing = await dbContext.Announcements
            .Where(a => a.Id == announcement.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (existing == null)
        {
            throw new InvalidOperationException($"お知らせメッセージが見つかりません。ID: {announcement.Id}");
        }

        // 更新
        existing.Category = announcement.Category;
        existing.PostDateTime = announcement.PostDateTime;
        existing.ExpireDateTime = announcement.ExpireDateTime;
        existing.DisplayPriority = announcement.DisplayPriority;
        existing.IsDeleted = announcement.IsDeleted;
        existing.ChangedAt = announcement.ChangedAt;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    ///  お知らせメッセージ ID を指定して、お知らせコンテンツを取得します。
    /// </summary>
    /// <param name="announcementId">お知らせメッセージ ID。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>お知らせコンテンツのリスト。</returns>
    public async Task<IReadOnlyCollection<Entities.AnnouncementContent>> FindAnnouncementContentsByAnnouncementIdAsync(
        Guid announcementId,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await this.dbContextFactory.CreateDbContextAsync(cancellationToken);

        var contents = await dbContext.AnnouncementContents
            .Where(c => c.AnnouncementId == announcementId)
            .ToListAsync(cancellationToken);

        return contents;
    }

    /// <summary>
    ///  お知らせコンテンツを更新します。
    /// </summary>
    /// <param name="content">更新するお知らせコンテンツ。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    public async Task UpdateAnnouncementContentAsync(
        Entities.AnnouncementContent content,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await this.dbContextFactory.CreateDbContextAsync(cancellationToken);

        var existing = await dbContext.AnnouncementContents
            .Where(c => c.Id == content.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (existing == null)
        {
            throw new InvalidOperationException($"お知らせコンテンツが見つかりません。ID: {content.Id}");
        }

        // 更新
        existing.LanguageCode = content.LanguageCode;
        existing.Title = content.Title;
        existing.Message = content.Message;
        existing.LinkedUrl = content.LinkedUrl;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    ///  お知らせコンテンツを削除します。
    /// </summary>
    /// <param name="contentIds">削除するお知らせコンテンツの ID リスト。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    public async Task DeleteAnnouncementContentsAsync(
        IEnumerable<Guid> contentIds,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await this.dbContextFactory.CreateDbContextAsync(cancellationToken);
        await dbContext.AnnouncementContents
            .Where(c => contentIds.Contains(c.Id))
            .ExecuteDeleteAsync(cancellationToken);
    }
}
