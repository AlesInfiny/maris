using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Dressca.Web.Configuration;

/// <summary>
///  アプリケーション構成設定の WebServerOptions です。
/// </summary>
public class WebServerOptions
{
    /// <summary>
    ///  <see cref="WebServerOptions"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public WebServerOptions()
    {
        this.AllowedOrigins = [];
        this.ApplicationCookieSettings = new List<ApplicationCookieSetting>();
    }

    /// <summary>
    ///  許可するオリジンを取得または設定します。
    /// </summary>
    public string[] AllowedOrigins { get; set; }

    /// <summary>
    /// アプリケーションで使用する Cookie の設定を取得または設定します。
    /// </summary>
    [ValidateEnumeratedItems]
    public IList<ApplicationCookieSetting> ApplicationCookieSettings { get; set; }

    /// <summary>
    /// アプリケーション構成設定を元に <see cref="CookieOptions"/> を作成します。
    /// </summary>
    /// <param name="cookieName">Cookie の名前。</param>
    /// <param name="timeProvider">日時のプロバイダ。通常はシステム日時。</param>
    /// <param name="cookiePolicyOptions">Cookie ポリシー オプション。</param>
    /// <returns>アプリケーション構成設定を元に作成した <see cref="CookieOptions"/> のインスタンス。</returns>
    /// <exception cref="ArgumentNullException">
    ///   <list type="bullet">
    ///     <item><paramref name="cookieName"/> が <see langword="null"/> です。</item>
    ///     <item><paramref name="timeProvider"/> が <see langword="null"/> です。</item>
    ///     <item><paramref name="cookiePolicyOptions"/> が <see langword="null"/> です。</item>
    ///   </list>
    /// </exception>
    public CookieOptions CreateCookieOptions(string cookieName, TimeProvider timeProvider, CookiePolicyOptions cookiePolicyOptions)
    {
        ArgumentNullException.ThrowIfNull(cookieName);
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentNullException.ThrowIfNull(cookiePolicyOptions);

        var options = new CookieOptions
        {
            Expires = timeProvider.GetUtcNow().AddDays(1),
        };
        ApplyCookiePolicy(options, cookiePolicyOptions);

        var cookieSetting = this.ApplicationCookieSettings.Where(c => c.CookieName == cookieName).FirstOrDefault();

        if (cookieSetting != null)
        {
            options.Expires = timeProvider.GetUtcNow().AddDays(cookieSetting.ExpiredDays);

            if (!string.IsNullOrEmpty(cookieSetting.Domain))
            {
                options.Domain = cookieSetting.Domain;
            }
        }

        return options;
    }

    private static void ApplyCookiePolicy(CookieOptions options, CookiePolicyOptions cookiePolicyOptions)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(cookiePolicyOptions);

        if (cookiePolicyOptions.MinimumSameSitePolicy != SameSiteMode.Unspecified)
        {
            options.SameSite = ApplyMinimumSameSitePolicy(options.SameSite, cookiePolicyOptions.MinimumSameSitePolicy);
        }

        if (cookiePolicyOptions.HttpOnly == HttpOnlyPolicy.Always)
        {
            options.HttpOnly = true;
        }

        if (cookiePolicyOptions.Secure == CookieSecurePolicy.Always)
        {
            options.Secure = true;
        }
    }

    private static SameSiteMode ApplyMinimumSameSitePolicy(SameSiteMode current, SameSiteMode minimum)
    {
        if (minimum == SameSiteMode.Unspecified)
        {
            return current;
        }

        if (current == SameSiteMode.Unspecified)
        {
            return minimum;
        }

        return GetSameSiteRank(current) < GetSameSiteRank(minimum) ? minimum : current;
    }

    private static int GetSameSiteRank(SameSiteMode mode)
    {
        return mode switch
        {
            SameSiteMode.None => 0,
            SameSiteMode.Lax => 1,
            SameSiteMode.Strict => 2,
            _ => -1,
        };
    }
}
