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
        this.CookieSettings = new List<CookieSetting>();
    }

    /// <summary>
    ///  許可するオリジンを取得または設定します。
    /// </summary>
    public string[] AllowedOrigins { get; set; }

    /// <summary>
    /// アプリケーションで使用する Cookie の設定を取得または設定します。
    /// </summary>
    [ValidateEnumeratedItems]
    public IList<CookieSetting> CookieSettings { get; set; }
}
