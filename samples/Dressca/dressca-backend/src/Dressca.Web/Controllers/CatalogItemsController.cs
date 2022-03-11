using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Controllers.ApiModel;
using Dressca.Web.Dto;
using Dressca.Web.Dto.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace Dressca.Web.Controllers;

/// <summary>
///  <see cref="CatalogItem"/> の情報にアクセスする API コントローラーです。
/// </summary>
[Route("api/catalog-items")]
[ApiController]
[Produces("application/json")]
public class CatalogItemsController : ControllerBase
{
    private readonly CatalogApplicationService service;
    private readonly IObjectMapper<CatalogItem, CatalogItemDto> mapper;

    /// <summary>
    ///  <see cref="CatalogItemsController"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="service">カタログアプリケーションサービス。</param>
    /// <param name="mapper"><see cref="CatalogItem"/> と <see cref="CatalogItemDto"/> のマッパー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="service"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="mapper"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public CatalogItemsController(
        CatalogApplicationService service,
        IObjectMapper<CatalogItem, CatalogItemDto> mapper)
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
    [ProducesResponseType(typeof(PagedList<CatalogItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Find([FromQuery] FindCatalogItemsQuery query)
    {
        var (catalogItems, totalCount) =
            await this.service.GetCatalogAsync(
                skip: query.GetSkipCount(),
                take: query.PageSize,
                brandId: query.BrandId,
                categoryId: query.CategoryId);
        var items = catalogItems
            .Select(catalogItem => this.mapper.Convert(catalogItem))
            .ToList();
        var returnValue = new PagedList<CatalogItemDto>(
            items: items,
            totalCount: totalCount,
            page: query.Page,
            pageSize: query.PageSize);
        return this.Ok(returnValue);
    }
}
