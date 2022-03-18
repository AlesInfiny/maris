using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Dto.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace Dressca.Web.Controllers;

/// <summary>
///  <see cref="CatalogBrand"/> の情報にアクセスする API コントローラーです。
/// </summary>
[Route("api/catalog-brands")]
[ApiController]
[Produces("application/json")]
public class CatalogBrandsController : ControllerBase
{
    private readonly CatalogApplicationService service;
    private readonly IObjectMapper<CatalogBrand, CatalogBrandDto> mapper;

    /// <summary>
    ///  <see cref="CatalogBrandsController"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="service">カタログアプリケーションサービス。</param>
    /// <param name="mapper"><see cref="CatalogBrand"/> と <see cref="CatalogBrandDto"/> のマッパー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="service"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="mapper"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public CatalogBrandsController(
        CatalogApplicationService service,
        IObjectMapper<CatalogBrand, CatalogBrandDto> mapper)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    ///  カタログブランドの一覧を取得します。
    /// </summary>
    /// <returns>カタログブランドの一覧。</returns>
    /// <response code="200">成功。</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CatalogBrandDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCatalogBrandsAsync()
    {
        var brands = await this.service.GetBrandsAsync();
        return this.Ok(brands
            .Select(brand => this.mapper.Convert(brand))
            .ToArray());
    }
}
