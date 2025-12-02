using System.ComponentModel.DataAnnotations;

namespace DresscaCMS.Announcement.Infrastructures.Entities;

/// <summary>
///  お知らせメッセージのテーブルエンティティです。
/// </summary>
public class Annoucements
{
    /// <summary>
    ///  お知らせメッセージ ID を取得または設定します。
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    ///  お知らせメッセージのカテゴリーを取得または設定します。
    /// </summary>
    [MaxLength(128)]
    public string? Category { get; set; }

    /// <summary>
    ///  掲載開始日時を取得または設定します。
    /// </summary>
    [Required]
    public DateTimeOffset PostDateTime { get; set; }

    /// <summary>
    ///  掲載終了日時を取得または設定します。
    /// </summary>
    public DateTimeOffset? ExpireDateTime { get; set; }

    /// <summary>
    ///  表示優先度を取得または設定します。
    /// </summary>
    [Required]
    public int DisplayPriority { get; set; }

    /// <summary>
    ///  レコード作成日時を取得または設定します。
    /// </summary>
    [Required]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    ///  レコード更新日時を取得または設定します。
    /// </summary>
    [Required]
    public DateTimeOffset ChangedAt { get; set; }

    /// <summary>
    ///  論理削除フラグを取得または設定します。
    /// </summary>
    [Required]
    public bool IsDeleted { get; set; }

    /// <summary>
    /// お知らせコンテンツを取得または設定します。
    /// </summary>
    public ICollection<AnnouncementContents> Contents { get; set; } = [];

    /// <summary>
    ///  お知らせメッセージ履歴を取得または設定します。
    /// </summary>
    public ICollection<AnnouncementHistory> Histories { get; set; } = [];
}
