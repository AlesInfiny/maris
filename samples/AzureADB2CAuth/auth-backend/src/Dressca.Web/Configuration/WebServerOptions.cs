namespace Dressca.Web.Configuration;

/// <summary>
///  アプリケーション構成設定の WebServerOptions です。
/// </summary>
public class WebServerOptions
{
    /// <summary>
    ///  許可するオリジンを取得または設定します。
    /// </summary>
    public string[] AllowedOrigins { get; set; } = [];
}
