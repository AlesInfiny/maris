using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.Catalog;

namespace Dressca.ApplicationCore.Ordering;

public class OrderFactory
{
    public Order CreateOrder(Basket basket, IReadOnlyList<CatalogItem> catalogItems, ShipTo shipToAddress)
    {
        var orderItems = basket.Items.Select(
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

        return new Order(basket.BuyerId, shipToAddress, orderItems);
    }
}
