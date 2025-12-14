namespace DresscaCMS.Announcement.ApplicationCore;

/// <summary>
/// お知らせメッセージ編集画面表示インタラクションの実行結果を表します。
/// </summary>
public sealed record GetAnnouncementWithHistoriesResult
{
    /// <summary>
    /// お知らせメッセージです。
    /// </summary>
    public required Infrastructures.Entities.Announcement Announcement { get; init; }

    /// <summary>
    /// お知らせコンテンツのリストです。
    /// </summary>
    public required IReadOnlyCollection<Infrastructures.Entities.AnnouncementContent> Contents { get; init; }

    /// <summary>
    /// お知らせメッセージ更新履歴のリストです。
    /// </summary>
    public required IReadOnlyCollection<Infrastructures.Entities.AnnouncementHistory> Histories { get; init; }
}
