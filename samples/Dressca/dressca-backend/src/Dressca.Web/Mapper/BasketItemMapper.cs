using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Baskets;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Dto.Baskets;

namespace Dressca.Web.Mapper;

/// <summary>
///  <see cref="BasketItem"/> と <see cref="BasketItemDto"/> のマッパーです。
/// </summary>
public class BasketItemMapper : IObjectMapper<BasketItem, BasketItemDto>
{
    /// <inheritdoc/>
    [return: NotNullIfNotNull("value")]
    public BasketItem? Convert(BasketItemDto? value)
    {
        if (value is null)
        {
            return null;
        }

        return new BasketItem(value.CatalogItemId, value.UnitPrice, value.Quantity) { Id = value.Id };
    }

    /// <inheritdoc/>
    [return: NotNullIfNotNull("value")]
    public BasketItemDto? Convert(BasketItem? value)
    {
        if (value is null)
        {
            return null;
        }

        return new BasketItemDto
        {
            Id = value.Id,
            CatalogItemId = value.CatalogItemId,
            Quantity = value.Quantity,
            UnitPrice = value.UnitPrice,
        };
    }
}
