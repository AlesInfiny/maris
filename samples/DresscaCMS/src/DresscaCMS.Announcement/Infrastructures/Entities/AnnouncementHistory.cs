using System.ComponentModel.DataAnnotations;
using DresscaCMS.Announcement.Resources;

namespace DresscaCMS.Announcement.Infrastructures.Entities;

/// <summary>
///  お知らせ履歴のテーブルエンティティです。
/// </summary>
public class AnnouncementHistory
{
    private Annoucements? announcement;

    /// <summary>
    ///  お知らせメッセージ履歴 ID です。
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    ///  お知らせメッセージ ID です。
    /// </summary>
    [Required]
    public Guid AnnouncementId { get; set; }

    /// <summary>
    ///  更新者のユーザー名を取得または設定します。
    /// </summary>
    [Required]
    [MaxLength(256)]
    public required string ChangedBy { get; set; }

    /// <summary>
    ///  このレコードが作成された日時（＝更新された日時）を取得または設定します。
    /// </summary>
    [Required]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    ///  変更の種類（作成、編集、削除）を取得または設定します。
    /// </summary>
    [Required]
    public int OperationType { get; set; }

    /// <summary>
    ///  お知らせメッセージのカテゴリー（履歴）を取得または設定します。
    /// </summary>
    [MaxLength(128)]
    public string? Category { get; set; }

    /// <summary>
    /// 掲載開始日時（履歴）を取得または設定します。
    /// </summary>
    [Required]
    public DateTimeOffset PostDateTime { get; set; }

    /// <summary>
    ///  掲載終了日時（履歴）を取得または設定します。
    /// </summary>
    public DateTimeOffset? ExpireDateTime { get; set; }

    /// <summary>
    ///  表示優先度（履歴）を取得または設定します。
    /// </summary>
    [Required]
    public int DisplayPriority { get; set; }

    /// <summary>
    ///  お知らせコンテンツ履歴へのナビゲーションプロパティです。
    /// </summary>
    public ICollection<AnnouncementContentHistory> Contents { get; set; } = [];

    /// <summary>
    ///  お知らせメッセージへのナビゲーションプロパティです。
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="Announcement"/> が設定されていません。</exception>
    /// <exception cref="ArgumentNullException"><see langword="null"/> を設定できません。</exception>
    public Annoucements Announcement
    {
        get => this.announcement ?? throw new InvalidOperationException(string.Format(Messages.PropertyNotInitialized, nameof(this.Announcement)));
        private set => this.announcement = value ?? throw new ArgumentNullException(nameof(value));
    }
}
