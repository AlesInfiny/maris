using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Dressca.Web.Configuration;

/// <summary>
///  アプリケーション構成設定の Cookie オプションです。
/// </summary>
public class CookieSettings
{
    /// <summary>
    ///  Cookie の Expires に設定する日数を取得または設定します。
    /// </summary>
    [Range(1, 100)]
    public int ExpiredDays { get; set; } = 1;

    /// <summary>
    ///  Cookie の Domain に設定するドメインを取得または設定します。
    /// </summary>
    public string? Domain { get; set; }

    /// <summary>
    ///  アプリケーション構成設定を元に <see cref="CookieOptions"/> を作成します。
    ///  必要に応じて <see cref="CookiePolicyOptions"/> によるポリシーを反映します。
    /// </summary>
    /// <param name="timeProvider">日時のプロバイダ。通常はシステム日時。</param>
    /// <param name="cookiePolicyOptions">Cookie ポリシー オプション。</param>
    /// <returns>アプリケーション構成設定を元に作成した <see cref="CookieOptions"/> のインスタンス。</returns>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="timeProvider"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public CookieOptions CreateCookieOptions(TimeProvider timeProvider, IOptions<CookiePolicyOptions>? cookiePolicyOptions)
    {
        return this.CreateCookieOptions(timeProvider, cookiePolicyOptions?.Value);
    }

    /// <summary>
    ///  アプリケーション構成設定を元に <see cref="CookieOptions"/> を作成します。
    ///  必要に応じて <see cref="CookiePolicyOptions"/> によるポリシーを反映します。
    /// </summary>
    /// <param name="timeProvider">日時のプロバイダ。通常はシステム日時。</param>
    /// <param name="cookiePolicyOptions">Cookie ポリシー オプション。</param>
    /// <returns>アプリケーション構成設定を元に作成した <see cref="CookieOptions"/> のインスタンス。</returns>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="timeProvider"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public CookieOptions CreateCookieOptions(TimeProvider timeProvider, CookiePolicyOptions? cookiePolicyOptions)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        var options = new CookieOptions
        {
            SameSite = SameSiteMode.Strict,
            HttpOnly = true,
            Secure = true,
            Expires = timeProvider.GetLocalNow().AddDays(this.ExpiredDays),
        };

        if (!string.IsNullOrEmpty(this.Domain))
        {
            options.Domain = this.Domain;
        }

        if (cookiePolicyOptions is not null)
        {
            ApplyCookiePolicy(options, cookiePolicyOptions);
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
