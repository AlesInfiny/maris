using System.ComponentModel.DataAnnotations;
using Dressca.Web.Consumer.Dto.Catalog;

namespace Dressca.Web.Consumer.Dto.Baskets;

/// <summary>
///  買い物かごのアイテムのレスポンスデータを表します。
/// </summary>
public class BasketItemResponse
{
    /// <summary>
    ///  カタログアイテム Id を取得または設定します。
    /// </summary>
    [Required]
    public long CatalogItemId { get; set; }

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
    ///  カタログアイテムを取得または設定します。
    /// </summary>
    public CatalogItemSummaryResponse? CatalogItem { get; set; }
}
