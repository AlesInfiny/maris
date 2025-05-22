using Microsoft.Extensions.Logging;

namespace Maris.Samples.ApplicationCore;

/// <summary>
///  商品情報を取り扱うアプリケーションサービスのサンプルです。
/// </summary>
public class ProductApplicationService
{
    private readonly IProductsRepository productRepository;
    private readonly ILogger<ProductApplicationService> logger;

    /// <summary>
    ///  <see cref="ProductApplicationService"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="productsRepository">リポジトリ。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="productsRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public ProductApplicationService(IProductsRepository productsRepository, ILogger<ProductApplicationService> logger)
    {
        this.productRepository = productsRepository ?? throw new ArgumentNullException(nameof(productsRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///  指定した商品カテゴリの商品一覧を取得します。
    /// </summary>
    /// <param name="category">商品カテゴリ。</param>
    /// <returns>商品一覧。</returns>
    public List<Product> GetProductsByCategory(ProductCategory category)
    {
        this.logger.LogInformation(Events.GetProductsByCategoryStart, $"{category.Id} の商品情報を取得します。");
        var products = this.productRepository.GetProductsByCategory(category);
        this.logger.LogInformation(Events.GetProductsByCategoryNormalEnd, $"{category.CategoryName} の商品情報を {products.Count} 件取得しました。");
        return products;
    }

    /// <summary>
    ///  指定した単価内の商品一覧を取得します。
    /// </summary>
    /// <param name="minPrice">
    ///  最小値。
    ///  <see langword="null"/> を指定した場合は <see cref="decimal.MinValue"/> として扱います。
    /// </param>
    /// <param name="maxPrice">
    ///  最大値。
    ///  <see langword="null"/> を指定した場合は <see cref="decimal.MaxValue"/> として扱います。
    /// </param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>商品一覧。</returns>
    public async Task<List<Product>> GetProductsByUnitPriceRangeAsync(
        decimal? minPrice, decimal? maxPrice, CancellationToken cancellationToken)
    {
        var minimumPrice = minPrice ?? decimal.MinValue;
        var maximumPrice = maxPrice ?? decimal.MaxValue;
        this.logger.LogInformation(Events.GetProductsByUnitPriceRangeStart, $"{minimumPrice} ～ {maximumPrice} の単価の商品情報を取得します。");
        var products = await this.productRepository.GetProductsByUnitPriceRangeAsync(minimumPrice, maximumPrice, cancellationToken);
        this.logger.LogInformation(Events.GetProductsByUnitPriceRangeNormalEnd, $"{minimumPrice} ～ {maximumPrice} の単価の商品情報を {products.Count} 件取得しました。");
        return products;
    }
}
