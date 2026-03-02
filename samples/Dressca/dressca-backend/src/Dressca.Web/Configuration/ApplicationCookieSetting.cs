using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Configuration;

/// <summary>
/// アプリケーションで使用する Cookie の設定を表すクラスです。
/// </summary>
public class ApplicationCookieSetting
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
}
