using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.DisplayItems;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Consumer.Dto.DisplayItem;

namespace Dressca.Web.Consumer.Mapper;

/// <summary>
///  <see cref="DisplayItemCategory"/> と <see cref="GetDisplayItemCategoriesResponse"/> のマッパーです。
/// </summary>
public class DisplayItemCategoryMapper : IObjectMapper<DisplayItemCategory, GetDisplayItemCategoriesResponse>
{
    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(value))]
    public GetDisplayItemCategoriesResponse? Convert(DisplayItemCategory? value)
    {
        if (value is null)
        {
            return null;
        }

        return new() { Id = value.Id, Name = value.Name };
    }
}
