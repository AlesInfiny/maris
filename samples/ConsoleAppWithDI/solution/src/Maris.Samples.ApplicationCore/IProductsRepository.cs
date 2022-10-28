namespace Maris.Samples.ApplicationCore;

/// <summary>
///  商品情報にアクセスするリポジトリです。
/// </summary>
public interface IProductsRepository
{
    /// <summary>
    ///  指定した商品カテゴリの商品一覧を取得します。
    /// </summary>
    /// <param name="productCategory">商品カテゴリ。</param>
    /// <returns>商品一覧。</returns>
    List<Product> GetProductsByCategory(ProductCategory productCategory);

    /// <summary>
    ///  指定した単価内の商品一覧を取得します。
    /// </summary>
    /// <param name="minPrice">最小値。</param>
    /// <param name="maxPrice">最大値。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>商品一覧。</returns>
    Task<List<Product>> GetProductsByUnitPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken);
}
