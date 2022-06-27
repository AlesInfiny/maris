using Dressca.ApplicationCore.Resources;
using Microsoft.Extensions.Logging;

namespace Dressca.ApplicationCore.Baskets;

/// <summary>
///  買い物かごに関するビジネスユースケースを実現するアプリケーションサービスです。
/// </summary>
public class BasketApplicationService
{
    private readonly IBasketRepository basketRepository;
    private readonly ILogger<BasketApplicationService> logger;

    /// <summary>
    ///  <see cref="BasketApplicationService"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="basketRepository">買い物かごリポジトリ。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="basketRepository"/> または <paramref name="logger"/> が <see langword="null"/> です。
    /// </exception>
    public BasketApplicationService(IBasketRepository basketRepository, ILogger<BasketApplicationService> logger)
    {
        this.basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///  アイテムを買い物かごに追加します。
    /// </summary>
    /// <param name="basketId">買い物かご Id。</param>
    /// <param name="catalogItemId">カタログアイテム Id 。</param>
    /// <param name="price">単価。</param>
    /// <param name="quantity">数量。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    /// <exception cref="BasketNotFoundException">買い物かごが見つからない場合。</exception>
    public async Task AddItemToBasketAsync(long basketId, long catalogItemId, decimal price, int quantity = 1, CancellationToken cancellationToken = default)
    {
        this.logger.LogDebug(Messages.BasketApplicationService_AddItemToBasketAsyncStart, basketId, catalogItemId, quantity);
        var basket = await this.basketRepository.GetAsync(basketId, cancellationToken);
        if (basket is null)
        {
            throw new BasketNotFoundException(basketId);
        }

        basket.AddItem(catalogItemId, price, quantity);
        basket.RemoveEmptyItems();
        await this.basketRepository.UpdateAsync(basket, cancellationToken);
        this.logger.LogDebug(Messages.BasketApplicationService_AddItemToBasketAsyncEnd, basketId, catalogItemId, quantity);
    }

    /// <summary>
    ///  買い物かごを削除します。
    /// </summary>
    /// <param name="basketId">買い物かご Id 。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    /// <exception cref="BasketNotFoundException">買い物かごが見つからない場合。</exception>
    public async Task DeleteBasketAsync(long basketId, CancellationToken cancellationToken = default)
    {
        this.logger.LogDebug(Messages.BasketApplicationService_DeleteBasketAsyncStart, basketId);
        var basket = await this.basketRepository.GetWithBasketItemsAsync(basketId, cancellationToken);
        if (basket is null)
        {
            throw new BasketNotFoundException(basketId);
        }

        await this.basketRepository.RemoveAsync(basket, cancellationToken);
        this.logger.LogDebug(Messages.BasketApplicationService_DeleteBasketAsyncEnd, basketId);
    }

    /// <summary>
    ///  買い物かごの各アイテムの数量を一括で設定します。
    /// </summary>
    /// <param name="basketId">買い物かご Id 。</param>
    /// <param name="quantities">カタログアイテム Id ごとの数量。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="quantities"/> が <see langword="null"/> です。</exception>
    /// <exception cref="InvalidOperationException">買い物かご内のいずれかのアイテムの数量が 0 未満になる場合。</exception>
    /// <exception cref="BasketNotFoundException">買い物かごが見つからない場合。</exception>
    public async Task SetQuantitiesAsync(long basketId, Dictionary<long, int> quantities, CancellationToken cancellationToken = default)
    {
        this.logger.LogDebug(Messages.BasketApplicationService_SetQuantitiesAsyncStart, basketId);
        ArgumentNullException.ThrowIfNull(quantities);
        var basket = await this.basketRepository.GetWithBasketItemsAsync(basketId, cancellationToken);
        if (basket is null)
        {
            throw new BasketNotFoundException(basketId);
        }

        foreach (var item in basket.Items)
        {
            if (quantities.TryGetValue(item.CatalogItemId, out var quantity))
            {
                this.logger.LogDebug(Messages.BasketApplicationService_SetQuantity, item.CatalogItemId, quantity);
                item.SetQuantity(quantity);
            }
        }

        basket.RemoveEmptyItems();
        await this.basketRepository.UpdateAsync(basket, cancellationToken);
        this.logger.LogDebug(Messages.BasketApplicationService_SetQuantitiesAsyncEnd, basketId);
    }

    /// <summary>
    ///  <paramref name="buyerId"/> に対応する買い物かご情報を取得します。
    ///  対応する買い物かご情報がない場合は、作成します。
    /// </summary>
    /// <param name="buyerId">購入者 Id 。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>買い物かご情報を返す非同期処理を表すタスク。</returns>
    /// <exception cref="ArgumentException"><paramref name="buyerId"/> が <see langword="null"/> または空白の場合.</exception>
    public async Task<Basket> GetOrCreateBasketForUserAsync(string buyerId, CancellationToken cancellationToken = default)
    {
        this.logger.LogDebug(Messages.BasketApplicationService_GetOrCreateBasketForUserAsyncStart, buyerId);
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

        this.logger.LogDebug(Messages.BasketApplicationService_GetOrCreateBasketForUserAsyncEnd, buyerId);
        return basket;
    }
}
