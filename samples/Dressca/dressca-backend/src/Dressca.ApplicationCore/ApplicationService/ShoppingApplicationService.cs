using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.DisplayItems;
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
    private readonly IDisplayItemRepository displayItemRepository;
    private readonly IDisplayItemDomainService displayItemDomainService;
    private readonly ILogger<ShoppingApplicationService> logger;

    /// <summary>
    ///  <see cref="ShoppingApplicationService"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="basketRepository">買い物かごリポジトリ。</param>
    /// <param name="orderRepository">注文リポジトリ。</param>
    /// <param name="orderFactory">注文エンティティファクトリー。</param>
    /// <param name="displayItemRepository">陳列品リポジトリ。</param>
    /// <param name="displayItemDomainService">陳列品ドメインサービス。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="basketRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="orderRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="orderFactory"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="displayItemRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="displayItemDomainService"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>/// </exception>
    public ShoppingApplicationService(
        IBasketRepository basketRepository,
        IOrderRepository orderRepository,
        IOrderFactory orderFactory,
        IDisplayItemRepository displayItemRepository,
        IDisplayItemDomainService displayItemDomainService,
        ILogger<ShoppingApplicationService> logger)
    {
        this.basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        this.orderFactory = orderFactory ?? throw new ArgumentNullException(nameof(orderFactory));
        this.displayItemRepository = displayItemRepository ?? throw new ArgumentNullException(nameof(displayItemRepository));
        this.displayItemDomainService = displayItemDomainService ?? throw new ArgumentNullException(nameof(displayItemDomainService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///  買い物かごアイテムの一覧を取得します。
    /// </summary>
    /// <param name="buyerId">購入者 Id 。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>
    ///  BasketResult : <paramref name="buyerId"/> に対応する買い物かご。
    ///  DisplayItems : 買い物かごアイテムの一覧。
    ///  DeletedItemIds : 削除済みの陳列品 Id のリスト。
    /// </returns>
    public async Task<(Basket BasketResult, IReadOnlyList<DisplayItem> DisplayItems, IReadOnlyList<Guid> DeletedItemIds)> GetBasketItemsAsync(string buyerId, CancellationToken cancellationToken = default)
    {
        this.logger.LogDebug(Events.DebugEvent, LogMessages.ShoppingApplicationService_GetBasketItemsAsyncStart, buyerId);

        Basket basket;
        IReadOnlyList<DisplayItem> displayItems;
        using (var scope = TransactionScopeManager.CreateTransactionScope())
        {
            basket = await this.GetOrCreateBasketForUserAsync(buyerId, cancellationToken);
            var displayItemIds = basket.Items.Select(basketItem => basketItem.DisplayItemId).ToList();
            displayItems = await this.displayItemRepository.FindAsync(displayItem => displayItemIds.Contains(displayItem.Id), cancellationToken);
            scope.Complete();
        }

        var deletedDisplayItemIds = displayItems.Where(item => item.IsDeleted == true).Select(item => item.Id).ToList();

        this.logger.LogDebug(Events.DebugEvent, LogMessages.ShoppingApplicationService_GetBasketItemsAsyncEnd, buyerId);
        return (BasketResult: basket, DisplayItems: displayItems, DeletedItemIds: deletedDisplayItemIds);
    }

    /// <summary>
    ///  買い物かごの各アイテムの数量を一括で設定します。
    /// </summary>
    /// <param name="buyerId">購入者 Id 。</param>
    /// <param name="quantities">各陳列品の数量。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>処理結果を返す非同期処理を表すタスク。</returns>
    public async Task SetBasketItemsQuantitiesAsync(string buyerId, Dictionary<Guid, int> quantities, CancellationToken cancellationToken = default)
    {
        this.logger.LogDebug(Events.DebugEvent, LogMessages.ShoppingApplicationService_SetBasketItemsQuantitiesAsyncStart, buyerId);

        using (var scope = TransactionScopeManager.CreateTransactionScope())
        {
            var basket = await this.GetOrCreateBasketForUserAsync(buyerId, cancellationToken);

            // 買い物かごに入っていない陳列品が指定されていないか確認
            var notExistsInBasketDisplayItemIds = quantities.Keys.Where(displayItemId => !basket.IsInDisplayItem(displayItemId));
            if (notExistsInBasketDisplayItemIds.Any())
            {
                throw new DisplayItemNotExistingInBasketException(notExistsInBasketDisplayItemIds);
            }

            // 陳列品リポジトリに存在しない陳列品が指定されていないか確認
            var (existsAll, existingDisplayItems) = await this.displayItemDomainService.ExistsAllAsync(quantities.Keys, cancellationToken);
            if (!existsAll)
            {
                var notExistingInRepositoryDisplayItemIds =
                    quantities.Keys
                       .Where(displayItemId => !existingDisplayItems.Any(item => item.Id == displayItemId));
                throw new DisplayItemNotExistingInRepositoryException(notExistingInRepositoryDisplayItemIds);
            }

            basket.SetItemsQuantity(quantities);
            var currentBasketItems = basket.Items.Select(i => string.Format(Messages.Basket_ItemQuantity, i.DisplayItemId, i.Quantity));
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
    /// <param name="displayItemId">陳列品 Id 。</param>
    /// <param name="addedQuantity">数量。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>処理結果を返す非同期処理を表すタスク。</returns>
    public async Task AddItemToBasketAsync(string buyerId, Guid displayItemId, int addedQuantity, CancellationToken cancellationToken = default)
    {
        this.logger.LogDebug(Events.DebugEvent, LogMessages.ShoppingApplicationService_AddItemToBasketAsyncStart, buyerId, displayItemId, addedQuantity);

        using (var scope = TransactionScopeManager.CreateTransactionScope())
        {
            var basket = await this.GetOrCreateBasketForUserAsync(buyerId, cancellationToken);

            // 陳列品リポジトリに存在しない陳列品が指定されていないか確認
            var (existsAll, displayItems) = await this.displayItemDomainService.ExistsAllAsync([displayItemId], cancellationToken);
            if (!existsAll)
            {
                List<Guid> notExistingInRepositoryDisplayItemIds = [displayItemId];
                throw new DisplayItemNotExistingInRepositoryException(notExistingInRepositoryDisplayItemIds);
            }

            var displayItem = displayItems[0];
            basket.AddItem(displayItemId, displayItem.Price, addedQuantity);
            basket.RemoveEmptyItems();
            await this.basketRepository.UpdateAsync(basket, cancellationToken);
            scope.Complete();
        }

        this.logger.LogDebug(Events.DebugEvent, LogMessages.ShoppingApplicationService_AddItemToBasketAsyncEnd, buyerId, displayItemId, addedQuantity);
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

            var displayItemIds = checkoutBasket.Items.Select(item => item.DisplayItemId).ToArray();
            var displayItems =
                await this.displayItemRepository.FindAsync(item => displayItemIds.Contains(item.Id) && !item.IsDeleted, cancellationToken);
            var missingIds = displayItemIds.Except(displayItems.Select(item => item.Id)).ToArray();
            if (missingIds.Length != 0)
            {
                throw new DisplayItemNotExistingInRepositoryException(missingIds);
            }

            var order = this.orderFactory.CreateOrder(checkoutBasket, displayItems, shipToAddress);
            ordered = await this.orderRepository.AddAsync(order, cancellationToken);

            // 買い物かごを削除
            await this.basketRepository.RemoveAsync(checkoutBasket, cancellationToken);
            scope.Complete();
        }

        this.logger.LogDebug(Events.DebugEvent, LogMessages.ShoppingApplicationService_CheckoutAsyncEnd, checkoutBasket.Id, ordered.Id);
        return ordered;
    }

    /// <summary>削除済みの陳列品を含め、指定した商品を買い物かごから削除します。</summary>
    /// <param name="buyerId">購入者 ID。</param>
    /// <param name="displayItemId">陳列品 ID。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>削除処理を表すタスク。</returns>
    public async Task RemoveItemFromBasketAsync(string buyerId, Guid displayItemId, CancellationToken cancellationToken = default)
    {
        using var scope = TransactionScopeManager.CreateTransactionScope();
        var basket = await this.GetOrCreateBasketForUserAsync(buyerId, cancellationToken);
        var items = await this.displayItemRepository.FindAsync(item => item.Id == displayItemId, cancellationToken);
        if (items.Count == 0)
        {
            throw new DisplayItemNotExistingInRepositoryException([displayItemId]);
        }

        if (!basket.IsInDisplayItem(displayItemId))
        {
            throw new DisplayItemNotExistingInBasketException([displayItemId]);
        }

        basket.SetItemsQuantity(new() { [displayItemId] = 0 });
        basket.RemoveEmptyItems();
        await this.basketRepository.UpdateAsync(basket, cancellationToken);
        scope.Complete();
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
            basket = new Basket { Id = Guid.CreateVersion7(), BuyerId = buyerId };
            return await this.basketRepository.AddAsync(basket, cancellationToken);
        }

        this.logger.LogDebug(Events.DebugEvent, LogMessages.ShoppingApplicationService_GetOrCreateBasketForUserAsyncEnd, buyerId);
        return basket;
    }
}
