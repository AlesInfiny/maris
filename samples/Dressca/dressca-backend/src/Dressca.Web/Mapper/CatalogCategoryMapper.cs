using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Dto.Catalog;

namespace Dressca.Web.Mapper;

/// <summary>
///  <see cref="CatalogCategory"/> と <see cref="CatalogCategoryDto"/> のマッパーです。
/// </summary>
public class CatalogCategoryMapper : IObjectMapper<CatalogCategory, CatalogCategoryDto>
{
    /// <inheritdoc/>
    public CatalogCategory? Convert(CatalogCategoryDto? value)
    {
        if (value is null)
        {
            return null;
        }

        return new(value.Name) { Id = value.Id };
    }

    /// <inheritdoc/>
    public CatalogCategoryDto? Convert(CatalogCategory? value)
    {
        if (value is null)
        {
            return null;
        }

        return new() { Id = value.Id, Name = value.Name };
    }
}
