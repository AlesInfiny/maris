namespace DresscaCMS.Announcement.ApplicationCore;

/// <summary>
///  お知らせメッセージと履歴の取得結果を表します。
/// </summary>
public sealed record GetAnnouncementAndHistoriesResult
{
    /// <summary>
    ///  お知らせメッセージです。
    /// </summary>
    public Infrastructures.Entities.Announcement? Announcement { get; init; }
}
