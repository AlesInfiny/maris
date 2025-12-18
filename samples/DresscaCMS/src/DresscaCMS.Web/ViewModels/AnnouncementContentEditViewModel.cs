using System.ComponentModel.DataAnnotations;

namespace DresscaCMS.Web.ViewModels;

/// <summary>
/// お知らせメッセージ編集画面においてお知らせコンテンツ部分の入力値を保持するビューモデルです。
/// </summary>
public class AnnouncementContentEditViewModel
{
    /// <summary>
    /// お知らせコンテンツ ID を取得または設定します。
    /// </summary>
    public Guid Id { get; set; } = Guid.Empty;

    /// <summary>
    /// お知らせメッセージ ID を取得または設定します。
    /// </summary>
    public Guid AnnouncementId { get; set; } = Guid.Empty;

    /// <summary>
    /// 言語コードを取得または設定します。
    /// </summary>
    [Required(ErrorMessage = "言語を選択してください。")]
    public string LanguageCode { get; set; } = "ja";

    /// <summary>
    /// タイトルを取得または設定します。
    /// </summary>
    [Required(ErrorMessage = "タイトルを入力してください。")]
    [StringLength(256, ErrorMessage = "タイトルは256文字以下で入力してください。")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// メッセージを取得または設定します。
    /// </summary>
    [Required(ErrorMessage = "メッセージを入力してください。")]
    [StringLength(512, ErrorMessage = "メッセージは512文字以下で入力してください。")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// リンク先 URL を取得または設定します。
    /// </summary>
    [Url(ErrorMessage = "リンク先URLはURL の形式で入力してください。")]
    [StringLength(1024, ErrorMessage = "リンク先URLは1024文字以下で入力してください。")]
    public string? LinkedUrl { get; set; }

    /// <summary>
    /// 画面上での表示順序を取得または設定します（言語の優先順に基づく）。
    /// </summary>
    public int DisplayOrder { get; set; }
}
