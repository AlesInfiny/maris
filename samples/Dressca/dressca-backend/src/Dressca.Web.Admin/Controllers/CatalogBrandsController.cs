using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Admin.Dto.CatalogBrands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Dressca.Web.Admin.Controllers;

/// <summary>
///  <see cref="CatalogBrand"/> の情報にアクセスする API コントローラーです。
/// </summary>
[Route("api/catalog-brands")]
[ApiController]
[Produces("application/json")]
[Authorize]
public class CatalogBrandsController : ControllerBase
{
    private readonly CatalogApplicationService service;
    private readonly IObjectMapper<CatalogBrand, GetCatalogBrandsResponse> mapper;

    /// <summary>
    ///  <see cref="CatalogBrandsController"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="service">カタログアプリケーションサービス。</param>
    /// <param name="mapper"><see cref="CatalogBrand"/> と <see cref="GetCatalogBrandsResponse"/> のマッパー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="service"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="mapper"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public CatalogBrandsController(
        CatalogApplicationService service,
        IObjectMapper<CatalogBrand, GetCatalogBrandsResponse> mapper)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    ///  カタログブランドの一覧を取得します。
    /// </summary>
    /// <returns>カタログブランドの一覧。</returns>
    /// <response code="200">成功。</response>
    /// <response code="401">未認証。</response>
    /// <response code="500">サーバーエラー。</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetCatalogBrandsResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [OpenApiOperation("getCatalogBrands")]
    public async Task<IActionResult> GetCatalogBrandsAsync()
    {
        var brands = await this.service.GetBrandsAsync();
        return this.Ok(brands
            .Select(brand => this.mapper.Convert(brand))
            .ToArray());
    }
}
