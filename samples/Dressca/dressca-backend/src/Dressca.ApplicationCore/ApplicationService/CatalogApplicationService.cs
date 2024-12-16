using System.Linq.Expressions;
using Dressca.ApplicationCore.Authorization;
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
    private readonly IUserStore userStore;
    private readonly ICatalogDomainService catalogDomainService;
    private readonly ILogger<CatalogApplicationService> logger;

    /// <summary>
    ///  <see cref="CatalogApplicationService"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="catalogRepository">カタログリポジトリ。</param>
    /// <param name="brandRepository">ブランドリポジトリ。</param>
    /// <param name="categoryRepository">カテゴリリポジトリ。</param>
    /// <param name="userStore">ユーザーのセッション情報のストア。</param>
    /// <param name="catalogDomainService">カタログドメインサービス。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="catalogRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="brandRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="categoryRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="userStore"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="catalogDomainService"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public CatalogApplicationService(
        ICatalogRepository catalogRepository,
        ICatalogBrandRepository brandRepository,
        ICatalogCategoryRepository categoryRepository,
        IUserStore userStore,
        ICatalogDomainService catalogDomainService,
        ILogger<CatalogApplicationService> logger)
    {
        this.catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
        this.brandRepository = brandRepository ?? throw new ArgumentNullException(nameof(brandRepository));
        this.categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        this.userStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
        this.catalogDomainService = catalogDomainService ?? throw new ArgumentNullException(nameof(catalogDomainService));
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

    /// <summary>
    /// カタログにアイテムを追加します。
    /// </summary>
    /// <param name="name">商品名。</param>
    /// <param name="description">説明。</param>
    /// <param name="price">単価。</param>
    /// <param name="productCode">商品コード。</param>
    /// <param name="catalogBrandId">カタログブランドID。</param>
    /// <param name="catalogCategoryId">カタログカテゴリID。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>追加したカタログアイテムの情報を返す非同期処理を表すタスク。</returns>
    /// <exception cref="PermissionDeniedException">追加権限がない場合。</exception>
    /// <exception cref="CatalogBrandNotExistingInRepositoryException">追加対象のカタログブランドが存在しなかった場合。</exception>
    /// <exception cref="CatalogCategoryNotExistingInRepositoryException">追加対象のカタログカテゴリが存在しなかった場合。</exception>
    public async Task<CatalogItem> AddItemToCatalogAsync(
        string name,
        string description,
        decimal price,
        string productCode,
        long catalogBrandId,
        long catalogCategoryId,
        CancellationToken cancellationToken = default)
    {
        this.logger.LogDebug(Events.DebugEvent, LogMessages.CatalogApplicationService_AddItemToCatalogAsyncStart);

        if (!this.userStore.IsInRole(Roles.Admin))
        {
            throw new PermissionDeniedException();
        }

        if (!await this.catalogDomainService.BrandExistsAsync(catalogBrandId, cancellationToken))
        {
            this.logger.LogInformation(Events.CatalogBrandIdDoesNotExistInRepository, LogMessages.CatalogBrandIdDoesNotExistInRepository, [catalogBrandId]);
            throw new CatalogBrandNotExistingInRepositoryException([catalogBrandId]);
        }

        if (!await this.catalogDomainService.CategoryExistsAsync(catalogCategoryId, cancellationToken))
        {
            this.logger.LogInformation(Events.CatalogCategoryIdDoesNotExistInRepository, LogMessages.CatalogCategoryIdDoesNotExistInRepository, [catalogCategoryId]);
            throw new CatalogCategoryNotExistingInRepositoryException([catalogCategoryId]);
        }

        var catalogItem = new CatalogItem()
        {
            Name = name,
            Description = description,
            Price = price,
            ProductCode = productCode,
            CatalogBrandId = catalogBrandId,
            CatalogCategoryId = catalogCategoryId,
        };
        CatalogItem catalogItemAdded;

        using (var scope = TransactionScopeManager.CreateTransactionScope())
        {
            catalogItemAdded = await this.catalogRepository.AddAsync(catalogItem, cancellationToken);
            scope.Complete();
        }

        this.logger.LogDebug(Events.DebugEvent, LogMessages.CatalogApplicationService_AddItemToCatalogAsyncEnd, catalogItemAdded.Id);
        return catalogItemAdded;
    }

    /// <summary>
    /// カタログからアイテムを削除します。
    /// </summary>
    /// <param name="id">削除対象のカタログアイテムの ID 。</param>
    /// <param name="rowVersion">削除対象のカタログアイテムの行バージョン。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>処理結果を返す非同期処理を表すタスク。</returns>
    /// <exception cref="PermissionDeniedException">削除権限がない場合。</exception>
    /// <exception cref="CatalogItemNotExistingInRepositoryException">削除対象のカタログアイテムが存在しなかった場合。</exception>>
    public async Task DeleteItemFromCatalogAsync(long id, byte[] rowVersion, CancellationToken cancellationToken = default)
    {
        this.logger.LogDebug(Events.DebugEvent, LogMessages.CatalogApplicationService_DeleteItemFromCatalogAsyncStart, id);

        if (!this.userStore.IsInRole(Roles.Admin))
        {
            throw new PermissionDeniedException();
        }

        if (!await this.catalogDomainService.ItemExistsAsync(id, cancellationToken))
        {
            this.logger.LogInformation(Events.CatalogItemIdDoesNotExistInRepository, LogMessages.CatalogItemIdDoesNotExistInRepository, [id]);
            throw new CatalogItemNotExistingInRepositoryException([id]);
        }

        using (var scope = TransactionScopeManager.CreateTransactionScope())
        {
            await this.catalogRepository.RemoveAsync(id, rowVersion, cancellationToken);
            scope.Complete();
        }

        this.logger.LogDebug(Events.DebugEvent, LogMessages.CatalogApplicationService_DeleteItemFromCatalogAsyncEnd, id);
    }

    /// <summary>
    /// カタログアイテムを更新します。
    /// </summary>
    /// <param name="id">カタログアイテム ID 。</param>
    /// <param name="name">商品名。</param>
    /// <param name="description">説明。</param>
    /// <param name="price">単価。</param>
    /// <param name="productCode">商品コード。</param>
    /// <param name="catalogBrandId">カタログブランド ID 。</param>
    /// <param name="catalogCategoryId">カタログカテゴリ ID 。</param>
    /// <param name="rowVersion">行バージョン。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>処理結果を返す非同期処理を表すタスク。</returns>
    /// <exception cref="PermissionDeniedException">更新権限がない場合。</exception>
    /// <exception cref="CatalogItemNotExistingInRepositoryException">更新対象のカタログアイテムが存在しなかった場合。</exception>
    /// <exception cref="CatalogBrandNotExistingInRepositoryException">更新対象のカタログブランドが存在しなかった場合。</exception>
    /// <exception cref="CatalogCategoryNotExistingInRepositoryException">更新対象のカタログカテゴリが存在しなかった場合。</exception>
    public async Task UpdateCatalogItemAsync(
                long id,
                string name,
                string description,
                decimal price,
                string productCode,
                long catalogBrandId,
                long catalogCategoryId,
                byte[] rowVersion,
                CancellationToken cancellationToken = default)
    {
        this.logger.LogDebug(Events.DebugEvent, LogMessages.CatalogApplicationService_UpdateCatalogItemAsyncStart, id);

        if (!this.userStore.IsInRole(Roles.Admin))
        {
            throw new PermissionDeniedException();
        }

        if (!await this.catalogDomainService.ItemExistsAsync(id, cancellationToken))
        {
            this.logger.LogInformation(Events.CatalogItemIdDoesNotExistInRepository, LogMessages.CatalogItemIdDoesNotExistInRepository, [id]);
            throw new CatalogItemNotExistingInRepositoryException([id]);
        }

        if (!await this.catalogDomainService.BrandExistsAsync(catalogBrandId, cancellationToken))
        {
            this.logger.LogInformation(Events.CatalogBrandIdDoesNotExistInRepository, LogMessages.CatalogBrandIdDoesNotExistInRepository, [catalogBrandId]);
            throw new CatalogBrandNotExistingInRepositoryException([catalogBrandId]);
        }

        if (!await this.catalogDomainService.CategoryExistsAsync(catalogCategoryId, cancellationToken))
        {
            this.logger.LogInformation(Events.CatalogCategoryIdDoesNotExistInRepository, LogMessages.CatalogCategoryIdDoesNotExistInRepository, [catalogCategoryId]);
            throw new CatalogCategoryNotExistingInRepositoryException([catalogCategoryId]);
        }

        var catalogItem = new CatalogItem()
        {
            Id = id,
            Name = name,
            Description = description,
            Price = price,
            ProductCode = productCode,
            CatalogBrandId = catalogBrandId,
            CatalogCategoryId = catalogCategoryId,
            RowVersion = rowVersion,
        };
        using (var scope = TransactionScopeManager.CreateTransactionScope())
        {
            await this.catalogRepository.UpdateAsync(catalogItem, cancellationToken);
            scope.Complete();
        }

        this.logger.LogDebug(Events.DebugEvent, LogMessages.CatalogApplicationService_UpdateCatalogItemAsyncEnd, id);
    }

    /// <summary>
    ///  管理者がカタログ情報を取得します。
    /// </summary>
    /// <param name="skip">読み飛ばす項目数。</param>
    /// <param name="take">最大取得項目数。</param>
    /// <param name="brandId">カタログブランド ID 。</param>
    /// <param name="categoryId">カタログカテゴリ ID 。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>カタログページと総アイテム数のタプルを返す非同期処理を表すタスク。</returns>
    /// <exception cref="PermissionDeniedException">更新権限がない場合。</exception>
    public async Task<(IReadOnlyList<CatalogItem> ItemsOnPage, int TotalItems)> GetCatalogItemsByAdminAsync(int skip, int take, long? brandId, long? categoryId, CancellationToken cancellationToken = default)
    {
        this.logger.LogDebug(Events.DebugEvent, LogMessages.CatalogApplicationService_GetCatalogItemsByAdminAsyncStart, brandId, categoryId);

        if (!this.userStore.IsInRole(Roles.Admin))
        {
            throw new PermissionDeniedException();
        }

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

        this.logger.LogDebug(Events.DebugEvent, LogMessages.CatalogApplicationService_GetCatalogItemsByAdminAsyncEnd, brandId, categoryId);
        return (ItemsOnPage: itemsOnPage, TotalItems: totalItems);
    }

    /// <summary>
    ///  管理者が指定したID のカタログアイテムを取得します。
    /// </summary>
    /// <param name="catalogItemId">カタログアイテム ID 。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>カタログアイテム。</returns>
    /// <exception cref="PermissionDeniedException">更新権限がない場合。</exception>
    /// <exception cref="CatalogItemNotExistingInRepositoryException">取得対象のカタログアイテムが存在しなかった場合。</exception>
    public async Task<CatalogItem?> GetCatalogItemByAdminAsync(long catalogItemId, CancellationToken cancellationToken = default)
    {
        this.logger.LogDebug(Events.DebugEvent, LogMessages.CatalogApplicationService_GetCatalogItemByAdminAsyncStart, catalogItemId);

        if (!this.userStore.IsInRole(Roles.Admin))
        {
            throw new PermissionDeniedException();
        }

        CatalogItem? catalogItem;
        using (var scope = TransactionScopeManager.CreateTransactionScope())
        {
            catalogItem = await this.catalogRepository.GetAsync(catalogItemId, cancellationToken);
            if (catalogItem == null)
            {
                this.logger.LogInformation(Events.CatalogItemIdDoesNotExistInRepository, LogMessages.CatalogItemIdDoesNotExistInRepository, [catalogItemId]);
                throw new CatalogItemNotExistingInRepositoryException([catalogItemId]);
            }

            scope.Complete();
        }

        this.logger.LogDebug(Events.DebugEvent, LogMessages.CatalogApplicationService_GetCatalogItemByAdminAsyncEnd, catalogItemId);
        return catalogItem;
    }
}
