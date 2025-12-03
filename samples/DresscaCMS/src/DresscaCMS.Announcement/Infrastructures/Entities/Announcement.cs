using DresscaCMS.Announcement.ApplicationCore.Enumerations;

namespace DresscaCMS.Announcement.Infrastructures.Entities;

/// <summary>
///  お知らせメッセージのテーブルエンティティです。
/// </summary>
public class Announcement
{
    /// <summary>
    ///  お知らせメッセージ ID を取得または設定します。
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    ///  お知らせメッセージのカテゴリーを取得または設定します。
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    ///  掲載開始日時を取得または設定します。
    /// </summary>
    public required DateTimeOffset PostDateTime { get; set; }

    /// <summary>
    ///  掲載終了日時を取得または設定します。
    /// </summary>
    public DateTimeOffset? ExpireDateTime { get; set; }

    /// <summary>
    ///  表示優先度を取得または設定します。
    /// </summary>
    public required DisplayPriority DisplayPriority { get; set; }

    /// <summary>
    ///  レコード作成日時を取得または設定します。
    /// </summary>
    public required DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    ///  レコード更新日時を取得または設定します。
    /// </summary>
    public required DateTimeOffset ChangedAt { get; set; }

    /// <summary>
    ///  論理削除フラグを取得または設定します。
    /// </summary>
    public required bool IsDeleted { get; set; }

    /// <summary>
    /// お知らせコンテンツを取得または設定します。
    /// </summary>
    public ICollection<AnnouncementContent> Contents { get; set; } = [];

    /// <summary>
    ///  お知らせメッセージ履歴を取得または設定します。
    /// </summary>
    public ICollection<AnnouncementHistory> Histories { get; set; } = [];
}
