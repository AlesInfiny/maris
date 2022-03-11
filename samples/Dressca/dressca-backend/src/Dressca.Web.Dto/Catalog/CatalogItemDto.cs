using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Dto.Catalog;

/// <summary>
///  カタログアイテムの DTO です。
/// </summary>
public class CatalogItemDto
{
    /// <summary>
    ///  カタログアイテム Id を取得または設定します。
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    ///  商品名を取得または設定します。
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///  説明を取得または設定します。
    /// </summary>
    [Required]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    ///  単価を取得または設定します。
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    ///  商品コードを取得または設定します。
    /// </summary>
    [Required]
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    ///  カタログカテゴリ Id を取得または設定します。
    /// </summary>
    public long CatalogCategoryId { get; set; }

    /// <summary>
    ///  カタログブランド Id を取得または設定します。
    /// </summary>
    public long CatalogBrandId { get; set; }
}
