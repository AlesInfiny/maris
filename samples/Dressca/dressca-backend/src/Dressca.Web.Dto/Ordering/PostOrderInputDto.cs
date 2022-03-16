using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Dto.Ordering;

/// <summary>
///  注文を行う処理の入力情報を表す DTO です。
/// </summary>
public class PostOrderInputDto
{
    /// <summary>
    ///  注文者の氏名を取得または設定します。
    /// </summary>
    [Required]
    [StringLength(64)]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    ///  郵便番号を取得または設定します。
    /// </summary>
    [Required]
    [StringLength(16)]
    public string PostalCode { get; set; } = string.Empty;

    /// <summary>
    ///  都道府県を取得または設定します。
    /// </summary>
    [Required]
    [StringLength(16)]
    public string Todofuken { get; set; } = string.Empty;

    /// <summary>
    ///  市区町村を取得または設定します。
    /// </summary>
    [Required]
    [StringLength(32)]
    public string Shikuchoson { get; set; } = string.Empty;

    /// <summary>
    ///  字／番地／建物名／部屋番号を取得または設定します。
    /// </summary>
    [Required]
    [StringLength(128)]
    public string AzanaAndOthers { get; set; } = string.Empty;
}
