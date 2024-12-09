using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Admin.Dto.CatalogBrands;
using Dressca.Web.Admin.Dto.CatalogCategories;
using Dressca.Web.Admin.Dto.CatalogItems;

namespace Dressca.Web.Admin.Mapper;

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
        services.AddSingleton<IObjectMapper<CatalogCategory, GetCatalogCategoriesResponse>, CatalogCategoryMapper>();
        services.AddSingleton<IObjectMapper<CatalogBrand, GetCatalogBrandsResponse>, CatalogBrandMapper>();
        services.AddSingleton<IObjectMapper<CatalogItem, GetCatalogItemResponse>, CatalogItemMapper>();
        return services;
    }
}
