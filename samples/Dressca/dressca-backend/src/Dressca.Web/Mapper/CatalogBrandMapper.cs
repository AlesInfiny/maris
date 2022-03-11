using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Dto.Catalog;

namespace Dressca.Web.Mapper;

/// <summary>
///  <see cref="CatalogBrand"/> と <see cref="CatalogBrandDto"/> のマッパーです。
/// </summary>
public class CatalogBrandMapper : IObjectMapper<CatalogBrand, CatalogBrandDto>
{
    /// <inheritdoc/>
    public CatalogBrand? Convert(CatalogBrandDto? value)
    {
        if (value is null)
        {
            return null;
        }

        return new(value.Name) { Id = value.Id };
    }

    /// <inheritdoc/>
    public CatalogBrandDto? Convert(CatalogBrand? value)
    {
        if (value is null)
        {
            return null;
        }

        return new() { Id = value.Id, Name = value.Name };
    }
}
