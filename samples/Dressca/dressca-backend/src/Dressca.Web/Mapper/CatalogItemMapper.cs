using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Dto.Catalog;

namespace Dressca.Web.Mapper;

/// <summary>
///  <see cref="CatalogItem"/> と <see cref="CatalogItemDto"/> のマッパーです。
/// </summary>
public class CatalogItemMapper : IObjectMapper<CatalogItem, CatalogItemDto>
{
    /// <inheritdoc/>
    public CatalogItem? Convert(CatalogItemDto? value)
    {
        if (value is null)
        {
            return null;
        }

        return new(
            catalogCategoryId: value.CatalogCategoryId,
            catalogBrandId: value.CatalogBrandId,
            description: value.Description,
            name: value.Name,
            price: value.Price,
            productCode: value.ProductCode)
        { Id = value.Id };
    }

    /// <inheritdoc/>
    public CatalogItemDto? Convert(CatalogItem? value)
    {
        if (value is null)
        {
            return null;
        }

        return new()
        {
            CatalogBrandId = value.CatalogBrandId,
            CatalogCategoryId = value.CatalogCategoryId,
            Description = value.Description,
            Id = value.Id,
            Name = value.Name,
            Price = value.Price,
            ProductCode = value.ProductCode,
        };
    }
}
