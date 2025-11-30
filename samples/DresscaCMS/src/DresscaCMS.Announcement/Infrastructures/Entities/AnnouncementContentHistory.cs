using System.ComponentModel.DataAnnotations;

namespace DresscaCMS.Announcement.Infrastructures.Entities;

/// <summary>
///  お知らせコンテンツ履歴のテーブルエンティティです。
/// </summary>
public class AnnouncementContentHistory
{
    /// <summary>
    ///  お知らせコンテンツ履歴 ID です。
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    ///  お知らせメッセージ履歴 ID です。
    /// </summary>
    [Required]
    public Guid AnnouncementHistoryId { get; set; }

    /// <summary>
    ///  言語コード（履歴）を取得または設定します。
    /// </summary>
    [Required]
    [MaxLength(8)]
    public string LanguageCode { get; set; } = default!;

    /// <summary>
    ///  タイトル（履歴）を取得または設定します。
    /// </summary>
    [Required]
    [MaxLength(256)]
    public string Title { get; set; } = default!;

    /// <summary>
    ///  メッセージ本文（履歴）を取得または設定します。
    /// </summary>
    [Required]
    [MaxLength(512)]
    public string Message { get; set; } = default!;

    /// <summary>
    ///  リンク先 URL（履歴）を取得または設定します。
    /// </summary>
    [MaxLength(1024)]
    public string? LinkedUrl { get; set; }

    /// <summary>
    ///  楽観同時実行制御カラムです。
    /// </summary>
    public byte[] RowVersion { get; set; } = [];

    /// <summary>
    ///  お知らせメッセージ履歴へのナビゲーションプロパティです。
    /// </summary>
    public AnnouncementHistory AnnouncementHistory { get; set; } = default!;
}
