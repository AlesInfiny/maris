using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.Authorization;
using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Admin.Controllers.ApiModel;
using Dressca.Web.Admin.Dto;
using Dressca.Web.Admin.Dto.Catalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;

namespace Dressca.Web.Admin.Controllers
{
    /// <summary>
    ///  <see cref="CatalogItem"/> の情報にアクセスする API コントローラーです。
    /// </summary>
    [Route("api/catalog-items")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class CatalogItemsController : ControllerBase
    {
        private readonly CatalogManagementApplicationService managementService;
        private readonly IObjectMapper<CatalogItem, CatalogItemResponse> mapper;
        private readonly ILogger<CatalogItemsController> logger;

        /// <summary>
        ///  <see cref="CatalogItemsController"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="service">カタログアプリケーションサービス。</param>
        /// <param name="managementService">カタログ管理アプリケーションサービス。</param>
        /// <param name="mapper"><see cref="CatalogItem"/> と <see cref="CatalogItemResponse"/> のマッパー。</param>
        /// <param name="logger">ロガー。</param>
        /// <exception cref="ArgumentNullException">
        ///  <list type="bullet">
        ///   <item><paramref name="service"/> が <see langword="null"/> です。</item>
        ///   <item><paramref name="managementService"/> が <see langword="null"/> です。</item>
        ///   <item><paramref name="mapper"/> が <see langword="null"/> です。</item>
        ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
        ///  </list>
        /// </exception>
        public CatalogItemsController(
            CatalogApplicationService service,
            CatalogManagementApplicationService managementService,
            IObjectMapper<CatalogItem, CatalogItemResponse> mapper,
            ILogger<CatalogItemsController> logger)
        {
            this.managementService = managementService ?? throw new ArgumentNullException(nameof(managementService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        /// <summary>
        /// 指定したIDのカタログアイテムを返します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns>カタログアイテム。</returns>
        /// <response code="200">成功。</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CatalogItemResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [OpenApiOperation("getById")]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            CatalogItem? catalogItem;

            try
            {
                catalogItem = await this.managementService.GetCatalogItemAsync(id);
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
                await this.managementService.GetCatalogItemsAsync(
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
        /// <param name="postCatalogItemRequest"></param>
        /// <response code="201">成功。</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [OpenApiOperation("postCatalogItem")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostCatalogItemAsync(PostCatalogItemRequest postCatalogItemRequest)
        {

            CatalogItem catalogItem;

            try
            {
                catalogItem = await managementService.AddItemToCatalogAsync(
                    postCatalogItemRequest.Name,
                    postCatalogItemRequest.Description,
                    postCatalogItemRequest.Price,
                    postCatalogItemRequest.ProductCode,
                    postCatalogItemRequest.CatalogBrandId,
                    postCatalogItemRequest.CatalogCategoryId
                    );
            }
            catch (PermissionDeniedException ex)
            {
                this.logger.LogWarning(Events.PermissionDenied, ex, ex.Message);
                return Unauthorized();
            }

            var actionName = ActionNameHelper.GetAsyncActionName(nameof(this.PostCatalogItemAsync));

            return this.CreatedAtAction(actionName, new { catalogItemId = catalogItem.Id }, null);
        }

        /// <summary>
        ///  カタログから指定したカタログアイテム Id のアイテムを削除します。
        /// </summary>
        /// <param name="catalogItemId">カタログアイテム Id 。</param>
        /// <returns>なし。</returns>
        /// <response code="204">成功。</response>
        [HttpDelete("{catalogItemId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [OpenApiOperation("deleteCatalogItem")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCatalogItemAsync(long catalogItemId)
        {
            try
            {
                await managementService.DeleteItemFromCatalogAsync(catalogItemId);
            }
            catch (PermissionDeniedException ex)
            {
                this.logger.LogWarning(Events.PermissionDenied, ex, ex.Message);
                return Unauthorized();
            }
            catch (CatalogItemNotExistingInRepositoryException ex)
            {
                this.logger.LogWarning(Events.CatalogItemNotExistingInRepository, ex, ex.Message);
                return this.NotFound();
            }
            return this.NoContent();
        }

        /// <summary>
        ///  カタログアイテムの情報を更新します。
        /// </summary>
        /// <param name="putCatalogItemRequest"></param>
        /// <returns>なし。</returns>
        /// <response code="204">成功。</response>
        /// <response code="401">認可エラー。</response>
        /// <response code="404">対象のIDが存在しない。</response>
        /// <response code="409">更新の競合が発生。</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [OpenApiOperation("putCatalogItem")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutCatalogItemAsync(PutCatalogItemRequest putCatalogItemRequest)
        {

            var command = new CatalogItemUpdateCommand(
                putCatalogItemRequest.Id,
                putCatalogItemRequest.Name,
                putCatalogItemRequest.Description,
                putCatalogItemRequest.Price,
                putCatalogItemRequest.ProductCode,
                putCatalogItemRequest.CatalogBrandId,
                putCatalogItemRequest.CatalogCategoryId,
                putCatalogItemRequest.RowVersion
                );

            try
            {
                await this.managementService.UpdateCatalogItemAsync(command);
            }
            catch (PermissionDeniedException ex)
            {
                this.logger.LogWarning(Events.PermissionDenied, ex, ex.Message);
                return Unauthorized();
            }
            catch (CatalogItemNotExistingInRepositoryException ex)
            {
                this.logger.LogWarning(Events.CatalogItemNotExistingInRepository, ex, ex.Message);
                return this.NotFound();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                this.logger.LogWarning(Events.CatalogItemNotExistingInRepository, ex, ex.Message);
                return this.Conflict();
            }
            return this.NoContent();
        }

    }
}
