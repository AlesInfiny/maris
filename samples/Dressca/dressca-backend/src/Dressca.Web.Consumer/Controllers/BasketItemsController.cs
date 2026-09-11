using System.ComponentModel.DataAnnotations;
using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.DisplayItems;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Consumer.Baskets;
using Dressca.Web.Consumer.Dto.Baskets;
using Dressca.Web.Consumer.Dto.DisplayItem;
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
    private readonly IObjectMapper<Basket, GetBasketItemsResponse> basketMapper;
    private readonly IObjectMapper<BasketItem, BasketItemApiModel> basketItemMapper;
    private readonly IObjectMapper<DisplayItem, GetDisplayItemResponse> displayItemMapper;
    private readonly IObjectMapper<DisplayItem, DisplayItemSummaryApiModel> displayItemSummaryResponseMapper;
    private readonly ILogger<BasketItemsController> logger;

    /// <summary>
    ///  <see cref="BasketItemsController"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="service">ショッピングアプリケーションサービス。</param>
    /// <param name="basketMapper"><see cref="Basket"/> と <see cref="GetBasketItemsResponse"/> のマッパー。</param>
    /// <param name="basketItemMapper"><see cref="BasketItem"/> と <see cref="BasketItemApiModel"/> のマッパー。</param>
    /// <param name="displayItemMapper"><see cref="DisplayItem"/> と <see cref="GetDisplayItemResponse"/> のマッパー。</param>
    /// <param name="displayItemSummaryResponseMapper"><see cref="DisplayItem"/> と <see cref="DisplayItemSummaryApiModel"/> のマッパー。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="basketMapper"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="basketItemMapper"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="displayItemMapper"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="displayItemSummaryResponseMapper"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public BasketItemsController(
        ShoppingApplicationService service,
        IObjectMapper<Basket, GetBasketItemsResponse> basketMapper,
        IObjectMapper<BasketItem, BasketItemApiModel> basketItemMapper,
        IObjectMapper<DisplayItem, GetDisplayItemResponse> displayItemMapper,
        IObjectMapper<DisplayItem, DisplayItemSummaryApiModel> displayItemSummaryResponseMapper,
        ILogger<BasketItemsController> logger)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.basketMapper = basketMapper ?? throw new ArgumentNullException(nameof(basketMapper));
        this.basketItemMapper = basketItemMapper ?? throw new ArgumentNullException(nameof(basketItemMapper));
        this.displayItemMapper = displayItemMapper ?? throw new ArgumentNullException(nameof(displayItemMapper));
        this.displayItemSummaryResponseMapper = displayItemSummaryResponseMapper ?? throw new ArgumentNullException(nameof(displayItemSummaryResponseMapper));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///  買い物かごアイテムの一覧を取得します。
    /// </summary>
    /// <returns>買い物かごアイテムの一覧。</returns>
    /// <response code="200">成功。</response>
    [HttpGet]
    [ProducesResponseType(typeof(GetBasketItemsResponse), StatusCodes.Status200OK)]
    [OpenApiOperation("getBasketItems")]
    public async Task<IActionResult> GetBasketItemsAsync()
    {
        var buyerId = this.HttpContext.GetBuyerId();

        var (basket, displayItems, deletedDisplayItemIds) = await this.service.GetBasketItemsAsync(buyerId);

        var basketResponse = this.basketMapper.Convert(basket);
        foreach (var basketItem in basketResponse.BasketItems)
        {
            basketItem.DisplayItem = this.GetDisplayItemSummary(basketItem.DisplayItemId, displayItems);
        }

        basketResponse.DeletedItemIds = deletedDisplayItemIds.ToList();

        return this.Ok(basketResponse);
    }

    /// <summary>
    ///  買い物かごアイテム内の数量を変更します。
    ///  買い物かご内に存在しない陳列品 ID は指定できません。
    /// </summary>
    /// <param name="putBasketItems">変更する買い物かごアイテムのデータリスト。</param>
    /// <returns>なし。</returns>
    /// <remarks>
    ///  <para>
    ///   この API では、買い物かご内に存在する商品の数量を変更できます。
    ///   買い物かご内に存在しない陳列品 Id を指定すると HTTP 400 を返却します。
    ///   またシステムに登録されていない陳列品 Id を指定した場合も HTTP 400 を返却します。
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
                putBasketItem.DisplayItemId.ThrowIfNull();
                return putBasketItem.DisplayItemId.Value;
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
    ///   この API では、システムに登録されていない陳列品 Id を指定した場合 HTTP 400 を返却します。
    ///   また買い物かごに追加していない陳列品を指定した場合、その商品を買い物かごに追加します。
    ///   すでに買い物かごに追加されている陳列品を指定した場合、指定した数量、買い物かご内の数量を追加します。
    ///  </para>
    ///  <para>
    ///   買い物かご内の陳列品の数量が 0 未満になるように減じることはできません。
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
        postBasketItem.DisplayItemId.ThrowIfNull();
        postBasketItem.AddedQuantity.ThrowIfNull();

        var buyerId = this.HttpContext.GetBuyerId();

        await this.service.AddItemToBasketAsync(buyerId, postBasketItem.DisplayItemId.Value, postBasketItem.AddedQuantity.Value);

        var actionName = ActionNameHelper.GetAsyncActionName(nameof(this.GetBasketItemsAsync));
        return this.CreatedAtAction(actionName, null);
    }

    /// <summary>
    ///  買い物かごから指定した陳列品 Id の商品を削除します。
    /// </summary>
    /// <param name="displayItemId">陳列品 Id 。</param>
    /// <returns>なし。</returns>
    /// <remarks>
    ///  <para>
    ///   displayItemId には買い物かご内に存在する陳列品 Id を指定してください。
    ///   陳列品 Id には UUID を指定してください。
    ///   UUID ではない値を指定した場合 HTTP 400 を返却します。
    ///   買い物かご内に指定した陳列品の商品が存在しない場合、 HTTP 404 を返却します。
    ///  </para>
    /// </remarks>
    /// <response code="204">成功。</response>
    /// <response code="400">リクエストエラー。</response>
    /// <response code="404">買い物かご内に指定した陳列品 Id がない。</response>
    [HttpDelete("{displayItemId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [OpenApiOperation("deleteBasketItem")]
    public async Task<IActionResult> DeleteBasketItemAsync(Guid displayItemId)
    {
        var buyerId = this.HttpContext.GetBuyerId();
        try
        {
            await this.service.RemoveItemFromBasketAsync(buyerId, displayItemId);
        }
        catch (DisplayItemNotExistingInBasketException ex)
        {
            this.logger.LogWarning(Events.DisplayItemIdDoesNotExistInBasket, ex, ex.Message);
            return this.NotFound();
        }

        return this.NoContent();
    }

    private DisplayItemSummaryApiModel? GetDisplayItemSummary(Guid displayItemId, IEnumerable<DisplayItem> displayItems)
    {
        var displayItem = displayItems.FirstOrDefault(displayItem => displayItem.Id == displayItemId);
        return this.displayItemSummaryResponseMapper.Convert(displayItem);
    }
}
