using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.Ordering;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Consumer.Baskets;
using Dressca.Web.Consumer.Dto.Ordering;
using Dressca.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Dressca.Web.Consumer.Controllers;

/// <summary>
///  <see cref="Order"/> の情報にアクセスする API コントローラーです。
/// </summary>
[Route("api/orders")]
[ApiController]
[Produces("application/json")]
public class OrdersController : ControllerBase
{
    private readonly ShoppingApplicationService shoppingApplicationService;
    private readonly OrderApplicationService orderApplicationService;
    private readonly IObjectMapper<Order, OrderResponse> orderMapper;
    private readonly ILogger<OrdersController> logger;

    /// <summary>
    ///  <see cref="OrdersController"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="shoppingApplicationService">ショッピングアプリケーションサービス。</param>
    /// <param name="orderApplicationService">注文アプリケーションサービス。</param>
    /// <param name="orderMapper"><see cref="Order"/> と <see cref="OrderResponse"/> のマッパー。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="shoppingApplicationService"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="orderApplicationService"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="orderMapper"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public OrdersController(
        ShoppingApplicationService shoppingApplicationService,
        OrderApplicationService orderApplicationService,
        IObjectMapper<Order, OrderResponse> orderMapper,
        ILogger<OrdersController> logger)
    {
        this.shoppingApplicationService = shoppingApplicationService ?? throw new ArgumentNullException(nameof(shoppingApplicationService));
        this.orderApplicationService = orderApplicationService ?? throw new ArgumentNullException(nameof(orderApplicationService));
        this.orderMapper = orderMapper ?? throw new ArgumentNullException(nameof(orderMapper));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///  注文情報を取得します。
    /// </summary>
    /// <param name="orderId">注文 Id 。</param>
    /// <returns>注文情報。</returns>
    /// <response code="200">成功。</response>
    /// <response code="404">注文 Id が存在しない。</response>
    [HttpGet("{orderId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [OpenApiOperation("getById")]
    public async Task<IActionResult> GetByIdAsync(long orderId)
    {
        var buyerId = this.HttpContext.GetBuyerId();
        try
        {
            var order = await this.orderApplicationService.GetOrderAsync(orderId, buyerId);
            var orderDto = this.orderMapper.Convert(order);
            return this.Ok(orderDto);
        }
        catch (OrderNotFoundException ex)
        {
            this.logger.LogWarning(Events.OrderNotFound, ex, ex.Message);
            return this.NotFound();
        }
    }

    /// <summary>
    ///  買い物かごに登録されている商品を注文します。
    /// </summary>
    /// <param name="postOrderInput">注文に必要な配送先などの情報。</param>
    /// <returns>なし。</returns>
    /// <response code="201">成功。</response>
    /// <response code="400">リクエストエラー。</response>
    /// <response code="500">サーバーエラー。</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [OpenApiOperation("postOrder")]
    public async Task<IActionResult> PostOrderAsync(PostOrderRequest postOrderInput)
    {
        var buyerId = this.HttpContext.GetBuyerId();

        var address = new Address(
            postalCode: postOrderInput.PostalCode,
            todofuken: postOrderInput.Todofuken,
            shikuchoson: postOrderInput.Shikuchoson,
            azanaAndOthers: postOrderInput.AzanaAndOthers);
        var shipToAddress = new ShipTo(postOrderInput.FullName, address);

        var order = await this.shoppingApplicationService.CheckoutAsync(buyerId, shipToAddress);

        var actionName = ActionNameHelper.GetAsyncActionName(nameof(this.GetByIdAsync));
        return this.CreatedAtAction(actionName, new { orderId = order.Id }, null);
    }
}
