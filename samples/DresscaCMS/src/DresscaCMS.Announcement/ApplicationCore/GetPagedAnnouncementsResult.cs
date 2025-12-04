namespace DresscaCMS.Announcement.ApplicationCore;

/// <summary>
/// ページングされたお知らせ情報の取得結果を表します。
/// </summary>
public sealed record GetPagedAnnouncementsResult
{
    /// <summary>
    /// 現在のページ番号。
    /// </summary>
    public int PageNumber { get; init; }

    /// <summary>
    /// 1ページあたりの件数。
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// 総件数。
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// 画面表示用 DTO のリスト。
    /// </summary>
    public IReadOnlyCollection<Infrastructures.Entities.Announcement> Announcements { get; init; } = [];

    /// <summary>
    /// 最終ページ番号。
    /// </summary>
    public int LastPageNumber { get; init; }

    /// <summary>
    /// 表示範囲の開始インデックス。
    /// </summary>
    public int DisplayFrom { get; init; }

    /// <summary>
    /// 表示範囲の終了インデックス。
    /// </summary>
    public int DisplayTo { get; init; }
}
