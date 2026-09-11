using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.DisplayItems;

namespace Dressca.ApplicationCore.Ordering;

/// <summary>
///  注文エンティティファクトリーの実装です。
/// </summary>
internal class OrderFactory : IOrderFactory
{
    /// <inheritdoc/>
    public Order CreateOrder(Basket basket, IReadOnlyList<DisplayItem> displayItems, ShipTo shipToAddress)
    {
        basket.ThrowIfNull();
        displayItems.ThrowIfNull();
        shipToAddress.ThrowIfNull();

        var orderItems = basket.Items.Select(
            basketItem =>
            {
                var displayItem = displayItems.First(c => c.Id == basketItem.DisplayItemId);
                var itemOrdered = new DisplayItemOrdered(displayItem.Id, displayItem.Name, displayItem.ProductCode);
                var orderItem = new OrderItem { Id = Guid.CreateVersion7(), ItemOrdered = itemOrdered, UnitPrice = basketItem.UnitPrice, Quantity = basketItem.Quantity };
                var orderItemAssets = displayItem.Assets
                    .Select(displayItemAsset => new OrderItemAsset { Id = Guid.CreateVersion7(), AssetCode = displayItemAsset.AssetCode, OrderItemId = orderItem.Id });
                orderItem.AddAssets(orderItemAssets);
                return orderItem;
            }).ToList();

        return new Order(orderItems) { Id = Guid.CreateVersion7(), BuyerId = basket.BuyerId, ShipToAddress = shipToAddress };
    }
}
