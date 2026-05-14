using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Baskets;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Consumer.Dto.Baskets;

namespace Dressca.Web.Consumer.Mapper;

/// <summary>
///  <see cref="BasketItem"/> と <see cref="BasketItemApiModel"/> のマッパーです。
/// </summary>
public class BasketItemMapper : IObjectMapper<BasketItem, BasketItemApiModel>
{
    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(value))]
    public BasketItemApiModel? Convert(BasketItem? value)
    {
        if (value is null)
        {
            return null;
        }

        return new BasketItemApiModel
        {
            CatalogItemId = value.CatalogItemId,
            Quantity = value.Quantity,
            UnitPrice = value.UnitPrice,
            SubTotal = value.GetSubTotal(),
        };
    }
}
