using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.Catalog;

namespace Dressca.ApplicationCore.Ordering;

/// <summary>
///  注文エンティティファクトリーの実装です。
/// </summary>
internal class OrderFactory : IOrderFactory
{
    /// <inheritdoc/>
    public Order CreateOrder(Basket basket, IReadOnlyList<CatalogItem> catalogItems, ShipTo shipToAddress)
    {
        basket.ThrowIfNull();
        catalogItems.ThrowIfNull();
        shipToAddress.ThrowIfNull();

        var orderItems = basket.Items.Select(
            basketItem =>
            {
                var catalogItem = catalogItems.First(c => c.Id == basketItem.CatalogItemId);
                var itemOrdered = new CatalogItemOrdered(catalogItem.Id, catalogItem.Name, catalogItem.ProductCode);
                var orderItem = new OrderItem { Id = Guid.CreateVersion7(), ItemOrdered = itemOrdered, UnitPrice = basketItem.UnitPrice, Quantity = basketItem.Quantity };
                var orderItemAssets = catalogItem.Assets
                    .Select(catalogItemAsset => new OrderItemAsset { Id = Guid.CreateVersion7(), AssetCode = catalogItemAsset.AssetCode, OrderItemId = orderItem.Id });
                orderItem.AddAssets(orderItemAssets);
                return orderItem;
            }).ToList();

        return new Order(orderItems) { Id = Guid.CreateVersion7(), BuyerId = basket.BuyerId, ShipToAddress = shipToAddress };
    }
}
