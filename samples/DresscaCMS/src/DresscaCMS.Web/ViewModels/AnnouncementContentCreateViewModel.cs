using System.ComponentModel.DataAnnotations;
using DresscaCMS.Web.Resources;

namespace DresscaCMS.Web.ViewModels;

/// <summary>
/// お知らせメッセージ登録画面においてお知らせコンテンツ部分の入力値を保持するビューモデルです。
/// </summary>
public class AnnouncementContentCreateViewModel
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
    [Display(Name = "言語")]
    [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.RequiredToSelect))]
    public string LanguageCode { get; set; } = "ja";

    /// <summary>
    /// タイトルを取得または設定します。
    /// </summary>
    [Display(Name = "タイトル")]
    [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.RequiredToInput))]
    [StringLength(256, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.StringLengthIsOver))]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// メッセージを取得または設定します。
    /// </summary>
    [Display(Name = "メッセージ")]
    [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.RequiredToInput))]
    [StringLength(512, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.StringLengthIsOver))]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// リンク先 URL を取得または設定します。
    /// </summary>
    [Display(Name = "リンク先URL")]
    [Url(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.EnterUrlFormat))]
    [StringLength(1024, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.StringLengthIsOver))]
    public string? LinkedUrl { get; set; }

    /// <summary>
    /// 画面上での表示順序を取得または設定します（言語の優先順に基づく）。
    /// </summary>
    public int DisplayOrder { get; set; }
}
