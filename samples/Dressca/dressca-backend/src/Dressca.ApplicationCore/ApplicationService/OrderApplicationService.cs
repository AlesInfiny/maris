using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.Catalog;
using Dressca.ApplicationCore.Ordering;
using Dressca.ApplicationCore.Resources;
using Microsoft.Extensions.Logging;

namespace Dressca.ApplicationCore.ApplicationService;

/// <summary>
///  注文情報に関するビジネスユースケースを実現するアプリケーションサービスです。
/// </summary>
public class OrderApplicationService
{
    private readonly IOrderRepository orderRepository;
    private readonly ILogger<ShoppingApplicationService> logger;

    /// <summary>
    ///  <see cref="OrderApplicationService"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="orderRepository">注文リポジトリ。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="orderRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>/// </exception>
    public OrderApplicationService(
        IOrderRepository orderRepository,
        ILogger<ShoppingApplicationService> logger)
    {
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        if (order is null || !order.HasMatchingBuyerId(buyerId))
        {
            throw new OrderNotFoundException(orderId, buyerId);
        }

        this.logger.LogDebug(Messages.OrderApplicationService_GetOrderAsyncEnd, orderId);
        return order;
    }
}
