using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Dto.Catalog;

/// <summary>
///  カタログアイテムのレスポンスデータを表します。
/// </summary>
public class CatalogItemResponse : CatalogItemSummaryResponse
{
    /// <summary>
    ///  説明を取得または設定します。
    /// </summary>
    [Required]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    ///  単価を取得または設定します。
    /// </summary>
    [Required]
    public decimal Price { get; set; }

    /// <summary>
    ///  カタログカテゴリ Id を取得または設定します。
    /// </summary>
    [Required]
    public long CatalogCategoryId { get; set; }

    /// <summary>
    ///  カタログブランド Id を取得または設定します。
    /// </summary>
    [Required]
    public long CatalogBrandId { get; set; }
}
