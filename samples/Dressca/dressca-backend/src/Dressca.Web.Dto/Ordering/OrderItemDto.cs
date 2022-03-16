using Dressca.Web.Dto.Catalog;

namespace Dressca.Web.Dto.Ordering;

/// <summary>
///  注文アイテムの DTO です。
/// </summary>
public class OrderItemDto
{
    /// <summary>
    ///  注文アイテム Id を取得します。
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    ///  注文された商品（カタログアイテム）を取得または設定します。
    /// </summary>
    public CatalogItemSummaryDto? ItemOrdered { get; set; }

    /// <summary>
    ///  単価を取得または設定します。
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    ///  数量を取得または設定します。
    /// </summary>
    public int Quantity { get; set; }
}
