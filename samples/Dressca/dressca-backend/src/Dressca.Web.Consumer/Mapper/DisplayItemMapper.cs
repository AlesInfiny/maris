using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.DisplayItems;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Consumer.Dto.DisplayItem;

namespace Dressca.Web.Consumer.Mapper;

/// <summary>
///  <see cref="DisplayItem"/> と <see cref="GetDisplayItemResponse"/> のマッパーです。
/// </summary>
public class DisplayItemMapper : IObjectMapper<DisplayItem, GetDisplayItemResponse>
{
    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(value))]
    public GetDisplayItemResponse? Convert(DisplayItem? value)
    {
        if (value is null)
        {
            return null;
        }

        return new()
        {
            DisplayItemBrandId = value.DisplayItemBrandId,
            DisplayItemCategoryId = value.DisplayItemCategoryId,
            Description = value.Description,
            Id = value.Id,
            Name = value.Name,
            Price = value.Price,
            ProductCode = value.ProductCode,
            AssetCodes = value.Assets.Select(asset => asset.AssetCode).ToList(),
        };
    }
}
