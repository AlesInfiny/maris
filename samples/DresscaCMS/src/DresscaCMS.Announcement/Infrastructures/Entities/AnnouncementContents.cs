using System.ComponentModel.DataAnnotations;
using DresscaCMS.Announcement.Resources;

namespace DresscaCMS.Announcement.Infrastructures.Entities;

/// <summary>
///  お知らせコンテンツのテーブルエンティティです。
/// </summary>
public class AnnouncementContents
{
    private Annoucements? announcement;

    /// <summary>
    ///  お知らせコンテンツ ID  を取得または設定します。
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    ///  お知らせ ID を取得または設定します。
    /// </summary>
    public required Guid AnnouncementId { get; set; }

    /// <summary>
    ///  言語コード（ "ja", "en" 等）を取得または設定します。
    /// </summary>
    public required string LanguageCode { get; set; }

    /// <summary>
    ///  タイトルを取得または設定します。
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    ///  メッセージ本文を取得または設定します。
    /// </summary>
    public required string Message { get; set; }

    /// <summary>
    ///  リンク先 URL を取得または設定します。
    /// </summary>
    public string? LinkedUrl { get; set; }

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
