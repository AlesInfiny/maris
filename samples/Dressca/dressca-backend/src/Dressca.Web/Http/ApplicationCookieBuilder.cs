using Dressca.Web.Configuration;
using Dressca.Web.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Dressca.Web.Http;

/// <summary>
/// アプリケーション構成設定および CookiePolicy を元に Cookie の設定を作成するためのクラスです。
/// </summary>
public class ApplicationCookieBuilder
{
    private readonly WebServerOptions webServerOptions;

    /// <summary>
    /// <see cref="ApplicationCookieBuilder"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="webServerOptions">アプリケーション構成設定の WebServerOptions。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="webServerOptions"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public ApplicationCookieBuilder(IOptions<WebServerOptions> webServerOptions)
    {
        ArgumentNullException.ThrowIfNull(webServerOptions);
        this.webServerOptions = webServerOptions.Value;
    }

    /// <summary>
    /// アプリケーション構成設定およびプログラムで設定した CookiePolicy を元に <see cref="CookieBuilder"/> を作成します。
    /// </summary>
    /// <param name="cookieName">Cookie の名前。</param>
    /// <param name="cookiePolicyOptions">Cookie ポリシー オプション。</param>
    /// <param name="context">HTTP コンテキスト。</param>
    /// <returns>アプリケーション構成設定およびプログラムで設定した CookiePolicy を元に作成した <see cref="CookieOptions"/> のインスタンス。</returns>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="cookieName"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="cookiePolicyOptions"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="context"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public CookieOptions CreateCookieOptions(string cookieName, CookiePolicyOptions cookiePolicyOptions, HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(cookiePolicyOptions);
        ArgumentNullException.ThrowIfNull(context);

        if (string.IsNullOrWhiteSpace(cookieName))
        {
            throw new ArgumentException(Messages.InvalidCookieName, nameof(cookieName));
        }

        var cookieBuilder = new CookieBuilder
        {
            Name = cookieName,
            MaxAge = TimeSpan.FromDays(1),
            Expiration = TimeSpan.FromDays(1),
            HttpOnly = cookiePolicyOptions.HttpOnly == HttpOnlyPolicy.Always,
            SecurePolicy = cookiePolicyOptions.Secure,
            SameSite = cookiePolicyOptions.MinimumSameSitePolicy,
        };

        var cookieSetting = this.webServerOptions.CookieSettings.FirstOrDefault(c => c.CookieName == cookieName);
        if (cookieSetting is not null)
        {
            cookieBuilder.MaxAge = TimeSpan.FromDays(cookieSetting.ExpiredDays);
            cookieBuilder.Expiration = TimeSpan.FromDays(cookieSetting.ExpiredDays);

            if (!string.IsNullOrWhiteSpace(cookieSetting.Domain))
            {
                cookieBuilder.Domain = cookieSetting.Domain.Trim();
            }

            if (!string.IsNullOrWhiteSpace(cookieSetting.Path))
            {
                cookieBuilder.Path = cookieSetting.Path.Trim();
            }
        }

        return cookieBuilder.Build(context);
    }
}
