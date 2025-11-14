using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Dressca.Web.Configuration;

/// <summary>
///  アプリケーション構成設定の Cookie オプションです。
/// </summary>
public class CookieSettings
{
    /// <summary>
    ///  Cookie の SameSite 属性を取得または設定します。
    /// </summary>
    public SameSiteMode SameSite { get; set; } = SameSiteMode.Strict;

    /// <summary>
    ///  Cookie の HttpOnly 属性を取得または設定します。
    /// </summary>
    public bool HttpOnly { get; set; } = true;

    /// <summary>
    ///  Cookie の Secure 属性を取得または設定します。
    /// </summary>
    public bool Secure { get; set; } = true;

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
    /// </summary>
    /// <param name="timeProvider">日時のプロバイダ。通常はシステム日時。</param>
    /// <returns>アプリケーション構成設定を元に作成した <see cref="CookieOptions"/> のインスタンス。</returns>
    public CookieOptions CreateCookieOptions(TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(nameof(timeProvider));

        var options = new CookieOptions
        {
            SameSite = this.SameSite,
            HttpOnly = this.HttpOnly,
            Secure = this.Secure,
            Expires = timeProvider.GetLocalNow().AddDays(this.ExpiredDays),
        };

        if (!string.IsNullOrEmpty(this.Domain))
        {
            options.Domain = this.Domain;
        }

        return options;
    }
}
