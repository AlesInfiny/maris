using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.Catalog;

namespace Dressca.ApplicationCore.Ordering;

public interface IOrderFactory
{
    Order CreateOrder(Basket basket, IReadOnlyList<CatalogItem> catalogItems, ShipTo shipToAddress);
}
