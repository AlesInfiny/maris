using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Admin.Dto.CatalogBrands;

/// <summary>
///  カタログブランドのレスポンスデータを表します。
///  カタログアイテムの製造元や企画元に基づいて定義されるブランドを表現します。
/// </summary>
public class GetCatalogBrandsResponse
{
    /// <summary>
    ///  カタログブランド Id を取得または設定します。
    /// </summary>
    [Required]
    public long Id { get; set; }

    /// <summary>
    ///  ブランド名を取得または設定します。
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;
}
