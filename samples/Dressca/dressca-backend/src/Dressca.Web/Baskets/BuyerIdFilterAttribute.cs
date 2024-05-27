﻿using Dressca.Web.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

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
    private readonly WebServerOptions? options;

    /// <summary>
    ///  <see cref="BuyerIdFilterAttribute"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="options">構成オプションに設定された Cookie の設定。</param>
    /// <param name="buyerIdCookieName">Cookie のキー名。未指定時は "Dressca-Bid" 。</param>
    public BuyerIdFilterAttribute(IOptions<WebServerOptions> options, string buyerIdCookieName = DefaultBuyerIdCookieName)
        : this(buyerIdCookieName, TimeProvider.System, options.Value)
    {
    }

    /// <summary>
    ///  <see cref="BuyerIdFilterAttribute"/> クラスの新しいインタンスを初期化します。
    ///  単体テスト用に<see cref="TimeProvider"/> を受け取ることができます。
    /// </summary>
    /// <param name="buyerIdCookieName">Cookie のキー名。</param>
    /// <param name="timeProvider">日時のプロバイダ。通常はシステム日時。</param>
    /// <param name="options">構成オプション。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <paramref name="buyerIdCookieName"/> が <see langword="null"/> です。
    ///   <paramref name="timeProvider"/> が <see langword="null"/> です。
    ///  </list>
    /// </exception>
    internal BuyerIdFilterAttribute(string buyerIdCookieName, TimeProvider timeProvider, WebServerOptions? options)
    {
        this.buyerIdCookieName = buyerIdCookieName ?? throw new ArgumentNullException(nameof(buyerIdCookieName));
        this.timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        this.options = options;
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
            this.CreateCookieOptions());

        base.OnActionExecuted(context);
    }

    /// <summary>
    /// 構成ファイルの内容を取得して Cookie の各種オプションを作成します。
    /// </summary>
    /// <returns>Cookie の各種オプション</returns>
    private CookieOptions CreateCookieOptions()
    {
        var defaultCookie = new CookieOptions()
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Secure = true,
            Expires = this.timeProvider.GetLocalNow().AddDays(7),
        };

        if (this.options == null || this.options.CookieSettings == null)
        {
            return defaultCookie;
        }

        var optionSettings = this.options.CookieSettings;

        var cookieOptions = new CookieOptions();
        cookieOptions.Expires = this.timeProvider.GetLocalNow().AddDays(optionSettings.ExpiredDays);
        cookieOptions.HttpOnly = optionSettings.HttpOnly;
        cookieOptions.Secure = optionSettings.Secure;
        cookieOptions.SameSite = optionSettings.SameSite;

        if (!string.IsNullOrEmpty(optionSettings.Domain))
        {
            cookieOptions.Domain = optionSettings.Domain;
        }

        return cookieOptions;
    }
}
