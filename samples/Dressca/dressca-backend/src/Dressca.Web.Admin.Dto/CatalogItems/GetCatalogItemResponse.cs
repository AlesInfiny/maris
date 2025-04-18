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
    public required string Description { get; set; }

    /// <summary>
    ///  単価を取得または設定します。
    /// </summary>
    [Required]
    public required decimal Price { get; set; }

    /// <summary>
    ///  カタログカテゴリ ID を取得または設定します。
    /// </summary>
    [Required]
    public required long CatalogCategoryId { get; set; }

    /// <summary>
    ///  カタログブランド ID を取得または設定します。
    /// </summary>
    [Required]
    public required long CatalogBrandId { get; set; }

    /// <summary>
    ///  カタログアイテム ID を取得または設定します。
    /// </summary>
    [Required]
    public required long Id { get; set; }

    /// <summary>
    ///  商品名を取得または設定します。
    /// </summary>
    [Required]
    public required string Name { get; set; }

    /// <summary>
    ///  商品コードを取得または設定します。
    /// </summary>
    [Required]
    public required string ProductCode { get; set; }

    /// <summary>
    ///  アセットコードの一覧を取得または設定します。
    /// </summary>
    public required IList<string> AssetCodes { get; set; }

    /// <summary>
    ///  行バージョンを取得または設定します。
    /// </summary>
    [Required]
    public required byte[] RowVersion { get; set; }

    /// <summary>
    ///  論理削除フラグを取得または設定します。
    /// </summary>
    [Required]
    public required bool IsDeleted { get; set; }
}
