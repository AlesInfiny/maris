using System.Linq.Expressions;
using Dressca.ApplicationCore.Catalog;
using Dressca.ApplicationCore.Resources;
using Microsoft.Extensions.Logging;

namespace Dressca.ApplicationCore.ApplicationService;

/// <summary>
///  カタログに関するビジネスユースケースを実現するアプリケーションサービスです。
/// </summary>
public class CatalogApplicationService
{
    private readonly ICatalogRepository catalogRepository;
    private readonly ICatalogBrandRepository brandRepository;
    private readonly ICatalogCategoryRepository categoryRepository;
    private readonly ILogger<CatalogApplicationService> logger;

    /// <summary>
    ///  <see cref="CatalogApplicationService"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="catalogRepository">カタログリポジトリ。</param>
    /// <param name="brandRepository">ブランドリポジトリ。</param>
    /// <param name="categoryRepository">カテゴリリポジトリ。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="catalogRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="brandRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="categoryRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public CatalogApplicationService(
        ICatalogRepository catalogRepository,
        ICatalogBrandRepository brandRepository,
        ICatalogCategoryRepository categoryRepository,
        ILogger<CatalogApplicationService> logger)
    {
        this.catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
        this.brandRepository = brandRepository ?? throw new ArgumentNullException(nameof(brandRepository));
        this.categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///  カタログ情報を取得します。
    /// </summary>
    /// <param name="skip">読み飛ばす項目数。</param>
    /// <param name="take">最大取得項目数。</param>
    /// <param name="brandId">カタログブランド Id 。</param>
    /// <param name="categoryId">カタログカテゴリ Id 。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>カタログページと総アイテム数のタプルを返す非同期処理を表すタスク。</returns>
    public async Task<(IReadOnlyList<CatalogItem> ItemsOnPage, int TotalItems)> GetCatalogItemsAsync(int skip, int take, long? brandId, long? categoryId, CancellationToken cancellationToken = default)
    {
        this.logger.LogDebug(Events.DebugEvent, LogMessages.CatalogApplicationService_GetCatalogItemsAsyncStart, brandId, categoryId);

        IReadOnlyList<CatalogItem> itemsOnPage;
        int totalItems;
        using (var scope = TransactionScopeManager.CreateTransactionScope())
        {
            Expression<Func<CatalogItem, bool>> specification = item =>
                (!brandId.HasValue || item.CatalogBrandId == brandId) &&
                (!categoryId.HasValue || item.CatalogCategoryId == categoryId);
            itemsOnPage = await this.catalogRepository.FindAsync(specification, skip, take, cancellationToken);
            totalItems = await this.catalogRepository.CountAsync(specification, cancellationToken);
            scope.Complete();
        }

        this.logger.LogDebug(Events.DebugEvent, LogMessages.CatalogApplicationService_GetCatalogItemsAsyncEnd, brandId, categoryId);
        return (ItemsOnPage: itemsOnPage, TotalItems: totalItems);
    }

    /// <summary>
    /// フィルタリング用のカタログブランドリストを取得します。
    /// </summary>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>カタログブランドリストを返す非同期処理を表すタスク。</returns>
    public Task<IReadOnlyList<CatalogBrand>> GetBrandsAsync(CancellationToken cancellationToken = default)
        => this.brandRepository.GetAllAsync(cancellationToken);

    /// <summary>
    /// フィルタリング用のカタログカテゴリリストを取得します。
    /// </summary>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>カタログカテゴリリストを返す非同期処理を表すタスク。</returns>
    public Task<IReadOnlyList<CatalogCategory>> GetCategoriesAsync(CancellationToken cancellationToken = default)
        => this.categoryRepository.GetAllAsync(cancellationToken);
}
