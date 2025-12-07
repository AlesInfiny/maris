namespace DresscaCMS.Announcement.ApplicationCore;

/// <summary>
/// 　お知らせメッセージ管理画面表示インタラクションの実行結果を表します。
/// </summary>
public sealed record GetPagedAnnouncementsResult
{
    /// <summary>
    /// ページ番号です。
    /// </summary>
    public int PageNumber { get; init; }

    /// <summary>
    /// 1ページあたりの表示件数です。
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// お知らせメッセージの総件数です。
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// お知らせメッセージのリストです。
    /// </summary>
    public IReadOnlyCollection<Infrastructures.Entities.Announcement> Announcements { get; init; } = [];

    /// <summary>
    /// 最後のページ番号です。
    /// </summary>
    public int LastPageNumber { get; init; }

    /// <summary>
    /// 表示するお知らせメッセージの開始件数です。
    /// </summary>
    public int DisplayFrom { get; init; }

    /// <summary>
    /// 表示するお知らせメッセージの終了件数です。
    /// </summary>
    public int DisplayTo { get; init; }
}
