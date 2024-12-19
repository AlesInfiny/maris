using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Consumer.Controllers.ApiModel;
using Dressca.Web.Consumer.Dto.Catalog;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Dressca.Web.Consumer.Controllers;

/// <summary>
///  <see cref="CatalogItem"/> の情報にアクセスする API コントローラーです。
/// </summary>
[Route("api/catalog-items")]
[ApiController]
[Produces("application/json")]
public class CatalogItemsController : ControllerBase
{
    private readonly CatalogApplicationService service;
    private readonly IObjectMapper<CatalogItem, CatalogItemResponse> mapper;

    /// <summary>
    ///  <see cref="CatalogItemsController"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="service">カタログアプリケーションサービス。</param>
    /// <param name="mapper"><see cref="CatalogItem"/> と <see cref="CatalogItemResponse"/> のマッパー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="service"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="mapper"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public CatalogItemsController(
        CatalogApplicationService service,
        IObjectMapper<CatalogItem, CatalogItemResponse> mapper)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    ///  カタログアイテムを検索して返します。
    /// </summary>
    /// <returns>カタログアイテムの一覧。</returns>
    /// <param name="query">検索クエリ。</param>
    /// <response code="200">成功。</response>
    /// <response code="400">リクエストエラー。</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedList<CatalogItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [OpenApiOperation("getByQuery")]
    public async Task<IActionResult> GetByQueryAsync([FromQuery] FindCatalogItemsQuery query)
    {
        var (catalogItems, totalCount) =
            await this.service.GetCatalogItemsAsync(
                skip: query.GetSkipCount(),
                take: query.PageSize,
                brandId: query.BrandId,
                categoryId: query.CategoryId);
        var items = catalogItems
            .Select(catalogItem => this.mapper.Convert(catalogItem))
            .ToList();
        var returnValue = new PagedList<CatalogItemResponse>(
            items: items,
            totalCount: totalCount,
            page: query.Page,
            pageSize: query.PageSize);
        return this.Ok(returnValue);
    }
}
