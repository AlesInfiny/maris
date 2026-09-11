using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.DisplayItems;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Consumer.Dto.DisplayItem;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Dressca.Web.Consumer.Controllers;

/// <summary>
///  <see cref="DisplayItemBrand"/> の情報にアクセスする API コントローラーです。
/// </summary>
[Route("api/display-item-brands")]
[ApiController]
[Produces("application/json")]
public class DisplayItemBrandsController : ControllerBase
{
    private readonly DisplayItemApplicationService service;
    private readonly IObjectMapper<DisplayItemBrand, GetDisplayItemBrandsResponse> mapper;

    /// <summary>
    ///  <see cref="DisplayItemBrandsController"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="service">陳列品アプリケーションサービス。</param>
    /// <param name="mapper"><see cref="DisplayItemBrand"/> と <see cref="GetDisplayItemBrandsResponse"/> のマッパー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="service"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="mapper"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public DisplayItemBrandsController(
        DisplayItemApplicationService service,
        IObjectMapper<DisplayItemBrand, GetDisplayItemBrandsResponse> mapper)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    ///  陳列品ブランドの一覧を取得します。
    /// </summary>
    /// <returns>陳列品ブランドの一覧。</returns>
    /// <response code="200">成功。</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetDisplayItemBrandsResponse>), StatusCodes.Status200OK)]
    [OpenApiOperation("getDisplayItemBrands")]
    public async Task<IActionResult> GetDisplayItemBrandsAsync()
    {
        var brands = await this.service.GetBrandsAsync();
        return this.Ok(brands
            .Select(brand => this.mapper.Convert(brand))
            .ToArray());
    }
}
