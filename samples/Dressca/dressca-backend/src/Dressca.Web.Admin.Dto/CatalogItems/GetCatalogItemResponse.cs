using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Admin.Dto.CatalogItems;

/// <summary>
///  カタログアイテムのレスポンスデータを表します。
/// </summary>
public class GetCatalogItemResponse
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

    /// <summary>
    ///  カタログアイテム Id を取得または設定します。
    /// </summary>
    [Required]
    public long Id { get; set; }

    /// <summary>
    ///  商品名を取得または設定します。
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///  商品コードを取得または設定します。
    /// </summary>
    [Required]
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    ///  アセットコードの一覧を取得または設定します。
    /// </summary>
    public IList<string> AssetCodes { get; set; } = [];

    /// <summary>
    /// 行バージョンを取得または設定します。
    /// </summary>
    [Required]
    public byte[] RowVersion { get; set; } = [];
}
