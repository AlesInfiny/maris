using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DresscaCMS.Announcement.Infrastructures.Entities;

/// <summary>
///  お知らせコンテンツのテーブルエンティティです。
/// </summary>
public class AnnouncementContent
{
    /// <summary>
    ///  お知らせコンテンツ ID  を取得または設定します。
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    ///  お知らせ ID を取得または設定します。
    /// </summary>
    [Required]
    public Guid AnnouncementId { get; set; }

    /// <summary>
    ///  言語コード（ "ja", "en" 等）を取得または設定します。
    /// </summary>
    [Required]
    [MaxLength(8)]
    public string LanguageCode { get; set; } = default!;

    /// <summary>
    ///  タイトルを取得または設定します。
    /// </summary>
    [Required]
    [MaxLength(256)]
    public string Title { get; set; } = default!;

    /// <summary>
    ///  メッセージ本文を取得または設定します。
    /// </summary>
    [Required]
    [MaxLength(512)]
    public string Message { get; set; } = default!;

    /// <summary>
    ///  リンク先 URL を取得または設定します。
    /// </summary>
    [MaxLength(1024)]
    public string? LinkedUrl { get; set; }

    /// <summary>
    ///  楽観同時実行制御カラムを取得または設定します。
    /// </summary>
    public byte[] RowVersion { get; set; } = [];

    /// <summary>
    ///  お知らせメッセージへのナビゲーションプロパティです。
    /// </summary>
    public Announcement Announcement { get; set; } = default!;
}
