using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Admin.Dto.Catalog;

namespace Dressca.Web.Admin.Mapper;

/// <summary>
///  <see cref="CatalogItem"/> と <see cref="CatalogItemResponse"/> のマッパーです。
/// </summary>
public class CatalogItemMapper : IObjectMapper<CatalogItem, CatalogItemResponse>
{
    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(value))]
    public CatalogItemResponse? Convert(CatalogItem? value)
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
            AssetCodes = value.Assets.Select(asset => asset.AssetCode).ToList(),
        };
    }
}
