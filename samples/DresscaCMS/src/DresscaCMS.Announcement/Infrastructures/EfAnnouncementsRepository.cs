using DresscaCMS.Announcement.ApplicationCore.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DresscaCMS.Announcement.Infrastructures;

/// <summary>
///  お知らせメッセージの取得を行う EF Core リポジトリ実装です。
/// </summary>
public class EfAnnouncementsRepository : IAnnouncementsRepository
{
    private readonly AnnouncementDbContext dbContext;

    /// <summary>
    ///  データアクセスに使用する <see cref="AnnouncementDbContext"/> を指定して
    ///  <see cref="EfAnnouncementsRepository"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="dbContext">データアクセスに使用する <see cref="AnnouncementDbContext"/> オブジェクト。</param>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="dbContext"/> が <see langword="null" />です。
    /// </exception>
    public EfAnnouncementsRepository(AnnouncementDbContext dbContext)
        => this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

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
      CancellationToken cancellationToken = default)
    {
        var query = this.dbContext.Announcements
            .Where(a => !a.IsDeleted)
            .Where(a => a.Contents.Any())
            .Include(a => a.Contents)
            .OrderBy(a => a.PostDateTime);

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
    public async Task<int> CountNotDeletedAsync(CancellationToken cancellationToken = default)
    {
        return await this.dbContext.Announcements
            .Where(x => !x.IsDeleted)
            .CountAsync(cancellationToken);
    }
}
