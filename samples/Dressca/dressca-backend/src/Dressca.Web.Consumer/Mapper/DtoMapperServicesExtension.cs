using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.DisplayItems;
using Dressca.ApplicationCore.Ordering;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Consumer.Dto.Baskets;
using Dressca.Web.Consumer.Dto.DisplayItem;
using Dressca.Web.Consumer.Dto.Ordering;

namespace Dressca.Web.Consumer.Mapper;

/// <summary>
///  DTO の Mapper 関連の <see cref="IServiceCollection"/> 拡張メソッドを提供します。
/// </summary>
public static class DtoMapperServicesExtension
{
    /// <summary>
    ///  DTO と エンティティの相互変換を行う
    ///  <see cref="IObjectMapper{T1, T2}"/> オブジェクトを登録します。
    /// </summary>
    /// <param name="services">サービスコレクション。</param>
    /// <returns>構築済みのサービスコレクション。</returns>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="services"/> が <see langword="null"/> です。
    /// </exception>
    public static IServiceCollection AddDresscaDtoMapper(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.AddSingleton<IObjectMapper<DisplayItemCategory, GetDisplayItemCategoriesResponse>, DisplayItemCategoryMapper>();
        services.AddSingleton<IObjectMapper<DisplayItemBrand, GetDisplayItemBrandsResponse>, DisplayItemBrandMapper>();
        services.AddSingleton<IObjectMapper<DisplayItem, GetDisplayItemResponse>, DisplayItemMapper>();
        services.AddSingleton<IObjectMapper<DisplayItem, DisplayItemSummaryApiModel>, DisplayItemSummaryApiModelMapper>();
        services.AddSingleton<IObjectMapper<BasketItem, BasketItemApiModel>, BasketItemMapper>();
        services.AddSingleton<IObjectMapper<Basket, GetBasketItemsResponse>, BasketMapper>();
        services.AddSingleton<IObjectMapper<Order, GetOrderByIdResponse>, OrderMapper>();
        return services;
    }
}
