using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Baskets;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Dto.Baskets;

namespace Dressca.Web.Mapper;

/// <summary>
///  <see cref="BasketItem"/> と <see cref="BasketItemResponse"/> のマッパーです。
/// </summary>
public class BasketItemMapper : IObjectMapper<BasketItem, BasketItemResponse>
{
    /// <inheritdoc/>
    [return: NotNullIfNotNull("value")]
    public BasketItemResponse? Convert(BasketItem? value)
    {
        if (value is null)
        {
            return null;
        }

        return new BasketItemResponse
        {
            CatalogItemId = value.CatalogItemId,
            Quantity = value.Quantity,
            UnitPrice = value.UnitPrice,
            SubTotal = value.GetSubTotal(),
        };
    }
}
