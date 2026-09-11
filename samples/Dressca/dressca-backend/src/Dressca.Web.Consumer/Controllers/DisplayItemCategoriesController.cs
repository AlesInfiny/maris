using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.DisplayItems;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Consumer.Dto.DisplayItem;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Dressca.Web.Consumer.Controllers;

/// <summary>
///  <see cref="DisplayItemCategory"/> の情報にアクセスする API コントローラーです。
/// </summary>
[Route("api/display-item-categories")]
[ApiController]
[Produces("application/json")]
public class DisplayItemCategoriesController : ControllerBase
{
    private readonly DisplayItemApplicationService service;
    private readonly IObjectMapper<DisplayItemCategory, GetDisplayItemCategoriesResponse> mapper;

    /// <summary>
    ///  <see cref="DisplayItemCategoriesController"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="service">陳列品アプリケーションサービス。</param>
    /// <param name="mapper"><see cref="DisplayItemCategory"/> と <see cref="GetDisplayItemCategoriesResponse"/> のマッパー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="service"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="mapper"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public DisplayItemCategoriesController(
        DisplayItemApplicationService service,
        IObjectMapper<DisplayItemCategory, GetDisplayItemCategoriesResponse> mapper)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    ///  陳列品カテゴリの一覧を取得します。
    /// </summary>
    /// <returns>陳列品カテゴリの一覧。</returns>
    /// <response code="200">成功。</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetDisplayItemCategoriesResponse>), StatusCodes.Status200OK)]
    [OpenApiOperation("getDisplayItemCategories")]
    public async Task<IActionResult> GetDisplayItemCategoriesAsync()
    {
        var categories = await this.service.GetCategoriesAsync();
        var returnValues = categories
            .Select(category => this.mapper.Convert(category))
            .ToArray();
        return this.Ok(returnValues);
    }
}
