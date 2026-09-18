using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.DisplayItems;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Consumer.Dto.DisplayItem;

namespace Dressca.Web.Consumer.Mapper;

/// <summary>
///  <see cref="DisplayItem"/> と <see cref="DisplayItemSummaryApiModel"/> のマッパーです。
/// </summary>
public class DisplayItemSummaryApiModelMapper : IObjectMapper<DisplayItem, DisplayItemSummaryApiModel>
{
    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(value))]
    public DisplayItemSummaryApiModel? Convert(DisplayItem? value)
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
