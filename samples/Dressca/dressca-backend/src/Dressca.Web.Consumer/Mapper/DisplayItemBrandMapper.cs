using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.DisplayItems;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Consumer.Dto.DisplayItem;

namespace Dressca.Web.Consumer.Mapper;

/// <summary>
///  <see cref="DisplayItemBrand"/> と <see cref="GetDisplayItemBrandsResponse"/> のマッパーです。
/// </summary>
public class DisplayItemBrandMapper : IObjectMapper<DisplayItemBrand, GetDisplayItemBrandsResponse>
{
    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(value))]
    public GetDisplayItemBrandsResponse? Convert(DisplayItemBrand? value)
    {
        if (value is null)
        {
            return null;
        }

        return new() { Id = value.Id, Name = value.Name };
    }
}
