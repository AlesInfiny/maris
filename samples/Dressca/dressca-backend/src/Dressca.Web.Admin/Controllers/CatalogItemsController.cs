using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Admin.Controllers.ApiModel;
using Dressca.Web.Admin.Dto;
using Dressca.Web.Admin.Dto.Catalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Dressca.Web.Admin.Controllers
{
    /// <summary>
    ///  <see cref="CatalogItem"/> の情報にアクセスする API コントローラーです。
    /// </summary>
    [Route("api/catalog-items")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Roles = "Admin")]
    public class CatalogItemsController : ControllerBase
    {
        private readonly CatalogManagementApplicationService managementService;
        private readonly IObjectMapper<CatalogItem, CatalogItemResponse> mapper;

        /// <summary>
        ///  <see cref="CatalogItemsController"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="service">カタログアプリケーションサービス。</param>
        /// <param name="managementService">カタログ管理アプリケーションサービス。</param>
        /// <param name="mapper"><see cref="CatalogItem"/> と <see cref="CatalogItemResponse"/> のマッパー。</param>
        /// <exception cref="ArgumentNullException">
        ///  <list type="bullet">
        ///   <item><paramref name="service"/> が <see langword="null"/> です。</item>
        ///   <item><paramref name="managementService"/> が <see langword="null"/> です。</item>
        ///   <item><paramref name="mapper"/> が <see langword="null"/> です。</item>
        ///  </list>
        /// </exception>
        public CatalogItemsController(
            CatalogApplicationService service,
            CatalogManagementApplicationService managementService,
            IObjectMapper<CatalogItem, CatalogItemResponse> mapper)
        {
            this.managementService = managementService ?? throw new ArgumentNullException(nameof(managementService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        /// <summary>
        /// 指定したIDのカタログアイテムを返します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns>カタログアイテム。</returns>
        /// <response code="200">成功。</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CatalogItemResponse), StatusCodes.Status200OK)]
        [OpenApiOperation("getById")]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            var item = await this.managementService.GetCatalogItemAsync(id);
            var response = this.mapper.Convert(item);
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
        [OpenApiOperation("postCatalogItem")]
        public async Task<IActionResult> PostCatalogItemAsync(PostCatalogItemRequest postCatalogItemRequest)
        {

            var catalogItem = await managementService.AddItemToCatalogAsync(
                postCatalogItemRequest.Name,
                postCatalogItemRequest.Description,
                postCatalogItemRequest.Price,
                postCatalogItemRequest.ProductCode,
                postCatalogItemRequest.CatalogBrandId,
                postCatalogItemRequest.CatalogCategoryId
                );

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
        [OpenApiOperation("deleteCatalogItem")]
        public async Task<IActionResult> DeleteCatalogItemAsync(long catalogItemId)
        {

            await managementService.DeleteItemFromCatalogAsync(catalogItemId);
            return this.NoContent();
        }

        /// <summary>
        ///  カタログアイテムの情報を更新します。
        /// </summary>
        /// <param name="putCatalogItemRequest"></param>
        /// <returns>なし。</returns>
        /// <response code="204">成功。</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [OpenApiOperation("putCatalogItem")]
        public async Task<IActionResult> PutCatalogItemAsync(PutCatalogItemRequest putCatalogItemRequest)
        {

            var command = new CatalogItemUpdateCommand(
                putCatalogItemRequest.Id,
                putCatalogItemRequest.Name,
                putCatalogItemRequest.Description,
                putCatalogItemRequest.Price,
                putCatalogItemRequest.ProductCode,
                putCatalogItemRequest.CatalogBrandId,
                putCatalogItemRequest.CatalogCategoryId);

            await this.managementService.UpdateCatalogItemAsync(command);

            return this.NoContent();
        }

    }
}
