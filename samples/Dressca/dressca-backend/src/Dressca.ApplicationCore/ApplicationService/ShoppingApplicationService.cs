using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.Catalog;
using Dressca.ApplicationCore.Ordering;
using Dressca.ApplicationCore.Resources;
using Microsoft.Extensions.Logging;

namespace Dressca.ApplicationCore.ApplicationService;

/// <summary>
///  買い物に関するビジネスユースケースを実現するアプリケーションサービスです。
/// </summary>
public class ShoppingApplicationService
{
    private readonly IBasketRepository basketRepository;
    private readonly IOrderRepository orderRepository;
    private readonly IOrderFactory orderFactory;
    private readonly ICatalogRepository catalogRepository;
    private readonly ICatalogDomainService catalogDomainService;
    private readonly ILogger<ShoppingApplicationService> logger;

    /// <summary>
    ///  <see cref="ShoppingApplicationService"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="basketRepository">買い物かごリポジトリ。</param>
    /// <param name="orderRepository">注文リポジトリ。</param>
    /// <param name="orderFactory">注文エンティティファクトリー。</param>
    /// <param name="catalogRepository">カタログリポジトリ。</param>
    /// <param name="catalogDomainService">カタログドメインサービス。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="basketRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="orderRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="orderFactory"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="catalogRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="catalogDomainService"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>/// </exception>
    public ShoppingApplicationService(
        IBasketRepository basketRepository,
        IOrderRepository orderRepository,
        IOrderFactory orderFactory,
        ICatalogRepository catalogRepository,
        ICatalogDomainService catalogDomainService,
        ILogger<ShoppingApplicationService> logger)
    {
        this.basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        this.orderFactory = orderFactory ?? throw new ArgumentNullException(nameof(orderFactory));
        this.catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
        this.catalogDomainService = catalogDomainService ?? throw new ArgumentNullException(nameof(catalogDomainService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///  買い物かごアイテムの一覧を取得します。
    /// </summary>
    /// <param name="buyerId">購入者 Id 。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>
    ///  BasketResult : <paramref name="buyerId"/> に対応する買い物かご。
    ///  CatalogItems : 買い物かごアイテムの一覧。
    /// </returns>
    public async Task<(Basket BasketResult, IReadOnlyList<CatalogItem> CatalogItems, List<long> DeletedItemIds)> GetBasketItemsAsync(string buyerId, CancellationToken cancellationToken = default)
    {
        this.logger.LogDebug(Events.DebugEvent, LogMessages.ShoppingApplicationService_GetBasketItemsAsyncStart, buyerId);

        Basket basket;
        IReadOnlyList<CatalogItem> catalogItems;
        using (var scope = TransactionScopeManager.CreateTransactionScope())
        {
            basket = await this.GetOrCreateBasketForUserAsync(buyerId, cancellationToken);
            var catalogItemIds = basket.Items.Select(basketItem => basketItem.CatalogItemId).ToList();
            catalogItems = await this.catalogRepository.FindAsync(catalogItem => catalogItemIds.Contains(catalogItem.Id), cancellationToken);
            scope.Complete();
        }

        var deletedCatalogItemIds = catalogItems.Where(item => item.IsDeleted == true).Select(item => item.Id).ToList();

        this.logger.LogDebug(Events.DebugEvent, LogMessages.ShoppingApplicationService_GetBasketItemsAsyncEnd, buyerId);
        return (BasketResult: basket, CatalogItems: catalogItems, DeletedItemIds: deletedCatalogItemIds);
    }

    /// <summary>
    ///  買い物かごの各アイテムの数量を一括で設定します。
    /// </summary>
    /// <param name="buyerId">購入者 Id 。</param>
    /// <param name="quantities">各カタログアイテムの数量。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>処理結果を返す非同期処理を表すタスク。</returns>
    public async Task SetBasketItemsQuantitiesAsync(string buyerId, Dictionary<long, int> quantities, CancellationToken cancellationToken = default)
    {
        this.logger.LogDebug(Events.DebugEvent, LogMessages.ShoppingApplicationService_SetBasketItemsQuantitiesAsyncStart, buyerId);

        using (var scope = TransactionScopeManager.CreateTransactionScope())
        {
            var basket = await this.GetOrCreateBasketForUserAsync(buyerId, cancellationToken);

            // 買い物かごに入っていないカタログアイテムが指定されていないか確認
            var notExistsInBasketCatalogIds = quantities.Keys.Where(catalogItemId => !basket.IsInCatalogItem(catalogItemId));
            if (notExistsInBasketCatalogIds.Any())
            {
                throw new CatalogItemNotExistingInBasketException(notExistsInBasketCatalogIds);
            }

            // カタログリポジトリに存在しないカタログアイテムが指定されていないか確認
            var (existsAll, existingCatalogItems) = await this.catalogDomainService.ExistsAllAsync(quantities.Keys, cancellationToken);
            if (!existsAll)
            {
                var notExistingInRepositoryCatalogIds =
                    quantities.Keys
                       .Where(catalogItemId => existingCatalogItems.Select(item => item.Id).Any(id => id != catalogItemId));
                throw new CatalogItemNotExistingInRepositoryException(notExistingInRepositoryCatalogIds);
            }

            basket.SetItemsQuantity(quantities);
            var currentBasketItems = basket.Items.Select(i => string.Format(Messages.Basket_ItemQuantity, i.CatalogItemId, i.Quantity));
            this.logger.LogDebug(Events.DebugEvent, LogMessages.Basket_AfterSettingQuantity, string.Join(";", currentBasketItems));
            basket.RemoveEmptyItems();
            await this.basketRepository.UpdateAsync(basket, cancellationToken);
            scope.Complete();
        }

        this.logger.LogDebug(Events.DebugEvent, LogMessages.ShoppingApplicationService_SetBasketItemsQuantitiesAsyncEnd, buyerId);
    }

    /// <summary>
    ///  買い物かごにアイテムを追加します。
    /// </summary>
    /// <param name="buyerId">購入者 Id 。</param>
    /// <param name="catalogItemId">カタログアイテム Id 。</param>
    /// <param name="addedQuantity">数量。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>処理結果を返す非同期処理を表すタスク。</returns>
    public async Task AddItemToBasketAsync(string buyerId, long catalogItemId, int addedQuantity, CancellationToken cancellationToken = default)
    {
        this.logger.LogDebug(Events.DebugEvent, LogMessages.ShoppingApplicationService_AddItemToBasketAsyncStart, buyerId, catalogItemId, addedQuantity);

        using (var scope = TransactionScopeManager.CreateTransactionScope())
        {
            var basket = await this.GetOrCreateBasketForUserAsync(buyerId, cancellationToken);

            // カタログリポジトリに存在しないカタログアイテムが指定されていないか確認
            var (existsAll, catalogItems) = await this.catalogDomainService.ExistsAllAsync([catalogItemId], cancellationToken);
            if (!existsAll)
            {
                List<long> notExistingInRepositoryCatalogIds = [catalogItemId];
                throw new CatalogItemNotExistingInRepositoryException(notExistingInRepositoryCatalogIds);
            }

            var catalogItem = catalogItems[0];
            basket.AddItem(catalogItemId, catalogItem.Price, addedQuantity);
            basket.RemoveEmptyItems();
            await this.basketRepository.UpdateAsync(basket, cancellationToken);
            scope.Complete();
        }

        this.logger.LogDebug(Events.DebugEvent, LogMessages.ShoppingApplicationService_AddItemToBasketAsyncEnd, buyerId, catalogItemId, addedQuantity);
    }

    /// <summary>
    ///  買い物かご内の商品を注文します。
    /// </summary>
    /// <param name="buyerId">購入者 Id 。</param>
    /// <param name="shipToAddress">お届け先。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>作成した注文情報を返す非同期処理を表すタスク。</returns>
    /// <exception cref="NullBasketOnCheckoutException">注文を作成する対象の買い物かごが存在しない場合。</exception>
    /// <exception cref="EmptyBasketOnCheckoutException">注文を作成する対象の買い物かごが空の場合。</exception>
    public async Task<Order> CheckoutAsync(string buyerId, ShipTo shipToAddress, CancellationToken cancellationToken = default)
    {
        this.logger.LogDebug(Events.DebugEvent, LogMessages.ShoppingApplicationService_CheckoutAsyncStart, buyerId);

        if (string.IsNullOrWhiteSpace(buyerId))
        {
            throw new ArgumentException(Messages.ArgumentIsNullOrWhiteSpace, nameof(buyerId));
        }

        Order ordered;
        Basket? checkoutBasket;
        using (var scope = TransactionScopeManager.CreateTransactionScope())
        {
            checkoutBasket = await this.basketRepository.GetWithBasketItemsAsync(buyerId, cancellationToken);

            if (checkoutBasket is null)
            {
                throw new NullBasketOnCheckoutException();
            }

            if (checkoutBasket.IsEmpty())
            {
                throw new EmptyBasketOnCheckoutException();
            }

            var catalogItemIds = checkoutBasket.Items.Select(item => item.CatalogItemId).ToArray();
            var catalogItems =
                await this.catalogRepository.FindAsync(item => catalogItemIds.Contains(item.Id), cancellationToken);
            var order = this.orderFactory.CreateOrder(checkoutBasket, catalogItems, shipToAddress);
            ordered = await this.orderRepository.AddAsync(order, cancellationToken);

            // 買い物かごを削除
            await this.basketRepository.RemoveAsync(checkoutBasket, cancellationToken);
            scope.Complete();
        }

        this.logger.LogDebug(Events.DebugEvent, LogMessages.ShoppingApplicationService_CheckoutAsyncEnd, checkoutBasket.Id, ordered.Id);
        return ordered;
    }

    private async Task<Basket> GetOrCreateBasketForUserAsync(string buyerId, CancellationToken cancellationToken)
    {
        this.logger.LogDebug(Events.DebugEvent, LogMessages.ShoppingApplicationService_GetOrCreateBasketForUserAsyncStart, buyerId);

        if (string.IsNullOrWhiteSpace(buyerId))
        {
            throw new ArgumentException(Messages.ArgumentIsNullOrWhiteSpace, nameof(buyerId));
        }

        var basket = await this.basketRepository.GetWithBasketItemsAsync(buyerId, cancellationToken);
        if (basket is null)
        {
            this.logger.LogDebug(Events.DebugEvent, LogMessages.CreateNewBasket_UserBasketNotFound, buyerId);
            basket = new Basket { BuyerId = buyerId };
            return await this.basketRepository.AddAsync(basket, cancellationToken);
        }

        this.logger.LogDebug(Events.DebugEvent, LogMessages.ShoppingApplicationService_GetOrCreateBasketForUserAsyncEnd, buyerId);
        return basket;
    }
}
