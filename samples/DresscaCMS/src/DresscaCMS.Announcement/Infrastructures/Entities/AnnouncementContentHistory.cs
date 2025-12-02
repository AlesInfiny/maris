using System.ComponentModel.DataAnnotations;
using DresscaCMS.Announcement.Resources;

namespace DresscaCMS.Announcement.Infrastructures.Entities;

/// <summary>
///  お知らせコンテンツ履歴のテーブルエンティティです。
/// </summary>
public class AnnouncementContentHistory
{
    private AnnouncementHistory? announcementHistory;

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
    public string LanguageCode { get; set; } = string.Empty;

    /// <summary>
    ///  タイトル（履歴）を取得または設定します。
    /// </summary>
    [Required]
    [MaxLength(256)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    ///  メッセージ本文（履歴）を取得または設定します。
    /// </summary>
    [Required]
    [MaxLength(512)]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    ///  リンク先 URL（履歴）を取得または設定します。
    /// </summary>
    [MaxLength(1024)]
    public string? LinkedUrl { get; set; }

    /// <summary>
    ///  お知らせメッセー履歴へのナビゲーションプロパティです。
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="AnnouncementHistory"/> が設定されていません。</exception>
    /// <exception cref="ArgumentNullException"><see langword="null"/> を設定できません。</exception>
    public AnnouncementHistory AnnouncementHistory {
        get => this.announcementHistory ?? throw new InvalidOperationException(string.Format(Messages.PropertyNotInitialized, nameof(this.AnnouncementHistory)));
        private set => this.announcementHistory = value ?? throw new ArgumentNullException(nameof(value));
    }
}
