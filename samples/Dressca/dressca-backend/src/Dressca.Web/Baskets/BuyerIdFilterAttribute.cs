﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace Dressca.Web.Baskets;

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

    /// <summary>
    ///  <see cref="BuyerIdFilterAttribute"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="buyerIdCookieName">Cookie のキー名。未指定時は "Dressca-Bid" 。</param>
    public BuyerIdFilterAttribute(string buyerIdCookieName = DefaultBuyerIdCookieName)
        : this(buyerIdCookieName, TimeProvider.System)
    {
    }

    /// <summary>
    ///  <see cref="BuyerIdFilterAttribute"/> クラスの新しいインタンスを初期化します。
    ///  単体テスト用に<see cref="TimeProvider"/> を受け取ることができます。
    /// </summary>
    /// <param name="buyerIdCookieName">Cookie のキー名。</param>
    /// <param name="timeProvider">日時のプロバイダ。通常はシステム日時。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <paramref name="buyerIdCookieName"/> が <see langword="null"/> です。
    ///   <paramref name="timeProvider"/> が <see langword="null"/> です。
    ///  </list>
    /// </exception>
    internal BuyerIdFilterAttribute(string buyerIdCookieName, TimeProvider timeProvider)
    {
        this.buyerIdCookieName = buyerIdCookieName ?? throw new ArgumentNullException(nameof(buyerIdCookieName));
        this.timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
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
            new CookieOptions()
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true,
                Expires = this.timeProvider.GetLocalNow().AddDays(7),
            });

        base.OnActionExecuted(context);
    }
}
