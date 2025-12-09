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
    Task<IReadOnlyCollection<Infrastructures.Entities.Announcement>> FindByPageNumberAndPageSizeAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

    /// <summary>
    ///  お知らせメッセージの総件数を取得します。（論理削除されていないものに限る）
    /// </summary>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>論理削除されていないお知らせメッセージの総件数。</returns>
    Task<int> CountNotDeletedAsync(CancellationToken cancellationToken);

    /// <summary>
    ///  指定された ID のお知らせメッセージを、コンテンツおよび履歴と共に取得します。
    /// </summary>
    /// <param name="announcementId">お知らせメッセージ ID。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>
    ///  指定された ID に対応するお知らせメッセージ。存在しない場合は <see langword="null"/> 。
    /// </returns>
    Task<Infrastructures.Entities.Announcement?> FindByIdWithContentsAndHistoriesAsync(
        Guid announcementId,
        CancellationToken cancellationToken);

    /// <summary>
    ///  お知らせメッセージを論理削除します。
    /// </summary>
    /// <param name="announcement">論理削除するお知らせメッセージ。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>更新件数。</returns>
    Task<int> UpdateAnnouncementAsync(
        Infrastructures.Entities.Announcement announcement,
        CancellationToken cancellationToken);

    /// <summary>
    ///  指定されたお知らせメッセージに紐づくお知らせコンテンツを削除します。
    /// </summary>
    /// <param name="announcementId">お知らせメッセージ ID。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>削除件数。</returns>
    Task<int> DeleteAnnouncementContentsAsync(
        Guid announcementId,
        CancellationToken cancellationToken);

    /// <summary>
    ///  お知らせメッセージ履歴を作成します。
    /// </summary>
    /// <param name="history">作成するお知らせメッセージ履歴。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>タスク。</returns>
    Task CreateAnnouncementHistoryAsync(
        Infrastructures.Entities.AnnouncementHistory history,
        CancellationToken cancellationToken);
}
