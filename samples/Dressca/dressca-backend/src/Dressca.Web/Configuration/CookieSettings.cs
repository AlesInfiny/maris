namespace Dressca.Web.Configuration;

/// <summary>
/// アプリケーション構成設定の Cookie オプションです。
/// </summary>
public class CookieSettings
{
    /// <summary>
    /// Cookie の SameSite 属性を取得または設定します。
    /// </summary>
    public SameSiteMode SameSite { get; set; } = SameSiteMode.Strict;

    /// <summary>
    /// Cookie の HttpOnly 属性を取得または設定します。
    /// </summary>
    public bool HttpOnly { get; set; } = true;

    /// <summary>
    /// Cookie の Secure 属性を取得または設定します。
    /// </summary>
    public bool Secure { get; set; } = true;

    /// <summary>
    /// Cookie の Expires に設定する日数を取得または設定します。
    /// </summary>
    public int ExpiredDays { get; set; }

    /// <summary>
    /// Cookie の Domain に設定するドメインを取得または設定します。
    /// </summary>
    public string? Domain { get; set; }
}
