using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Dto.Catalog;

/// <summary>
///  カタログアイテムの概要のレスポンスデータを表します。
/// </summary>
public class CatalogItemSummaryResponse
{
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
    public IList<string> AssetCodes { get; set; } = new List<string>();
}
