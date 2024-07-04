using System.ComponentModel.DataAnnotations;
using Dressca.Web.Dto.Accounting;

namespace Dressca.Web.Dto.Ordering;

/// <summary>
///  注文情報のレスポンスデータを表します。
/// </summary>
public class OrderResponse
{
    /// <summary>
    ///  注文 Id を取得または設定します。
    /// </summary>
    [Required]
    public long Id { get; set; }

    /// <summary>
    ///  購入者 Id を取得または設定します。
    /// </summary>
    [Required]
    public string BuyerId { get; set; } = string.Empty;

    /// <summary>
    ///  注文日を取得または設定します。
    /// </summary>
    [Required]
    public DateTimeOffset OrderDate { get; set; }

    /// <summary>
    ///  お届け先氏名を取得または設定します。
    /// </summary>
    [Required]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    ///  お届け先郵便番号を取得または設定します。
    /// </summary>
    [Required]
    public string PostalCode { get; set; } = string.Empty;

    /// <summary>
    ///  お届け先都道府県を取得または設定します。
    /// </summary>
    [Required]
    public string Todofuken { get; set; } = string.Empty;

    /// <summary>
    ///  お届け先市区町村を取得または設定します。
    /// </summary>
    [Required]
    public string Shikuchoson { get; set; } = string.Empty;

    /// <summary>
    ///  お届け先字／番地／建物名／部屋番号を取得または設定します。
    /// </summary>
    [Required]
    public string AzanaAndOthers { get; set; } = string.Empty;

    /// <summary>
    ///  会計情報を取得または設定します。
    /// </summary>
    public AccountResponse? Account { get; set; }

    /// <summary>
    ///  注文アイテムのリストを取得または設定します。
    /// </summary>
    public IList<OrderItemResponse> OrderItems { get; set; } = [];
}
