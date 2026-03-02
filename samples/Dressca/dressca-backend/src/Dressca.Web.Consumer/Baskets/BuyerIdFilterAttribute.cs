using Dressca.Web.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Dressca.Web.Consumer.Baskets;

/// <summary>
///  購入者 Id の発行と保存を行う <see cref="ActionFilterAttribute"/> です。
/// </summary>
/// <remarks>
///  <para>
///   購入者 Id は、利用者を一意に識別するための情報です。
///   購入者 Id は Cookie を用いてリクエスト間で共有します。
///   サーバー処理内では、 <see cref="HttpContext"/> 内に保存して持ち回ります。
///  </para>
///  <para>
///   Cookie に購入者 Id が保存されていない場合は、 Web API の処理開始前に新たな購入者 Id を発行します。
///  </para>
/// </remarks>
public class BuyerIdFilterAttribute : ActionFilterAttribute
{
    private const string DefaultBuyerIdCookieName = "Dressca-Bid";
    private readonly string buyerIdCookieName;
    private readonly TimeProvider timeProvider;
    private readonly WebServerOptions options;
    private readonly CookiePolicyOptions cookiePolicyOptions;

    /// <summary>
    ///  <see cref="BuyerIdFilterAttribute"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="options">構成オプションに設定された Cookie の設定。</param>
    /// <param name="buyerIdCookieName">Cookie のキー名。未指定時は "Dressca-Bid" 。</param>
    /// <param name="cookiePolicyOptions">Cookie ポリシー オプション。</param>
    public BuyerIdFilterAttribute(IOptions<WebServerOptions> options, IOptions<CookiePolicyOptions> cookiePolicyOptions, string buyerIdCookieName = DefaultBuyerIdCookieName)
        : this(buyerIdCookieName, TimeProvider.System, options.Value, cookiePolicyOptions.Value)
    {
    }

    /// <summary>
    ///  <see cref="BuyerIdFilterAttribute"/> クラスの新しいインタンスを初期化します。
    ///  単体テスト用に<see cref="TimeProvider"/> を受け取ることができます。
    /// </summary>
    /// <param name="buyerIdCookieName">Cookie のキー名。</param>
    /// <param name="timeProvider">日時のプロバイダ。通常はシステム日時。</param>
    /// <param name="options">構成オプション。</param>
    /// <param name="cookiePolicyOptions">Cookie ポリシー オプション。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="buyerIdCookieName"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="timeProvider"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="options"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="cookiePolicyOptions"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    internal BuyerIdFilterAttribute(string buyerIdCookieName, TimeProvider timeProvider, WebServerOptions options, CookiePolicyOptions cookiePolicyOptions)
    {
        this.buyerIdCookieName = buyerIdCookieName ?? throw new ArgumentNullException(nameof(buyerIdCookieName));
        this.timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        this.options = options ?? throw new ArgumentNullException(nameof(options));
        this.cookiePolicyOptions = cookiePolicyOptions ?? throw new ArgumentNullException(nameof(cookiePolicyOptions));
    }

    /// <inheritdoc/>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        if (context.HttpContext.Request.Cookies.TryGetValue(this.buyerIdCookieName, out var buyerId))
        {
            if (!string.IsNullOrWhiteSpace(buyerId) && Guid.TryParse(buyerId, out _))
            {
                // Cookie から復元できる場合は上書き。
                context.HttpContext.SetBuyerId(buyerId);
                return;
            }
        }

        // Cookie から復元できない場合は作成して HttpContext に保存
        var newBuyerId = context.HttpContext.GetBuyerId();
        context.HttpContext.SetBuyerId(newBuyerId);
    }

    /// <inheritdoc/>
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var buyerId = context.HttpContext.GetBuyerId();
        context.HttpContext.Response.Cookies.Append(
            this.buyerIdCookieName,
            buyerId,
            this.options.CookieSettings.CreateCookieOptions(this.timeProvider, this.cookiePolicyOptions));

        base.OnActionExecuted(context);
    }
}
