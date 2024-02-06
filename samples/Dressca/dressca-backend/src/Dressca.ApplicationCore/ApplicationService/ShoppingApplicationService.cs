using System.Transactions;
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
    private readonly ICatalogRepository catalogRepository;
    private readonly ICatalogDomainService catalogDomainService;
    private readonly ILogger<ShoppingApplicationService> logger;

    /// <summary>
    ///  <see cref="ShoppingApplicationService"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="basketRepository">買い物かごリポジトリ。</param>
    /// <param name="orderRepository">注文リポジトリ。</param>
    /// <param name="catalogRepository">カタログリポジトリ。</param>
    /// <param name="catalogDomainService">カタログドメインサービス。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="catalogRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="catalogDomainService"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>/// </exception>
    public ShoppingApplicationService(
        IBasketRepository basketRepository,
        IOrderRepository orderRepository,
        ICatalogRepository catalogRepository,
        ICatalogDomainService catalogDomainService,
        ILogger<ShoppingApplicationService> logger)
    {
        this.basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
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
    public async Task<(Basket BasketResult, IReadOnlyList<CatalogItem> CatalogItems)> GetBasketItemsAsync(string buyerId, CancellationToken cancellationToken = default)
    {
        Basket basket;
        IReadOnlyList<CatalogItem> catalogItems;

        using (var scope = new TransactionScope(
            TransactionScopeOption.RequiresNew,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled))
        {
            basket = await this.GetOrCreateBasketForUserAsync(buyerId, cancellationToken);
            var catalogItemIds = basket.Items.Select(basketItem => basketItem.CatalogItemId).ToList();
            catalogItems = await this.catalogRepository.FindAsync(catalogItem => catalogItemIds.Contains(catalogItem.Id));

            scope.Complete();
        }

        return (BasketResult: basket, CatalogItems: catalogItems);
    }

    /// <summary>
    ///  買い物かごの各アイテムの数量を一括で設定します。
    /// </summary>
    /// <param name="buyerId">購入者 Id 。</param>
    /// <param name="quantities">各カタログアイテムの数量。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>処理結果を返す非同期処理を表すタスク。</returns>
    public async Task<bool> SetBasketItemsQuantitiesAsync(string buyerId, Dictionary<long, int> quantities, CancellationToken cancellationToken = default)
    {
        using (var scope = new TransactionScope(
            TransactionScopeOption.RequiresNew,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled))
        {
            // 買い物かごに入っていないカタログアイテムが指定されていないか確認
            var basket = await this.GetOrCreateBasketForUserAsync(buyerId, cancellationToken);
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

            basket.SetItemsQuantity(quantities);
            basket.RemoveEmptyItems();
            await this.basketRepository.UpdateAsync(basket, cancellationToken);

            scope.Complete();
        }

        return true;
    }

    /// <summary>
    ///  買い物かごにアイテムを追加します。
    /// </summary>
    /// <param name="buyerId">購入者 Id 。</param>
    /// <param name="catalogItemId">カタログアイテム Id 。</param>
    /// <param name="addedQuantity">数量。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>処理結果を返す非同期処理を表すタスク。</returns>
    public async Task<bool> AddItemToBasketAsync(string buyerId, long catalogItemId, int addedQuantity, CancellationToken cancellationToken = default)
    {
        using (var scope = new TransactionScope(
            TransactionScopeOption.RequiresNew,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled))
        {
            var basket = await this.GetOrCreateBasketForUserAsync(buyerId, cancellationToken);

            // カタログリポジトリに存在しないカタログアイテムが指定されていないか確認
            var (existsAll, catalogItems) = await this.catalogDomainService.ExistsAllAsync(new[] { catalogItemId });
            if (!existsAll)
            {
                return false;
            }

            var catalogItem = catalogItems[0];
            basket.AddItem(catalogItemId, catalogItem.Price, addedQuantity);
            basket.RemoveEmptyItems();
            await this.basketRepository.UpdateAsync(basket, cancellationToken);

            scope.Complete();
        }

        return true;
    }

    /// <summary>
    ///  注文を作成します。
    /// </summary>
    /// <param name="buyerId">購入者 Id 。</param>
    /// <param name="shipToAddress">お届け先。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>作成した注文情報を返す非同期処理を表すタスク。</returns>
    /// <exception cref="EmptyBasketOnCheckoutException">注文を作成する対象の買い物かごが空の場合。</exception>
    public async Task<Order> CreateOrderAsync(string buyerId, ShipTo shipToAddress, CancellationToken cancellationToken = default)
    {
        //this.logger.LogDebug(Messages.OrderApplicationService_CreateOrderAsyncStart, basketId);

        Order ordered;
        Basket checkoutBasket;

        using (var scope = new TransactionScope(
            TransactionScopeOption.RequiresNew,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled))
        {
            checkoutBasket = await this.GetOrCreateBasketForUserAsync(buyerId, cancellationToken);

            if (checkoutBasket.IsEmpty())
            {
                throw new EmptyBasketOnCheckoutException();
            }

            var catalogItemIds = checkoutBasket.Items.Select(item => item.CatalogItemId).ToArray();
            var catalogItems =
                await this.catalogRepository.FindAsync(item => catalogItemIds.Contains(item.Id), cancellationToken);
            var orderItems = checkoutBasket.GetOrderItems(catalogItems);
            var order = new Order(checkoutBasket.BuyerId, shipToAddress, orderItems);
            ordered = await this.orderRepository.AddAsync(order, cancellationToken);

            // 買い物かごを削除
            await this.basketRepository.RemoveAsync(checkoutBasket, cancellationToken);

            scope.Complete();
        }

        //this.logger.LogDebug(Messages.OrderApplicationService_CreateOrderAsyncEnd, checkoutBasket.Id, ordered.Id);

        return ordered;
    }

    /// <summary>
    ///  指定した注文 Id 、購入者 Id の注文情報を取得します。
    /// </summary>
    /// <param name="orderId">注文 Id 。</param>
    /// <param name="buyerId">購入者 Id 。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>注文情報。</returns>
    /// <exception cref="OrderNotFoundException">注文情報が見つからない場合。</exception>
    public async Task<Order> GetOrderAsync(long orderId, string buyerId, CancellationToken cancellationToken = default)
    {
        this.logger.LogDebug(Messages.OrderApplicationService_GetOrderAsyncStart, orderId);
        var order = await this.orderRepository.FindAsync(orderId, cancellationToken);
        if (order is null || order.BuyerId != buyerId)
        {
            throw new OrderNotFoundException(orderId, buyerId);
        }

        this.logger.LogDebug(Messages.OrderApplicationService_GetOrderAsyncEnd, orderId);
        return order;
    }

    private async Task<Basket> GetOrCreateBasketForUserAsync(string buyerId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(buyerId))
        {
            throw new ArgumentException(Messages.ArgumentIsNullOrWhiteSpace, nameof(buyerId));
        }

        var basket = await this.basketRepository.GetWithBasketItemsAsync(buyerId, cancellationToken);
        if (basket is null)
        {
            this.logger.LogDebug(Messages.CreateNewBasket_UserBasketNotFound, buyerId);
            basket = new Basket(buyerId);
            return await this.basketRepository.AddAsync(basket, cancellationToken);
        }

        return basket;
    }
}
