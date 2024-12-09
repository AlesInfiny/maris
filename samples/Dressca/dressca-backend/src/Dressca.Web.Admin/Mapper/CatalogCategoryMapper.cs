using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Admin.Dto.Catalog;

namespace Dressca.Web.Admin.Mapper;

/// <summary>
///  <see cref="CatalogCategory"/> と <see cref="CatalogCategoryResponse"/> のマッパーです。
/// </summary>
public class CatalogCategoryMapper : IObjectMapper<CatalogCategory, CatalogCategoryResponse>
{
    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(value))]
    public CatalogCategoryResponse? Convert(CatalogCategory? value)
    {
        if (value is null)
        {
            return null;
        }

        return new() { Id = value.Id, Name = value.Name };
    }
}
