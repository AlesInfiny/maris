namespace DresscaCMS.Announcement.ApplicationCore.RepositoryInterfaces;

/// <summary>
///  お知らせメッセージを取得するためのリポジトリインターフェイスです。
/// </summary>
public interface IAnnouncementsRepository
{
    /// <summary>
    ///  範囲指定してお知らせメッセージを取得するためのリポジトリインターフェースです。
    /// </summary>
    /// <param name="pageNumber">ページ番号</param>
    /// <param name="pageSize">1 ページあたりの表示件数</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>
    ///  指定したページ番号とページサイズに基づいて取得されたお知らせメッセージのコレクションを非同期で返します。
    /// </returns>
    Task<IReadOnlyCollection<Infrastructures.Entities.Announcement>> FindByPageNumberAndPageSizeAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    ///  お知らせメッセージの総件数を取得します。（論理削除されていないものに限る）
    /// </summary>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>論理削除されていないお知らせメッセージの総件数。</returns>
    Task<int> CountNotDeletedAsync(CancellationToken cancellationToken);
}
