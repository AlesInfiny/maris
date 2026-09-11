using System.ComponentModel.DataAnnotations;
using Dressca.Web.Consumer.Dto.DisplayItem;

namespace Dressca.Web.Consumer.Dto.Ordering;

/// <summary>
///  注文アイテムのレスポンスデータを表します。
/// </summary>
public class OrderItemApiModel
{
    /// <summary>
    ///  注文アイテム Id を取得します。
    /// </summary>
    [Required]
    public Guid Id { get; init; }

    /// <summary>
    ///  注文された商品（陳列品）を取得または設定します。
    /// </summary>
    public DisplayItemSummaryApiModel? ItemOrdered { get; set; }

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
}
