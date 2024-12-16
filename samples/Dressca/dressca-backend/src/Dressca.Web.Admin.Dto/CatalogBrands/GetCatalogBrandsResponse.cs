using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Admin.Dto.CatalogBrands;

/// <summary>
///  カタログブランドのレスポンスデータを表します。
///  カタログアイテムの製造元や企画元に基づいて定義されるブランドを表現します。
/// </summary>
public class GetCatalogBrandsResponse
{
    /// <summary>
    ///  カタログブランド ID を取得または設定します。
    /// </summary>
    [Required]
    public required long Id { get; set; }

    /// <summary>
    ///  ブランド名を取得または設定します。
    /// </summary>
    [Required]
    public required string Name { get; set; }
}
