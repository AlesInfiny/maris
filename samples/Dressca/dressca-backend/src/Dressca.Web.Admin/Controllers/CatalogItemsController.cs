using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.Authorization;
using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Admin.Controllers.ApiModel;
using Dressca.Web.Admin.Dto;
using Dressca.Web.Admin.Dto.Catalog;
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
    private readonly IObjectMapper<CatalogItem, CatalogItemResponse> mapper;
    private readonly ILogger<CatalogItemsController> logger;

    /// <summary>
    ///  <see cref="CatalogItemsController"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="service">カタログアプリケーションサービス。</param>
    /// <param name="mapper"><see cref="CatalogItem"/> と <see cref="CatalogItemResponse"/> のマッパー。</param>
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
        IObjectMapper<CatalogItem, CatalogItemResponse> mapper,
        ILogger<CatalogItemsController> logger)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// 指定したIDのカタログアイテムを返します。
    /// </summary>
    /// <param name="id">ID。</param>
    /// <returns>カタログアイテム。</returns>
    /// <response code="200">成功。</response>
    /// <response code="404">指定した ID のアイテムがカタログに存在しない。</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CatalogItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [OpenApiOperation("getById")]
    public async Task<IActionResult> GetByIdAsync(long id)
    {
        CatalogItem? catalogItem;

        try
        {
            catalogItem = await this.service.GetCatalogItemByAdminAsync(id);
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

    /// <summary>
    ///  カタログにアイテムを追加します。
    /// </summary>
    /// <param name="postCatalogItemRequest">追加するアイテムの情報。</param>
    /// <response code="201">成功。</response>
    /// <response code="404">失敗。</response>
    /// <returns>representing the asynchronous operation.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [OpenApiOperation("postCatalogItem")]
    [Authorize(Roles = "Admin")]
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

        var actionName = ActionNameHelper.GetAsyncActionName(nameof(this.PostCatalogItemAsync));

        return this.CreatedAtAction(actionName, new { catalogItemId = catalogItem.Id }, null);
    }

    /// <summary>
    ///  カタログから指定したカタログアイテム ID のアイテムを削除します。
    /// </summary>
    /// <param name="catalogItemId">カタログアイテム ID 。</param>
    /// <returns>なし。</returns>
    /// <response code="204">成功。</response>
    /// <response code="404">指定した ID のアイテムがカタログに存在しない。</response>
    [HttpDelete("{catalogItemId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [OpenApiOperation("deleteCatalogItem")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCatalogItemAsync(long catalogItemId)
    {
        try
        {
            await this.service.DeleteItemFromCatalogAsync(catalogItemId);
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
    ///  指定したIDのカタログアイテムの情報を更新します。
    /// </summary>
    /// <param name="catalogItemId">カタログアイテムID。</param>
    /// <param name="putCatalogItemRequest">更新するカタログアイテムの情報。</param>
    /// <returns>なし。</returns>
    /// <response code="204">成功。</response>
    /// <response code="404">指定した ID のアイテムがカタログに存在しない。</response>
    /// <response code="409">更新の競合が発生。</response>
    [HttpPut("{catalogItemId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
    [OpenApiOperation("putCatalogItem")]
    [Authorize(Roles = "Admin")]
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
                putCatalogItemRequest.RowVersion);
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
