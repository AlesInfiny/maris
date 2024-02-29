using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Dto.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace Dressca.Web.Controllers;

/// <summary>
///  <see cref="CatalogCategory"/> の情報にアクセスする API コントローラーです。
/// </summary>
[Route("api/catalog-categories")]
[ApiController]
[Produces("application/json")]
public class CatalogCategoriesController : ControllerBase
{
    private readonly CatalogApplicationService service;
    private readonly IObjectMapper<CatalogCategory, CatalogCategoryResponse> mapper;

    /// <summary>
    ///  <see cref="CatalogCategoriesController"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="service">カタログアプリケーションサービス。</param>
    /// <param name="mapper"><see cref="CatalogCategory"/> と <see cref="CatalogCategoryResponse"/> のマッパー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="service"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="mapper"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public CatalogCategoriesController(
        CatalogApplicationService service,
        IObjectMapper<CatalogCategory, CatalogCategoryResponse> mapper)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    ///  カタログカテゴリの一覧を取得します。
    /// </summary>
    /// <returns>カタログカテゴリの一覧。</returns>
    /// <response code="200">成功。</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CatalogCategoryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCatalogCategoriesAsync()
    {
        var categories = await this.service.GetCategoriesAsync();
        var returnValues = categories
            .Select(category => this.mapper.Convert(category))
            .ToArray();
        return this.Ok(returnValues);
    }
}
