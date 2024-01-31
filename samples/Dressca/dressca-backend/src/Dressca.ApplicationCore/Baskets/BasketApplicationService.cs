using System.Collections.Generic;
using System.Transactions;
using Dressca.ApplicationCore.Catalog;
using Dressca.ApplicationCore.Resources;
using Microsoft.Extensions.Logging;

namespace Dressca.ApplicationCore.Baskets;

/// <summary>
///  買い物かごに関するビジネスユースケースを実現するアプリケーションサービスです。
/// </summary>
public class BasketApplicationService
{
    private readonly ICatalogRepository catalogRepository;
    private readonly IBasketDomainService basketDomainService;
    private readonly ICatalogDomainService catalogDomainService;
    private readonly ILogger<BasketApplicationService> logger;

    /// <summary>
    ///  <see cref="BasketApplicationService"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="catalogRepository">カタログリポジトリ。</param>
    /// <param name="basketDomainService">買い物かごドメインサービス。</param>
    /// <param name="catalogDomainService">カタログドメインサービス。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="catalogRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="basketDomainService"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="catalogDomainService"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>/// </exception>
    public BasketApplicationService(
        ICatalogRepository catalogRepository,
        IBasketDomainService basketDomainService,
        ICatalogDomainService catalogDomainService,
        ILogger<BasketApplicationService> logger)
    {
        this.catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
        this.basketDomainService = basketDomainService ?? throw new ArgumentNullException(nameof(basketDomainService));
        this.catalogDomainService = catalogDomainService ?? throw new ArgumentNullException(nameof(catalogDomainService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<(Basket BasketResult, IReadOnlyList<CatalogItem> CatalogItems)> GetBasketItemsAsync(string buyerId)
    {
        Basket basket;
        IReadOnlyList<CatalogItem> catalogItems;

        using (var scope = new TransactionScope(
            TransactionScopeOption.RequiresNew,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled))
        {
            basket = await this.basketDomainService.GetOrCreateBasketForUserAsync(buyerId);
            var catalogItemIds = basket.Items.Select(basketItem => basketItem.CatalogItemId).ToList();
            catalogItems = await this.catalogRepository.FindAsync(catalogItem => catalogItemIds.Contains(catalogItem.Id));

            scope.Complete();
        }

        return (BasketResult: basket, CatalogItems: catalogItems);
    }

    public async Task<bool> ChangeBasketItemsQuantitiesAsync(string buyerId, Dictionary<long, int> quantities)
    {
        using (var scope = new TransactionScope(
            TransactionScopeOption.RequiresNew,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled))
        {
            // 買い物かごに入っていないカタログアイテムが指定されていないか確認
            var basket = await this.basketDomainService.GetOrCreateBasketForUserAsync(buyerId);
            var notExistsInBasketCatalogIds = quantities.Keys.Where(catalogItemId => !basket.IsInCatalogItem(catalogItemId));
            if (notExistsInBasketCatalogIds.Any())
            {
                // 後ほどログメッセージ追加
                //this.logger.LogWarning(Messages.CatalogItemIdDoesNotExistInBasket, string.Join(',', notExistsInBasketCatalogIds));
                return false;
            }

            // カタログリポジトリに存在しないカタログアイテムが指定されていないか確認
            var (existsAll, _) = await this.catalogDomainService.ExistsAllAsync(quantities.Keys);
            if (!existsAll)
            {
                return false;
            }

            await this.basketDomainService.SetQuantitiesAsync(basket.Id, quantities);

            scope.Complete();
        }

        return true;
    }

    public async Task<bool> AddItemToBasketAsync(string buyerId, long catalogItemId, int addedQuantity)
    {
        using (var scope = new TransactionScope(
            TransactionScopeOption.RequiresNew,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled))
        {
            var basket = await this.basketDomainService.GetOrCreateBasketForUserAsync(buyerId);

            // カタログリポジトリに存在しないカタログアイテムが指定されていないか確認
            var (existsAll, catalogItems) = await this.catalogDomainService.ExistsAllAsync(new[] { catalogItemId });
            if (!existsAll)
            {
                return false;
            }

            var catalogItem = catalogItems[0];
            await this.basketDomainService.AddItemToBasketAsync(basket.Id, catalogItemId, catalogItem.Price, addedQuantity);

            scope.Complete();
        }

        return true;
    }

    public async Task<bool> DeleteBasketItemAsync(string buyerId, long catalogItemId)
    {
        using (var scope = new TransactionScope(
            TransactionScopeOption.RequiresNew,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled))
        {
            var basket = await this.basketDomainService.GetOrCreateBasketForUserAsync(buyerId);
            if (!basket.IsInCatalogItem(catalogItemId))
            {
                // 後ほどログメッセージ追加
                //this.logger.LogWarning(Messages.CatalogItemIdDoesNotExistInBasket, catalogItemId);
                return false;
            }

            await this.basketDomainService.SetQuantitiesAsync(basket.Id, new() { { catalogItemId, 0 } });

            scope.Complete();
        }

        return true;
    }
}
