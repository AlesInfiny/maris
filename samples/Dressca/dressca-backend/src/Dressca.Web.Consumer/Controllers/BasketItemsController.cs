using System.ComponentModel.DataAnnotations;
using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Consumer.Baskets;
using Dressca.Web.Consumer.Dto.Baskets;
using Dressca.Web.Consumer.Dto.Catalog;
using Dressca.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Dressca.Web.Consumer.Controllers;

/// <summary>
///  <see cref="BasketItem"/> の情報にアクセスする API コントローラーです。
/// </summary>
[Route("api/basket-items")]
[ApiController]
[Produces("application/json")]
public class BasketItemsController : ControllerBase
{
    private readonly ShoppingApplicationService service;
    private readonly IObjectMapper<Basket, BasketResponse> basketMapper;
    private readonly IObjectMapper<BasketItem, BasketItemResponse> basketItemMapper;
    private readonly IObjectMapper<CatalogItem, CatalogItemResponse> catalogItemMapper;
    private readonly IObjectMapper<CatalogItem, CatalogItemSummaryResponse> catalogItemSummaryResponseMapper;
    private readonly ILogger<BasketItemsController> logger;

    /// <summary>
    ///  <see cref="BasketItemsController"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="service">ショッピングアプリケーションサービス。</param>
    /// <param name="basketMapper"><see cref="Basket"/> と <see cref="BasketResponse"/> のマッパー。</param>
    /// <param name="basketItemMapper"><see cref="BasketItem"/> と <see cref="BasketItemResponse"/> のマッパー。</param>
    /// <param name="catalogItemMapper"><see cref="CatalogItem"/> と <see cref="CatalogItemResponse"/> のマッパー。</param>
    /// <param name="catalogItemSummaryResponseMapper"><see cref="CatalogItem"/> と <see cref="CatalogItemSummaryResponse"/> のマッパー。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="basketMapper"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="basketItemMapper"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="catalogItemMapper"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="catalogItemSummaryResponseMapper"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public BasketItemsController(
        ShoppingApplicationService service,
        IObjectMapper<Basket, BasketResponse> basketMapper,
        IObjectMapper<BasketItem, BasketItemResponse> basketItemMapper,
        IObjectMapper<CatalogItem, CatalogItemResponse> catalogItemMapper,
        IObjectMapper<CatalogItem, CatalogItemSummaryResponse> catalogItemSummaryResponseMapper,
        ILogger<BasketItemsController> logger)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.basketMapper = basketMapper ?? throw new ArgumentNullException(nameof(basketMapper));
        this.basketItemMapper = basketItemMapper ?? throw new ArgumentNullException(nameof(basketItemMapper));
        this.catalogItemMapper = catalogItemMapper ?? throw new ArgumentNullException(nameof(catalogItemMapper));
        this.catalogItemSummaryResponseMapper = catalogItemSummaryResponseMapper ?? throw new ArgumentNullException(nameof(catalogItemSummaryResponseMapper));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///  買い物かごアイテムの一覧を取得します。
    /// </summary>
    /// <returns>買い物かごアイテムの一覧。</returns>
    /// <response code="200">成功。</response>
    [HttpGet]
    [ProducesResponseType(typeof(BasketResponse), StatusCodes.Status200OK)]
    [OpenApiOperation("getBasketItems")]
    public async Task<IActionResult> GetBasketItemsAsync()
    {
        var buyerId = this.HttpContext.GetBuyerId();

        var (basket, catalogItems, deletedCatalogItemIds) = await this.service.GetBasketItemsAsync(buyerId);

        var basketResponse = this.basketMapper.Convert(basket);
        foreach (var basketItem in basketResponse.BasketItems)
        {
            basketItem.CatalogItem = this.GetCatalogItemSummary(basketItem.CatalogItemId, catalogItems);
        }

        basketResponse.DeletedItemIds = deletedCatalogItemIds.ToList();

        return this.Ok(basketResponse);
    }

    /// <summary>
    ///  買い物かごアイテム内の数量を変更します。
    ///  買い物かご内に存在しないカタログアイテム ID は指定できません。
    /// </summary>
    /// <param name="putBasketItems">変更する買い物かごアイテムのデータリスト。</param>
    /// <returns>なし。</returns>
    /// <remarks>
    ///  <para>
    ///   この API では、買い物かご内に存在する商品の数量を変更できます。
    ///   買い物かご内に存在しないカタログアイテム Id を指定すると HTTP 400 を返却します。
    ///   またシステムに登録されていないカタログアイテム Id を指定した場合も HTTP 400 を返却します。
    ///  </para>
    /// </remarks>
    /// <response code="204">成功。</response>
    /// <response code="400">リクエストエラー。</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [OpenApiOperation("putBasketItems")]
    public async Task<IActionResult> PutBasketItemsAsync(IEnumerable<PutBasketItemsRequest> putBasketItems)
    {
        if (!putBasketItems.Any())
        {
            return this.NoContent();
        }

        var quantities = putBasketItems.ToDictionary(
            putBasketItem =>
            {
                putBasketItem.CatalogItemId.ThrowIfNull();
                return putBasketItem.CatalogItemId.Value;
            },
            putBasketItem =>
            {
                putBasketItem.Quantity.ThrowIfNull();
                return putBasketItem.Quantity.Value;
            });

        var buyerId = this.HttpContext.GetBuyerId();
        await this.service.SetBasketItemsQuantitiesAsync(buyerId, quantities);

        return this.NoContent();
    }

    /// <summary>
    ///  買い物かごに商品を追加します。
    /// </summary>
    /// <param name="postBasketItem">追加する商品の情報。</param>
    /// <returns>なし。</returns>
    /// <remarks>
    ///  <para>
    ///   この API では、システムに登録されていないカタログアイテム Id を指定した場合 HTTP 400 を返却します。
    ///   また買い物かごに追加していないカタログアイテムを指定した場合、その商品を買い物かごに追加します。
    ///   すでに買い物かごに追加されているカタログアイテムを指定した場合、指定した数量、買い物かご内の数量を追加します。
    ///  </para>
    ///  <para>
    ///   買い物かご内のカタログアイテムの数量が 0 未満になるように減じることはできません。
    ///   計算の結果数量が 0 未満になる場合 HTTP 500 を返却します。
    ///  </para>
    /// </remarks>
    /// <response code="201">作成完了。</response>
    /// <response code="400">リクエストエラー。</response>
    /// <response code="500">サーバーエラー。</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [OpenApiOperation("postBasketItem")]
    public async Task<IActionResult> PostBasketItemAsync(PostBasketItemsRequest postBasketItem)
    {
        postBasketItem.CatalogItemId.ThrowIfNull();
        postBasketItem.AddedQuantity.ThrowIfNull();

        var buyerId = this.HttpContext.GetBuyerId();

        await this.service.AddItemToBasketAsync(buyerId, postBasketItem.CatalogItemId.Value, postBasketItem.AddedQuantity.Value);

        var actionName = ActionNameHelper.GetAsyncActionName(nameof(this.GetBasketItemsAsync));
        return this.CreatedAtAction(actionName, null);
    }

    /// <summary>
    ///  買い物かごから指定したカタログアイテム Id の商品を削除します。
    /// </summary>
    /// <param name="catalogItemId">カタログアイテム Id 。</param>
    /// <returns>なし。</returns>
    /// <remarks>
    ///  <para>
    ///   catalogItemId には買い物かご内に存在するカタログアイテム Id を指定してください。
    ///   カタログアイテム Id は 1 以上の整数です。
    ///   0 以下の値を指定したり、整数値ではない値を指定した場合 HTTP 400 を返却します。
    ///   買い物かご内に指定したカタログアイテムの商品が存在しない場合、 HTTP 404 を返却します。
    ///  </para>
    /// </remarks>
    /// <response code="204">成功。</response>
    /// <response code="400">リクエストエラー。</response>
    /// <response code="404">買い物かご内に指定したカタログアイテム Id がない。</response>
    [HttpDelete("{catalogItemId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [OpenApiOperation("deleteBasketItem")]
    public async Task<IActionResult> DeleteBasketItemAsync([Range(1L, long.MaxValue)] long catalogItemId)
    {
        var buyerId = this.HttpContext.GetBuyerId();
        try
        {
            await this.service.SetBasketItemsQuantitiesAsync(buyerId, new() { { catalogItemId, 0 } });
        }
        catch (CatalogItemNotExistingInBasketException ex)
        {
            this.logger.LogWarning(Events.CatalogItemIdDoesNotExistInBasket, ex, ex.Message);
            return this.NotFound();
        }

        return this.NoContent();
    }

    private CatalogItemSummaryResponse? GetCatalogItemSummary(long catalogItemId, IEnumerable<CatalogItem> catalogItems)
    {
        var catalogItem = catalogItems.FirstOrDefault(catalogItem => catalogItem.Id == catalogItemId);
        return this.catalogItemSummaryResponseMapper.Convert(catalogItem);
    }
}
