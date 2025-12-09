using System.Linq;
using DresscaCMS.Announcement.ApplicationCore.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DresscaCMS.Announcement.Infrastructures;

/// <summary>
///  お知らせメッセージの取得を行う EF Core リポジトリ実装です。
/// </summary>
public class EfAnnouncementsRepository : IAnnouncementsRepository
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
}
