using Dressca.ApplicationCore.Resources;
using Microsoft.Extensions.Logging;

namespace Dressca.ApplicationCore.Baskets;

public class BasketDomainService : IBasketDomainService
{
    private readonly IBasketRepository basketRepository;
    private readonly ILogger<BasketDomainService> logger;

    public BasketDomainService(IBasketRepository basketRepository, ILogger<BasketDomainService> logger)
    {
        this.basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task AddItemToBasketAsync(long basketId, long catalogItemId, decimal price, int quantity = 1, CancellationToken cancellationToken = default)
    {
        var basket = await this.basketRepository.GetAsync(basketId, cancellationToken);
        if (basket is null)
        {
            throw new BasketNotFoundException(basketId);
        }

        basket.AddItem(catalogItemId, price, quantity);
        basket.RemoveEmptyItems();
        await this.basketRepository.UpdateAsync(basket, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task DeleteBasketAsync(long basketId, CancellationToken cancellationToken = default)
    {
        var basket = await this.basketRepository.GetWithBasketItemsAsync(basketId, cancellationToken);
        if (basket is null)
        {
            throw new BasketNotFoundException(basketId);
        }

        await this.basketRepository.RemoveAsync(basket, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SetQuantitiesAsync(long basketId, Dictionary<long, int> quantities, CancellationToken cancellationToken = default)
    {
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
    }

    /// <inheritdoc/>
    public async Task<Basket> GetOrCreateBasketForUserAsync(string buyerId, CancellationToken cancellationToken = default)
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
