namespace Maris.Samples.ApplicationCore;

/// <summary>
///  商品情報を表すエンティティです。
/// </summary>
public class Product
{
    /// <summary>
    ///  ID を取得または設定します。
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    ///  商品名を取得または設定します。
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///  単価を取得または設定します。
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    ///  商品カテゴリの ID を取得または設定します。
    /// </summary>
    public long ProductCategoryId { get; set; }

    /// <summary>
    ///  商品カテゴリの情報を取得または設定します。
    /// </summary>
    public ProductCategory? ProductCategory { get; set; }
}
