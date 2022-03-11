using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Dto.Catalog;

/// <summary>
///  カタログカテゴリの DTO です。
/// </summary>
public class CatalogCategoryDto
{
    /// <summary>
    ///  カタログカテゴリ Id を取得または設定します。
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    ///  カテゴリ名を取得または設定します。
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;
}
