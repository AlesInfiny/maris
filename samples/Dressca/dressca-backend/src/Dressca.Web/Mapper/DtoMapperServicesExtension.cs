using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.Catalog;
using Dressca.ApplicationCore.Ordering;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Dto.Baskets;
using Dressca.Web.Dto.Catalog;
using Dressca.Web.Dto.Ordering;

namespace Dressca.Web.Mapper;

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
        services.AddSingleton<IObjectMapper<CatalogCategory, CatalogCategoryDto>, CatalogCategoryMapper>();
        services.AddSingleton<IObjectMapper<CatalogBrand, CatalogBrandDto>, CatalogBrandMapper>();
        services.AddSingleton<IObjectMapper<CatalogItem, CatalogItemDto>, CatalogItemMapper>();
        services.AddSingleton<IObjectMapper<BasketItem, BasketItemDto>, BasketItemMapper>();
        services.AddSingleton<IObjectMapper<Basket, BasketResponse>, BasketMapper>();
        services.AddSingleton<IObjectMapper<Order, OrderDto>, OrderMapper>();
        return services;
    }
}
