using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Consumer.Dto.Catalog;

namespace Dressca.Web.Consumer.Mapper;

/// <summary>
///  <see cref="CatalogItem"/> と <see cref="CatalogItemApiModel"/> のマッパーです。
/// </summary>
public class CatalogItemMapper : IObjectMapper<CatalogItem, CatalogItemApiModel>
{
    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(value))]
    public CatalogItemApiModel? Convert(CatalogItem? value)
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
