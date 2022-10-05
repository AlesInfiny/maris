namespace Maris.Samples.ApplicationCore;

/// <summary>
///  商品カテゴリを表すエンティティです。
/// </summary>
public class ProductCategory
{
    /// <summary>
    ///  商品カテゴリの ID を取得または設定します。
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    ///  商品カテゴリの名前を取得または設定します。
    /// </summary>
    public string CategoryName { get; set; } = string.Empty;
}
