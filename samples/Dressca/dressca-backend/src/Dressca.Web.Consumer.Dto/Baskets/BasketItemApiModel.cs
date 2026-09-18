using System.ComponentModel.DataAnnotations;
using Dressca.Web.Consumer.Dto.DisplayItem;

namespace Dressca.Web.Consumer.Dto.Baskets;

/// <summary>
///  買い物かごのアイテムのレスポンスデータを表します。
/// </summary>
public class BasketItemApiModel
{
    /// <summary>
    ///  陳列品 Id を取得または設定します。
    /// </summary>
    [Required]
    public Guid DisplayItemId { get; set; }

    /// <summary>
    ///  単価を取得または設定します。
    /// </summary>
    [Required]
    public decimal UnitPrice { get; set; }

    /// <summary>
    ///  数量を取得または設定します。
    /// </summary>
    [Required]
    public int Quantity { get; set; }

    /// <summary>
    ///  小計額を取得します。
    /// </summary>
    [Required]
    public decimal SubTotal { get; set; }

    /// <summary>
    ///  陳列品を取得または設定します。
    /// </summary>
    public DisplayItemSummaryApiModel? DisplayItem { get; set; }
}
