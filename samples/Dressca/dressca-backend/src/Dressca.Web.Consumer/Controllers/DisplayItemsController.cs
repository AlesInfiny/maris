using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.DisplayItems;
using Dressca.SystemCommon;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Consumer.Controllers.ApiModel;
using Dressca.Web.Consumer.Dto.DisplayItem;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Dressca.Web.Consumer.Controllers;

/// <summary>
///  <see cref="DisplayItem"/> の情報にアクセスする API コントローラーです。
/// </summary>
[Route("api/display-items")]
[ApiController]
[Produces("application/json")]
public class DisplayItemsController : ControllerBase
{
    private readonly DisplayItemApplicationService service;
    private readonly IObjectMapper<DisplayItem, GetDisplayItemResponse> mapper;

    /// <summary>
    ///  <see cref="DisplayItemsController"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="service">陳列品アプリケーションサービス。</param>
    /// <param name="mapper"><see cref="DisplayItem"/> と <see cref="GetDisplayItemResponse"/> のマッパー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="service"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="mapper"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public DisplayItemsController(
        DisplayItemApplicationService service,
        IObjectMapper<DisplayItem, GetDisplayItemResponse> mapper)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    ///  陳列品を検索して返します。
    /// </summary>
    /// <returns>陳列品の一覧。</returns>
    /// <param name="query">検索クエリ。</param>
    /// <response code="200">成功。</response>
    /// <response code="400">リクエストエラー。</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedList<GetDisplayItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [OpenApiOperation("getByQuery")]
    public async Task<IActionResult> GetByQueryAsync([FromQuery] FindDisplayItemsQuery query)
    {
        var (displayItems, totalCount) =
            await this.service.GetDisplayItemsAsync(
                skip: query.GetSkipCount(),
                take: query.PageSize,
                brandId: query.BrandId,
                categoryId: query.CategoryId);
        var items = displayItems
            .Select(displayItem => this.mapper.Convert(displayItem))
            .ToList();
        var returnValue = new PagedList<GetDisplayItemResponse>(
            items: items,
            totalCount: totalCount,
            page: query.Page,
            pageSize: query.PageSize);
        return this.Ok(returnValue);
    }
}
