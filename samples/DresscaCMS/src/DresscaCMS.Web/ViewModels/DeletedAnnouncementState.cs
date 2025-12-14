namespace DresscaCMS.Web.ViewModels;

/// <summary>
/// 削除されたお知らせメッセージの状態を表すクラス
/// </summary>
public class DeletedAnnouncementState
{
    /// <summary>
    /// お知らせメッセージを取得または設定します。
    /// </summary>
    public required DresscaCMS.Announcement.Infrastructures.Entities.Announcement Announcement { get; set; }

    /// <summary>
    /// お知らせコンテンツのリストを取得または設定します。
    /// </summary>
    public required List<DresscaCMS.Announcement.Infrastructures.Entities.AnnouncementContent> Contents { get; set; }

    /// <summary>
    /// お知らせメッセージ履歴のリストを取得または設定します。
    /// </summary>
    public required List<DresscaCMS.Announcement.Infrastructures.Entities.AnnouncementHistory> Histories { get; set; }
}
