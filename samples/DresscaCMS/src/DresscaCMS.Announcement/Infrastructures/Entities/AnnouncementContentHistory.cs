using DresscaCMS.Announcement.Resources;

namespace DresscaCMS.Announcement.Infrastructures.Entities;

/// <summary>
///  お知らせコンテンツ履歴のテーブルエンティティです。
/// </summary>
public class AnnouncementContentHistory
{
    /// <summary>
    ///  お知らせコンテンツ履歴 ID です。
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///  お知らせメッセージ履歴 ID です。
    /// </summary>
    public required Guid AnnouncementHistoryId { get; set; }

    /// <summary>
    ///  言語コード（履歴）を取得または設定します。
    /// </summary>
    public required string LanguageCode { get; set; }

    /// <summary>
    ///  タイトル（履歴）を取得または設定します。
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    ///  メッセージ本文（履歴）を取得または設定します。
    /// </summary>
    public required string Message { get; set; }

    /// <summary>
    ///  リンク先 URL（履歴）を取得または設定します。
    /// </summary>
    public string? LinkedUrl { get; set; }

    /// <summary>
    ///  お知らせメッセー履歴へのナビゲーションプロパティです。
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="AnnouncementHistory"/> が設定されていません。</exception>
    /// <exception cref="ArgumentNullException"><see langword="null"/> を設定できません。</exception>
    public AnnouncementHistory AnnouncementHistory
    {
        get => field ?? throw new InvalidOperationException(string.Format(Messages.PropertyNotInitialized, nameof(this.AnnouncementHistory)));
        private set => field = value ?? throw new ArgumentNullException(nameof(value));
    }
}
