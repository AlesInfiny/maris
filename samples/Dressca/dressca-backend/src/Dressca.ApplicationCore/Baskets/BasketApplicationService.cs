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
    /// <returns>非同期処理を表すタスク。</returns>
    /// <exception cref="BasketNotFoundException">買い物かごが見つからない場合。</exception>
    public async Task AddItemToBasket(long basketId, long catalogItemId, decimal price, int quantity = 1)
    {
        this.logger.LogDebug(ApplicationCoreMessages.BasketApplicationService_AddItemToBasketStart, basketId, catalogItemId, quantity);
        var basket = await this.basketRepository.GetAsync(basketId);
        if (basket is null)
        {
            throw new BasketNotFoundException(basketId);
        }

        basket.AddItem(catalogItemId, price, quantity);
        basket.RemoveEmptyItems();
        await this.basketRepository.UpdateAsync(basket);
        this.logger.LogDebug(ApplicationCoreMessages.BasketApplicationService_AddItemToBasketEnd, basketId, catalogItemId, quantity);
    }

    /// <summary>
    ///  買い物かごを削除します。
    /// </summary>
    /// <param name="basketId">買い物かご Id 。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    /// <exception cref="BasketNotFoundException">買い物かごが見つからない場合。</exception>
    public async Task DeleteBasket(long basketId)
    {
        this.logger.LogDebug(ApplicationCoreMessages.BasketApplicationService_DeleteBasketStart, basketId);
        var basket = await this.basketRepository.GetWithBasketItemsAsync(basketId);
        if (basket is null)
        {
            throw new BasketNotFoundException(basketId);
        }

        await this.basketRepository.RemoveAsync(basket);
        this.logger.LogDebug(ApplicationCoreMessages.BasketApplicationService_DeleteBasketEnd, basketId);
    }

    /// <summary>
    ///  買い物かごの各アイテムの数量を一括で設定します。
    /// </summary>
    /// <param name="basketId">買い物かご Id 。</param>
    /// <param name="quantities">カタログアイテム Id ごとの数量。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="quantities"/> が <see langword="null"/> です。</exception>
    /// <exception cref="InvalidOperationException">買い物かご内のいずれかのアイテムの数量が 0 未満になる場合。</exception>
    /// <exception cref="BasketNotFoundException">買い物かごが見つからない場合。</exception>
    public async Task SetQuantities(long basketId, Dictionary<long, int> quantities)
    {
        this.logger.LogDebug(ApplicationCoreMessages.BasketApplicationService_SetQuantitiesStart, basketId);
        ArgumentNullException.ThrowIfNull(quantities);
        var basket = await this.basketRepository.GetWithBasketItemsAsync(basketId);
        if (basket is null)
        {
            throw new BasketNotFoundException(basketId);
        }

        foreach (var item in basket.Items)
        {
            if (quantities.TryGetValue(item.CatalogItemId, out var quantity))
            {
                this.logger.LogDebug(ApplicationCoreMessages.BasketApplicationService_SetQuantities, item.CatalogItemId, quantity);
                item.SetQuantity(quantity);
            }
        }

        basket.RemoveEmptyItems();
        await this.basketRepository.UpdateAsync(basket);
        this.logger.LogDebug(ApplicationCoreMessages.BasketApplicationService_SetQuantitiesEnd, basketId);
    }

    /// <summary>
    ///  Anonymous ユーザーが登録した買い物かご情報を指定されたユーザーの買い物かご情報に追加登録します。
    /// </summary>
    /// <param name="anonymousId">Anonymous ユーザーのユーザー Id 。</param>
    /// <param name="userId">ログイン後のユーザー Id 。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    /// <remarks>
    ///  <para>
    ///   未ログイン状態で買い物かごを操作した後、ログインした際に呼び出されることを想定しています。
    ///   Anonymous ユーザーの買い物かごが見つからない場合、処理をスキップします。
    ///   ログイン後ユーザーの買い物かごが見つからない場合、買い物かごを新しく作成します。
    ///  </para>
    /// </remarks>
    /// <exception cref="ArgumentException"><paramref name="anonymousId"/> または <paramref name="userId"/> が <see langword="null"/> または空白の場合。</exception>
    public async Task TransferBasket(string anonymousId, string userId)
    {
        this.logger.LogDebug(ApplicationCoreMessages.BasketApplicationService_TransferBasketStart, anonymousId, userId);
        if (string.IsNullOrWhiteSpace(anonymousId))
        {
            throw new ArgumentException(ApplicationCoreMessages.ArgumentIsNullOrWhiteSpace, nameof(anonymousId));
        }

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException(ApplicationCoreMessages.ArgumentIsNullOrWhiteSpace, nameof(userId));
        }

        var anonymousBasket = await this.basketRepository.GetWithBasketItemsAsync(anonymousId);
        if (anonymousBasket is null)
        {
            this.logger.LogDebug(ApplicationCoreMessages.UserBasketNotFound, anonymousId);
            return;
        }

        var userBasket = await this.GetOrCreateBasketForUser(userId);
        foreach (var item in anonymousBasket.Items)
        {
            userBasket.AddItem(item.CatalogItemId, item.UnitPrice, item.Quantity);
        }

        await this.basketRepository.UpdateAsync(userBasket);
        await this.basketRepository.RemoveAsync(anonymousBasket);
        this.logger.LogDebug(ApplicationCoreMessages.BasketApplicationService_TransferBasketEnd, anonymousId, userId);
    }

    /// <summary>
    ///  <paramref name="userId"/> に対応する買い物かご情報を取得します。
    ///  対応する買い物かご情報がない場合は、作成します。
    /// </summary>
    /// <param name="userId">ユーザー Id 。</param>
    /// <returns>買い物かご情報を返す非同期処理を表すタスク。</returns>
    /// <exception cref="ArgumentException"><paramref name="userId"/> が <see langword="null"/> または空白の場合.</exception>
    public async Task<Basket> GetOrCreateBasketForUser(string userId)
    {
        this.logger.LogDebug(ApplicationCoreMessages.BasketApplicationService_GetOrCreateBasketForUserStart, userId);
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException(null, nameof(userId));
        }

        var basket = await this.basketRepository.GetWithBasketItemsAsync(userId);
        if (basket is null)
        {
            this.logger.LogDebug(ApplicationCoreMessages.CreateNewBasket_UserBasketNotFound, userId);
            basket = new Basket(userId);
            return await this.basketRepository.AddAsync(basket);
        }

        this.logger.LogDebug(ApplicationCoreMessages.BasketApplicationService_GetOrCreateBasketForUserEnd, userId);
        return basket;
    }
}
