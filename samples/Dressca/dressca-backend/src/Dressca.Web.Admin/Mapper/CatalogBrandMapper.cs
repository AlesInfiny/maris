using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Admin.Dto.CatalogBrands;

namespace Dressca.Web.Admin.Mapper;

/// <summary>
///  <see cref="CatalogBrand"/> と <see cref="GetCatalogBrandsResponse"/> のマッパーです。
/// </summary>
public class CatalogBrandMapper : IObjectMapper<CatalogBrand, GetCatalogBrandsResponse>
{
    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(value))]
    public GetCatalogBrandsResponse? Convert(CatalogBrand? value)
    {
        if (value is null)
        {
            return null;
        }

        return new() { Id = value.Id, Name = value.Name };
    }
}
