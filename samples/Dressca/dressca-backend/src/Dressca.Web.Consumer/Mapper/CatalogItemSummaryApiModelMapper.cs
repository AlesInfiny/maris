using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Consumer.Dto.Catalog;

namespace Dressca.Web.Consumer.Mapper;

/// <summary>
///  <see cref="CatalogItem"/> と <see cref="CatalogItemSummaryApiModel"/> のマッパーです。
/// </summary>
public class CatalogItemSummaryApiModelMapper : IObjectMapper<CatalogItem, CatalogItemSummaryApiModel>
{
    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(value))]
    public CatalogItemSummaryApiModel? Convert(CatalogItem? value)
    {
        if (value is null)
        {
            return null;
        }

        return new()
        {
            Id = value.Id,
            Name = value.Name,
            ProductCode = value.ProductCode,
            AssetCodes = value.Assets.Select(asset => asset.AssetCode).ToList(),
        };
    }
}
