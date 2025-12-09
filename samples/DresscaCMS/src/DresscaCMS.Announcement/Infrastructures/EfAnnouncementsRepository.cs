using System.Linq;
using DresscaCMS.Announcement.ApplicationCore;
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

    /// <inheritdoc/>
    public async Task<Entities.Announcement?> FindByIdWithContentsAndHistoriesAsync(
        Guid announcementId,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await this.dbContextFactory.CreateDbContextAsync(cancellationToken);

        var announcement = await dbContext.Announcements
            .Where(a => a.Id == announcementId && !a.IsDeleted)
            .Include(a => a.Contents)
            .Include(a => a.Histories.OrderByDescending(h => h.CreatedAt))
                .ThenInclude(h => h.Contents)
            .FirstOrDefaultAsync(cancellationToken);

        if (announcement == null)
        {
            return null;
        }

        // 言語コード → 優先順位（0,1,2,...）のマップを作成
        var languageOrder = LanguagePriorityProvider.GetLanguageOrderMap();

        // お知らせコンテンツを言語優先順でソート
        announcement.Contents = announcement.Contents
            .OrderBy(c => languageOrder.TryGetValue(c.LanguageCode, out var priority) ? priority : int.MaxValue)
            .ToList();

        // 履歴のコンテンツも言語優先順でソート
        foreach (var history in announcement.Histories)
        {
            history.Contents = history.Contents
                .OrderBy(c => languageOrder.TryGetValue(c.LanguageCode, out var priority) ? priority : int.MaxValue)
                .ToList();
        }

        return announcement;
    }

    /// <inheritdoc/>
    public async Task<int> UpdateAnnouncementAsync(
        Entities.Announcement announcement,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await this.dbContextFactory.CreateDbContextAsync(cancellationToken);

        dbContext.Announcements.Update(announcement);
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<int> DeleteAnnouncementContentsAsync(
        Guid announcementId,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await this.dbContextFactory.CreateDbContextAsync(cancellationToken);

        var contents = await dbContext.AnnouncementContents
            .Where(c => c.AnnouncementId == announcementId)
            .ToListAsync(cancellationToken);

        dbContext.AnnouncementContents.RemoveRange(contents);
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task CreateAnnouncementHistoryAsync(
        Entities.AnnouncementHistory history,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await this.dbContextFactory.CreateDbContextAsync(cancellationToken);

        await dbContext.AnnouncementHistories.AddAsync(history, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
