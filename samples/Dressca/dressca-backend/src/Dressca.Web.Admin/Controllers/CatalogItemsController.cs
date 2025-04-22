using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.Authorization;
using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Admin.Authorization;
using Dressca.Web.Admin.Controllers.ApiModel;
using Dressca.Web.Admin.Dto.CatalogItems;
using Dressca.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Dressca.Web.Admin.Controllers;

/// <summary>
///  <see cref="CatalogItem"/> の情報にアクセスする API コントローラーです。
/// </summary>
[Route("api/catalog-items")]
[ApiController]
[Produces("application/json")]
[Authorize]
public class CatalogItemsController : ControllerBase
{
    private readonly CatalogApplicationService service;
    private readonly IObjectMapper<CatalogItem, GetCatalogItemResponse> mapper;
    private readonly ILogger<CatalogItemsController> logger;

    /// <summary>
    ///  <see cref="CatalogItemsController"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="service">カタログアプリケーションサービス。</param>
    /// <param name="mapper"><see cref="CatalogItem"/> と <see cref="GetCatalogItemResponse"/> のマッパー。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="service"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="mapper"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public CatalogItemsController(
        CatalogApplicationService service,
        IObjectMapper<CatalogItem, GetCatalogItemResponse> mapper,
        ILogger<CatalogItemsController> logger)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///  指定した ID のカタログアイテムを返します。
    /// </summary>
    /// <param name="catalogItemId">ID。</param>
    /// <returns>カタログアイテム。</returns>
    /// <response code="200">成功。</response>
    /// <response code="400">リクエストエラー。</response>
    /// <response code="401">未認証。</response>
    /// <response code="404">指定した ID のアイテムがカタログに存在しない。</response>
    /// <response code="500">サーバーエラー。</response>
    [HttpGet("{catalogItemId}")]
    [ProducesResponseType(typeof(GetCatalogItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [OpenApiOperation("getCatalogItem")]
    [Authorize(Policy = Policies.RequireAdminRole)]
    public async Task<IActionResult> GetCatalogItemAsync(long catalogItemId)
    {
        CatalogItem? catalogItem;

        try
        {
            catalogItem = await this.service.GetCatalogItemForAdminAsync(catalogItemId);
        }
        catch (PermissionDeniedException ex)
        {
            this.logger.LogWarning(Events.PermissionDenied, ex, ex.Message);
            return this.NotFound();
        }
        catch (CatalogItemNotExistingInRepositoryException ex)
        {
            this.logger.LogWarning(Events.CatalogItemNotExistingInRepository, ex, ex.Message);
            return this.NotFound();
        }

        var response = this.mapper.Convert(catalogItem);
        return this.Ok(response);
    }

    /// <summary>
    ///  カタログアイテムを検索して一覧を返します。
    /// </summary>
    /// <returns>カタログアイテムの一覧。</returns>
    /// <param name="query">検索クエリ。</param>
    /// <response code="200">成功。</response>
    /// <response code="400">リクエストエラー。</response>
    /// <response code="401">未認証。</response>
    /// <response code="404">失敗。</response>
    /// <response code="500">サーバーエラー。</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<GetCatalogItemResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [OpenApiOperation("getByQuery")]
    [Authorize(Policy = Policies.RequireAdminRole)]
    public async Task<IActionResult> GetByQueryAsync([FromQuery] FindCatalogItemsQuery query)
    {
        (IReadOnlyList<CatalogItem> CatalogItems, int TotalCount) itemsAndCount;

        try
        {
            itemsAndCount =
            await this.service.GetCatalogItemsForAdminAsync(
                skip: query.GetSkipCount(),
                take: query.PageSize,
                brandId: query.BrandId,
                categoryId: query.CategoryId);
        }
        catch (PermissionDeniedException ex)
        {
            this.logger.LogWarning(Events.PermissionDenied, ex, ex.Message);
            return this.NotFound();
        }

        var items = itemsAndCount.CatalogItems
            .Select(catalogItem => this.mapper.Convert(catalogItem))
            .ToList();
        var returnValue = new PagedList<GetCatalogItemResponse>(
            items: items,
            totalCount: itemsAndCount.TotalCount,
            page: query.Page,
            pageSize: query.PageSize);
        return this.Ok(returnValue);
    }

    /// <summary>
    ///  カタログにアイテムを追加します。
    /// </summary>
    /// <param name="postCatalogItemRequest">追加するアイテムの情報。</param>
    /// <returns>なし。</returns>
    /// <response code="201">成功。</response>
    /// <response code="400">リクエストエラー。</response>
    /// <response code="401">未認証。</response>
    /// <response code="404">失敗。</response>
    /// <response code="500">サーバーエラー。</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [OpenApiOperation("postCatalogItem")]
    [Authorize(Policy = Policies.RequireAdminRole)]
    public async Task<IActionResult> PostCatalogItemAsync(PostCatalogItemRequest postCatalogItemRequest)
    {
        CatalogItem catalogItem;

        try
        {
            catalogItem = await this.service.AddItemToCatalogAsync(
                postCatalogItemRequest.Name,
                postCatalogItemRequest.Description,
                postCatalogItemRequest.Price,
                postCatalogItemRequest.ProductCode,
                postCatalogItemRequest.CatalogBrandId,
                postCatalogItemRequest.CatalogCategoryId);
        }
        catch (PermissionDeniedException ex)
        {
            this.logger.LogWarning(Events.PermissionDenied, ex, ex.Message);
            return this.NotFound();
        }

        var actionName = ActionNameHelper.GetAsyncActionName(nameof(this.GetCatalogItemAsync));

        return this.CreatedAtAction(actionName, new { catalogItemId = catalogItem.Id }, null);
    }

    /// <summary>
    ///  カタログから指定したカタログアイテム ID のアイテムを削除します。
    /// </summary>
    /// <param name="catalogItemId">カタログアイテム ID 。</param>
    /// <param name="rowVersion">行バージョン</param>
    /// <returns>なし。</returns>
    /// <response code="204">成功。</response>
    /// <response code="400">リクエストエラー。</response>
    /// <response code="401">未認証。</response>
    /// <response code="404">指定した ID のアイテムがカタログに存在しない。</response>
    /// <response code="409">競合が発生。</response>
    /// <response code="500">サーバーエラー。</response>
    [HttpDelete("{catalogItemId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [OpenApiOperation("deleteCatalogItem")]
    [Authorize(Policy = Policies.RequireAdminRole)]
    public async Task<IActionResult> DeleteCatalogItemAsync(long catalogItemId, [FromQuery] byte[] rowVersion)
    {
        try
        {
            await this.service.DeleteItemFromCatalogAsync(catalogItemId, rowVersion);
        }
        catch (PermissionDeniedException ex)
        {
            this.logger.LogWarning(Events.PermissionDenied, ex, ex.Message);
            return this.NotFound();
        }
        catch (CatalogItemNotExistingInRepositoryException ex)
        {
            this.logger.LogWarning(Events.CatalogItemNotExistingInRepository, ex, ex.Message);
            return this.NotFound();
        }

        return this.NoContent();
    }

    /// <summary>
    ///  指定した ID のカタログアイテムの情報を更新します。
    /// </summary>
    /// <param name="catalogItemId">カタログアイテムID。</param>
    /// <param name="putCatalogItemRequest">更新するカタログアイテムの情報。</param>
    /// <returns>なし。</returns>
    /// <response code="204">成功。</response>
    /// <response code="400">リクエストエラー。</response>
    /// <response code="401">未認証。</response>
    /// <response code="404">指定した ID のアイテムがカタログに存在しない。</response>
    /// <response code="409">競合が発生。</response>
    /// <response code="500">サーバーエラー。</response>
    [HttpPut("{catalogItemId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [OpenApiOperation("putCatalogItem")]
    [Authorize(Policy = Policies.RequireAdminRole)]
    public async Task<IActionResult> PutCatalogItemAsync(long catalogItemId, PutCatalogItemRequest putCatalogItemRequest)
    {
        try
        {
            await this.service.UpdateCatalogItemAsync(
                catalogItemId,
                putCatalogItemRequest.Name,
                putCatalogItemRequest.Description,
                putCatalogItemRequest.Price,
                putCatalogItemRequest.ProductCode,
                putCatalogItemRequest.CatalogBrandId,
                putCatalogItemRequest.CatalogCategoryId,
                putCatalogItemRequest.RowVersion,
                putCatalogItemRequest.IsDeleted);
        }
        catch (PermissionDeniedException ex)
        {
            this.logger.LogWarning(Events.PermissionDenied, ex, ex.Message);
            return this.NotFound();
        }
        catch (CatalogItemNotExistingInRepositoryException ex)
        {
            this.logger.LogWarning(Events.CatalogItemNotExistingInRepository, ex, ex.Message);
            return this.NotFound();
        }

        return this.NoContent();
    }
}
