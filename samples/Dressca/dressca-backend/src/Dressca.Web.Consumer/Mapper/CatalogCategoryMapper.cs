using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Consumer.Dto.Catalog;

namespace Dressca.Web.Consumer.Mapper;

/// <summary>
///  <see cref="CatalogCategory"/> と <see cref="GetCatalogCategoriesResponse"/> のマッパーです。
/// </summary>
public class CatalogCategoryMapper : IObjectMapper<CatalogCategory, GetCatalogCategoriesResponse>
{
    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(value))]
    public GetCatalogCategoriesResponse? Convert(CatalogCategory? value)
    {
        if (value is null)
        {
            return null;
        }

        return new() { Id = value.Id, Name = value.Name };
    }
}
