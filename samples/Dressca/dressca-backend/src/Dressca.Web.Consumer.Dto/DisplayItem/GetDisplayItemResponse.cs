using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Consumer.Dto.DisplayItem;

/// <summary>
///  陳列品のレスポンスデータを表します。
/// </summary>
public class GetDisplayItemResponse
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
    ///  陳列品カテゴリ Id を取得または設定します。
    /// </summary>
    [Required]
    public Guid DisplayItemCategoryId { get; set; }

    /// <summary>
    ///  陳列品ブランド Id を取得または設定します。
    /// </summary>
    [Required]
    public Guid DisplayItemBrandId { get; set; }

    /// <summary>
    ///  陳列品 Id を取得または設定します。
    /// </summary>
    [Required]
    public Guid Id { get; set; }

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
}
