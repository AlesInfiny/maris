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
    ///  お知らせメッセージを登録します。
    /// </summary>
    /// <param name="announcement">登録するお知らせメッセージ。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>登録したお知らせメッセージの ID。</returns>
    Task<Guid> CreateAnnouncementAsync(
        Infrastructures.Entities.Announcement announcement,
        CancellationToken cancellationToken);

    /// <summary>
    ///  お知らせコンテンツを登録します。
    /// </summary>
    /// <param name="content">登録するお知らせコンテンツ。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    Task CreateAnnouncementContentAsync(
        Infrastructures.Entities.AnnouncementContent content,
        CancellationToken cancellationToken);

    /// <summary>
    ///  お知らせメッセージ履歴を登録します。
    /// </summary>
    /// <param name="history">登録するお知らせメッセージ履歴。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    Task CreateAnnouncementHistoryAsync(
        Infrastructures.Entities.AnnouncementHistory history,
        CancellationToken cancellationToken);

    /// <summary>
    ///  お知らせコンテンツ履歴を登録します。
    /// </summary>
    /// <param name="contentHistory">登録するお知らせコンテンツ履歴。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    Task CreateAnnouncementContentHistoryAsync(
        Infrastructures.Entities.AnnouncementContentHistory contentHistory,
        CancellationToken cancellationToken);

    /// <summary>
    ///  お知らせメッセージ ID を指定して、お知らせメッセージ、お知らせコンテンツ、更新履歴を取得します。
    /// </summary>
    /// <param name="announcementId">お知らせメッセージ ID。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>お知らせメッセージ、お知らせコンテンツ、更新履歴を含む結果。お知らせメッセージが存在しない場合は null。</returns>
    Task<GetAnnouncementWithHistoriesResult?> FindByAnnouncementWithContentAsync(
        Guid announcementId,
        CancellationToken cancellationToken);

    /// <summary>
    ///  お知らせメッセージを更新します。
    /// </summary>
    /// <param name="announcement">更新するお知らせメッセージ。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    Task UpdateAnnouncementAsync(
        Infrastructures.Entities.Announcement announcement,
        CancellationToken cancellationToken);

    /// <summary>
    ///  お知らせメッセージ ID を指定して、お知らせコンテンツを取得します。
    /// </summary>
    /// <param name="announcementId">お知らせメッセージ ID。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>お知らせコンテンツのリスト。</returns>
    Task<IReadOnlyCollection<Infrastructures.Entities.AnnouncementContent>> FindAnnouncementContentsByAnnouncementIdAsync(
        Guid announcementId,
        CancellationToken cancellationToken);

    /// <summary>
    ///  お知らせコンテンツを更新します。
    /// </summary>
    /// <param name="content">更新するお知らせコンテンツ。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    Task UpdateAnnouncementContentAsync(
        Infrastructures.Entities.AnnouncementContent content,
        CancellationToken cancellationToken);

    /// <summary>
    ///  お知らせコンテンツを削除します。
    /// </summary>
    /// <param name="contentIds">削除するお知らせコンテンツの ID リスト。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    Task DeleteAnnouncementContentsAsync(
        IEnumerable<Guid> contentIds,
        CancellationToken cancellationToken);
}
