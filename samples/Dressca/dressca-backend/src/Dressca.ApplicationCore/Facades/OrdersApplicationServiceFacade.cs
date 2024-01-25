using System.Transactions;
using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.Ordering;

namespace Dressca.ApplicationCore.Facades;

/// <summary>
/// 注文に関する操作のトランザクション管理を行う Facade クラスです。
/// </summary>
public class OrdersApplicationServiceFacade
{
    private readonly OrderApplicationService orderApplicationService;
    private readonly BasketApplicationService basketApplicationService;

    /// <summary>
    ///  <see cref="OrdersApplicationServiceFacade"/> の新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="orderApplicationService">注文アプリケーションサービス。</param>
    /// <param name="basketApplicationService">買い物かごアプリケーションサービス。</param>
    /// <exception cref="ArgumentNullException">
    ///   <item><paramref name="orderApplicationService"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="basketApplicationService"/> が <see langword="null"/> です。</item>
    /// </exception>
    public OrdersApplicationServiceFacade(
        OrderApplicationService orderApplicationService,
        BasketApplicationService basketApplicationService)
    {
        this.orderApplicationService = orderApplicationService ?? throw new ArgumentNullException(nameof(orderApplicationService));
        this.basketApplicationService = basketApplicationService ?? throw new ArgumentNullException(nameof(basketApplicationService));
    }

    /// <summary>
    ///  注文情報を取得するトランザクションを開始します。
    /// </summary>
    /// <param name="orderId">注文 Id 。</param>
    /// <param name="buyerId">購入者 Id 。</param>
    /// <returns>注文情報。</returns>
    public async Task<Order> GetOrderAsync(long orderId, string buyerId)
    {
        Order order;

        using (var scope = new TransactionScope(
            TransactionScopeOption.RequiresNew,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled))
        {
            order = await this.orderApplicationService.GetOrderAsync(orderId, buyerId);

            scope.Complete();
        }

        return order;
    }

    /// <summary>
    ///  商品を注文するトランザクションを開始します。
    /// </summary>
    /// <param name="shipToAddress">お届け先。</param>
    /// <param name="buyerId">購入者 Id 。</param>
    /// <returns>作成した注文情報。</returns>
    public async Task<Order> PostOrderAsync(ShipTo shipToAddress, string buyerId)
    {
        Order order;

        using (var scope = new TransactionScope(
            TransactionScopeOption.RequiresNew,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled))
        {
            var basket = await this.basketApplicationService.GetOrCreateBasketForUserAsync(buyerId);
            order = await this.orderApplicationService.CreateOrderAsync(basket.Id, shipToAddress);

            // 買い物かごを削除
            await this.basketApplicationService.DeleteBasketAsync(basket.Id);

            scope.Complete();
        }

        return order;
    }
}
