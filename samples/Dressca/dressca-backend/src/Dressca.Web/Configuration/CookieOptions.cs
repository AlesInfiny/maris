namespace Dressca.Web.Configuration;

/// <summary>
/// アプリケーション構成設定の Cookie オプションです。
/// </summary>
public class CookieOptions
{
    /// <summary>
    /// Cookie の SameSite 属性を取得または設定します。
    /// </summary>
    public string SameSite { get; set; } = string.Empty;

    /// <summary>
    /// Cookie の HttpOnly 属性を取得または設定します。
    /// </summary>
    public bool HttpOnly { get; set; }

    /// <summary>
    /// Cookie の Secure 属性を取得または設定します。
    /// </summary>
    public bool Secure { get; set; }

    /// <summary>
    /// Cookie の Expires に設定する日数を取得または設定します。
    /// </summary>
    public int ExpiredDays { get; set; }

    /// <summary>
    /// Cookie の Domain に設定するドメインを取得または設定します。
    /// </summary>
    public string Domain { get; set; } = string.Empty;
}
