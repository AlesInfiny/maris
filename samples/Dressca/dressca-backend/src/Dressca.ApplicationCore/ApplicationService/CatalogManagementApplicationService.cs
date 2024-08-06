using Dressca.ApplicationCore.Catalog;
using Dressca.ApplicationCore.Resources;
using Microsoft.Extensions.Logging;

namespace Dressca.ApplicationCore.ApplicationService;

/// <summary>
/// カタログ管理に関するビジネスユースケースを実現するアプリケーションサービスです。
/// </summary>
public class CatalogManagementApplicationService
{
    private readonly ICatalogRepository catalogRepository;
    private readonly ICatalogBrandRepository brandRepository;
    private readonly ICatalogCategoryRepository categoryRepository;
    private readonly ILogger<CatalogManagementApplicationService> logger;

    /// <summary>
    ///  <see cref="CatalogManagementApplicationService"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="catalogRepository">カタログレポジトリ。</param>
    /// <param name="brandRepository">カタログブランドレポジトリ。</param>
    /// <param name="categoryRepository">カタログカテゴリレポジトリ。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    /// <list type="bullet">
    ///   <item><paramref name="catalogRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="brandRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="categoryRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    /// </list>
    /// </exception>
    public CatalogManagementApplicationService(
    ICatalogRepository catalogRepository,
    ICatalogBrandRepository brandRepository,
    ICatalogCategoryRepository categoryRepository,
    ILogger<CatalogManagementApplicationService> logger)
    {
        this.catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
        this.brandRepository = brandRepository ?? throw new ArgumentNullException(nameof(brandRepository));
        this.categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// カタログにアイテムを追加します。
    /// </summary>
    /// <param name="name">商品名。</param>
    /// <param name="description">説明。</param>
    /// <param name="price">単価。</param>
    /// <param name="productCode">商品コード。</param>
    /// <param name="catalogBrandId">カタログブランドID。</param>
    /// <param name="catalogCategoryId">カタログカテゴリID。</param>
    /// <returns>追加したカタログアイテムの情報を返す非同期処理を表すタスク。</returns>
    public async Task<CatalogItem> AddItemToCatalogAsync(
        string name,
        string description,
        decimal price,
        string productCode,
        long catalogBrandId,
        long catalogCategoryId)
    {
        using (var scope = TransactionScopeManager.CreateTransactionScope())
        {
            var catalogItem = new CatalogItem()
            {
                Name = name,
                Description = description,
                Price = price,
                ProductCode = productCode,
                CatalogBrandId = catalogBrandId,

                CatalogCategoryId = catalogCategoryId,
            };
            var catalogItemAdded = await this.catalogRepository.AddAsync(catalogItem);
            scope.Complete();
            return catalogItemAdded;
        }
    }

    /// <summary>
    /// カタログからアイテムを削除します。
    /// </summary>
    /// <param name="id">削除対象のカタログアイテムのID。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>処理結果を返す非同期処理を表すタスク。</returns>
    public async Task DeleteItemFromCatalogAsync(long id, CancellationToken cancellationToken = default)
    {
        using (var scope = TransactionScopeManager.CreateTransactionScope())
        {
            var catalogItem = await this.catalogRepository.GetAsync(id, cancellationToken);

            if (catalogItem == null)
            {
                this.logger.LogInformation(Events.CatalogItemIdDoesNotExistInRepository, LogMessages.CatalogItemIdDoesNotExistInRepository, [id]);
                throw new CatalogItemNotExistingInRepositoryException([id]);
            }

            await this.catalogRepository.RemoveAsync(catalogItem, cancellationToken);
            scope.Complete();
        }
    }

    /// <summary>
    /// カタログアイテムを更新します。
    /// </summary>
    /// <param name="command">更新処理のファサードとなるコマンドオブジェクト。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>処理結果を返す非同期処理を表すタスク。</returns>
    /// <exception cref="CatalogItemNotExistingInRepositoryException">更新対象のカタログアイテムが存在しなかった場合。</exception>
    public async Task UpdateCatalogItemAsync(
        CatalogItemUpdateCommand command,
        CancellationToken cancellationToken = default)
    {
        using (var scope = TransactionScopeManager.CreateTransactionScope())
        {
            var catalogItem = await this.catalogRepository.GetAsync(command.Id, cancellationToken);

            if (catalogItem == null)
            {
                this.logger.LogInformation(Events.CatalogItemIdDoesNotExistInRepository, LogMessages.CatalogItemIdDoesNotExistInRepository, [command.Id]);
                throw new CatalogItemNotExistingInRepositoryException([command.Id]);
            }

            catalogItem.SetName(command.Name);
            catalogItem.SetDescription(command.Description);
            catalogItem.SetPrice(command.Price);
            catalogItem.SetProductCode(command.ProductCode);

            var catalogBrand = await this.brandRepository.GetAsync(command.CatalogBrandId, cancellationToken);
            if (catalogBrand == null)
            {
                this.logger.LogInformation(Events.CatalogBrandIdDoesNotExistInRepository, LogMessages.CatalogBrandIdDoesNotExistInRepository, [command.CatalogBrandId]);
                throw new CatalogBrandNotExistingInRepositoryException([command.CatalogBrandId]);
            }

            catalogItem.SetCatalogBrandId(command.CatalogBrandId);

            var catalogCategory = await this.categoryRepository.GetAsync(command.CatalogCategoryId);
            if (catalogCategory == null)
            {
                this.logger.LogInformation(Events.CatalogCategoryIdDoesNotExistInRepository, LogMessages.CatalogCategoryIdDoesNotExistInRepository, [command.CatalogCategoryId]);
                throw new CatalogCategoryNotExistingInRepositoryException([command.CatalogCategoryId]);
            }

            catalogItem.SetCatalogCategoryId(command.CatalogCategoryId);

            await this.catalogRepository.UpdateAsync(catalogItem, cancellationToken);
            scope.Complete();
        }
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
            itemsOnPage = await this.catalogRepository.FindAsync(
                item =>
                    (!brandId.HasValue || item.CatalogBrandId == brandId) &&
                    (!categoryId.HasValue || item.CatalogCategoryId == categoryId),
                skip,
                take,
                cancellationToken);
            totalItems = await this.catalogRepository.CountAsync(
                item =>
                    (!brandId.HasValue || item.CatalogBrandId == brandId) &&
                    (!categoryId.HasValue || item.CatalogCategoryId == categoryId),
                cancellationToken);
            scope.Complete();
        }

        this.logger.LogDebug(Events.DebugEvent, LogMessages.CatalogApplicationService_GetCatalogItemsAsyncEnd, brandId, categoryId);
        return (ItemsOnPage: itemsOnPage, TotalItems: totalItems);
    }

    /// <summary>
    /// 指定したIdのカタログアイテムを取得します。
    /// </summary>
    /// <param name="catalogItemId">カタログアイテムID。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>カタログアイテム。</returns>
    public async Task<CatalogItem?> GetCatalogItemAsync(long catalogItemId, CancellationToken cancellationToken = default)
    {
        CatalogItem? catalogItem;
        using (var scope = TransactionScopeManager.CreateTransactionScope())
        {
            catalogItem = await this.catalogRepository.GetAsync(catalogItemId, cancellationToken);
            scope.Complete();
        }

        return catalogItem;
    }
}
