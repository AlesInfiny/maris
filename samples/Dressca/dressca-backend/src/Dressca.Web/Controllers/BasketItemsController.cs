using System.ComponentModel.DataAnnotations;
using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.BuyerIdExtensions;
using Dressca.Web.Dto.Baskets;
using Dressca.Web.Dto.Catalog;
using Dressca.Web.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Dressca.Web.Controllers;

/// <summary>
///  <see cref="BasketItem"/> の情報にアクセスする API コントローラーです。
/// </summary>
[Route("api/basket-items")]
[ApiController]
[Produces("application/json")]
public class BasketItemsController : ControllerBase
{
    private const string BuyerIdCookieName = "Dressca-Bid";
    private readonly BasketApplicationService basketApplicationService;
    private readonly CatalogDomainService catalogDomainService;
    private readonly ICatalogRepository catalogRepository;
    private readonly IObjectMapper<BasketItem, BasketItemDto> basketItemMapper;
    private readonly IObjectMapper<CatalogItem, CatalogItemDto> catalogItemMapper;
    private readonly ILogger<BasketItemsController> logger;

    /// <summary>
    ///  <see cref="BasketItemsController"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="basketApplicationService">買い物かごアプリケーションサービス。</param>
    /// <param name="catalogDomainService">カタログドメインサービス。</param>
    /// <param name="catalogRepository">カタログアリポジトリ。</param>
    /// <param name="basketItemMapper"><see cref="Basket"/> と <see cref="BasketItemDto"/> のマッパー。</param>
    /// <param name="catalogItemMapper"><see cref="CatalogItem"/> と <see cref="CatalogItemDto"/> のマッパー。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="basketApplicationService"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="catalogDomainService"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="catalogRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="basketItemMapper"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="catalogItemMapper"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public BasketItemsController(
        BasketApplicationService basketApplicationService,
        CatalogDomainService catalogDomainService,
        ICatalogRepository catalogRepository,
        IObjectMapper<BasketItem, BasketItemDto> basketItemMapper,
        IObjectMapper<CatalogItem, CatalogItemDto> catalogItemMapper,
        ILogger<BasketItemsController> logger)
    {
        this.basketApplicationService = basketApplicationService ?? throw new ArgumentNullException(nameof(basketApplicationService));
        this.catalogDomainService = catalogDomainService ?? throw new ArgumentNullException(nameof(catalogDomainService));
        this.catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
        this.basketItemMapper = basketItemMapper ?? throw new ArgumentNullException(nameof(basketItemMapper));
        this.catalogItemMapper = catalogItemMapper ?? throw new ArgumentNullException(nameof(catalogItemMapper));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///  買い物かごアイテムの一覧を取得します。
    /// </summary>
    /// <returns>買い物かごアイテムの一覧。</returns>
    /// <response code="200">成功。</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BasketItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List()
    {
        var buyerId = this.HttpContext.GetBuyerId();
        var basket = await this.basketApplicationService.GetOrCreateBasketForUser(buyerId);
        var catalogItemIds = basket.Items.Select(basketItem => basketItem.CatalogItemId).ToList();
        var catalogItems = await this.catalogRepository.FindAsync(catalogItem => catalogItemIds.Contains(catalogItem.Id));
        var returnValue = basket.Items.Select(basketItem =>
        {
            var basketItemDto = this.basketItemMapper.Convert(basketItem);
            basketItemDto.CatalogItem = this.GetCatalogItemSummary(basketItem.CatalogItemId, catalogItems);
            return basketItemDto;
        });
        return this.Ok(returnValue);
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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutBasketItems(IEnumerable<PutBasketItemsInputDto> putBasketItems)
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

        // 買い物かごに入っていないカタログアイテムが指定されていないか確認
        var buyerId = this.HttpContext.GetBuyerId();
        var basket = await this.basketApplicationService.GetOrCreateBasketForUser(buyerId);
        var notExistsInBasketCatalogIds = quantities.Keys.Where(catalogItemId => !basket.IsInCatalogItem(catalogItemId));
        if (notExistsInBasketCatalogIds.Any())
        {
            this.logger.LogWarning(WebMessages.CatalogItemIdDoesNotExistInBasket, string.Join(',', notExistsInBasketCatalogIds));
            return this.BadRequest();
        }

        // カタログリポジトリに存在しないカタログアイテムが指定されていないか確認
        var (existsAll, _) = await this.catalogDomainService.ExistsAllAsync(quantities.Keys);
        if (!existsAll)
        {
            return this.BadRequest();
        }

        await this.basketApplicationService.SetQuantities(basket.Id, quantities);
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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> PostBasketItem(PostBasketItemsInputDto postBasketItem)
    {
        postBasketItem.CatalogItemId.ThrowIfNull();
        postBasketItem.AddedQuantity.ThrowIfNull();

        var buyerId = this.HttpContext.GetBuyerId();
        var basket = await this.basketApplicationService.GetOrCreateBasketForUser(buyerId);

        // カタログリポジトリに存在しないカタログアイテムが指定されていないか確認
        var (existsAll, catalogItems) = await this.catalogDomainService.ExistsAllAsync(new[] { postBasketItem.CatalogItemId.Value });
        if (!existsAll)
        {
            return this.BadRequest();
        }

        var catalogItem = catalogItems[0];
        await this.basketApplicationService.AddItemToBasket(basket.Id, postBasketItem.CatalogItemId.Value, catalogItem.Price, postBasketItem.AddedQuantity.Value);
        return this.CreatedAtAction(nameof(this.List), null);
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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBasketItem([Range(1L, long.MaxValue)] long catalogItemId)
    {
        // 買い物かごに入っていないカタログアイテムが指定されていないか確認
        var buyerId = this.HttpContext.GetBuyerId();
        var basket = await this.basketApplicationService.GetOrCreateBasketForUser(buyerId);
        if (!basket.IsInCatalogItem(catalogItemId))
        {
            this.logger.LogWarning(WebMessages.CatalogItemIdDoesNotExistInBasket, catalogItemId);
            return this.NotFound();
        }

        await this.basketApplicationService.SetQuantities(basket.Id, new() { { catalogItemId, 0 } });
        return this.NoContent();
    }

    private CatalogItemSummaryDto? GetCatalogItemSummary(long catalogItemId, IEnumerable<CatalogItem> catalogItems)
    {
        var catalogItem = catalogItems.FirstOrDefault(catalogItem => catalogItem.Id == catalogItemId);
        return this.catalogItemMapper.Convert(catalogItem);
    }
}
