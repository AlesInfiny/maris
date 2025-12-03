using DresscaCMS.Announcement.ApplicationCore;
using DresscaCMS.Announcement.Resources;

namespace DresscaCMS.Announcement.Infrastructures.Entities;

/// <summary>
///  お知らせ履歴のテーブルエンティティです。
/// </summary>
public class AnnouncementHistory
{
    private Announcement? announcement;

    /// <summary>
    ///  お知らせメッセージ履歴 ID です。
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    ///  お知らせメッセージ ID です。
    /// </summary>
    public required Guid AnnouncementId { get; set; }

    /// <summary>
    ///  更新者のユーザー名を取得または設定します。
    /// </summary>
    public required string ChangedBy { get; set; }

    /// <summary>
    ///  このレコードが作成された日時（＝更新された日時）を取得または設定します。
    /// </summary>
    public required DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    ///  変更の種類（作成、編集、削除）を取得または設定します。
    /// </summary>
    public required OperationType OperationType { get; set; }

    /// <summary>
    ///  お知らせメッセージのカテゴリー（履歴）を取得または設定します。
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// 掲載開始日時（履歴）を取得または設定します。
    /// </summary>
    public required DateTimeOffset PostDateTime { get; set; }

    /// <summary>
    ///  掲載終了日時（履歴）を取得または設定します。
    /// </summary>
    public DateTimeOffset? ExpireDateTime { get; set; }

    /// <summary>
    ///  表示優先度（履歴）を取得または設定します。
    /// </summary>
    public required DisplayPriority DisplayPriority { get; set; }

    /// <summary>
    ///  お知らせコンテンツ履歴へのナビゲーションプロパティです。
    /// </summary>
    public ICollection<AnnouncementContentHistory> Contents { get; set; } = [];

    /// <summary>
    ///  お知らせメッセージへのナビゲーションプロパティです。
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="Announcement"/> が設定されていません。</exception>
    /// <exception cref="ArgumentNullException"><see langword="null"/> を設定できません。</exception>
    public Announcement Announcement
    {
        get => this.announcement ?? throw new InvalidOperationException(string.Format(Messages.PropertyNotInitialized, nameof(this.Announcement)));
        private set => this.announcement = value ?? throw new ArgumentNullException(nameof(value));
    }
}
