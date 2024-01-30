using System.Threading;
using System.Transactions;
using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.Catalog;
using Dressca.ApplicationCore.Resources;
using Microsoft.Extensions.Logging;

namespace Dressca.ApplicationCore.Ordering;

/// <summary>
///  注文に関連するビジネスユースケースを実現する Applicaiton Service です。
/// </summary>
public class OrderApplicationService
{
    private readonly IOrderRepository orderRepository;
    private readonly ICatalogRepository catalogRepository;
    private readonly BasketDomainService basketDomainService;
    private readonly ILogger<OrderApplicationService> logger;

    /// <summary>
    ///  <see cref="OrderApplicationService"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="orderRepository">注文リポジトリ。</param>
    /// <param name="basketRepository">買い物かごリポジトリ。</param>
    /// <param name="catalogRepository">カタログリポジトリ。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="orderRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="basketRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="catalogRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public OrderApplicationService(
        IOrderRepository orderRepository,
        IBasketRepository basketRepository,
        ICatalogRepository catalogRepository,
        BasketDomainService basketDomainService,
        ILogger<OrderApplicationService> logger)
    {
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        this.catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
        this.basketDomainService = basketDomainService ?? throw new ArgumentNullException(nameof(basketDomainService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Order> PostOrderAsync(string buyerId, ShipTo shipToAddress, CancellationToken cancellationToken = default)
    {
        //this.logger.LogDebug(Messages.OrderApplicationService_CreateOrderAsyncStart, basketId);

        Order ordered;
        Basket checkoutBasket;

        using (var scope = new TransactionScope(
            TransactionScopeOption.RequiresNew,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled))
        {
            checkoutBasket = await this.basketDomainService.GetOrCreateBasketForUserAsync(buyerId);

            if (checkoutBasket.IsEmpty())
            {
                throw new EmptyBasketOnCheckoutException();
            }

            var catalogItemIds = checkoutBasket.Items.Select(item => item.CatalogItemId).ToArray();
            var catalogItems =
                await this.catalogRepository.FindAsync(item => catalogItemIds.Contains(item.Id), cancellationToken);
            var orderItems = checkoutBasket.Items.Select(
                basketItem =>
                {
                    var catalogItem = catalogItems.First(c => c.Id == basketItem.CatalogItemId);
                    var itemOrdered = new CatalogItemOrdered(catalogItem.Id, catalogItem.Name, catalogItem.ProductCode);
                    var orderItem = new OrderItem(itemOrdered, basketItem.UnitPrice, basketItem.Quantity);
                    var orderItemAssets = catalogItem.Assets
                        .Select(catalogItemAsset => new OrderItemAsset(catalogItemAsset.AssetCode, orderItem.Id));
                    orderItem.AddAssets(orderItemAssets);
                    return orderItem;
                }).ToList();
            var order = new Order(checkoutBasket.BuyerId, shipToAddress, orderItems);
            ordered = await this.orderRepository.AddAsync(order, cancellationToken);

            // 買い物かごを削除
            await this.basketDomainService.DeleteBasketAsync(checkoutBasket.Id);

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
}
