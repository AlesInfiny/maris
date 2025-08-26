using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Dto.ServerTime;

/// <summary>
///  サーバー時間のレスポンスデータを表します。
/// </summary>
public class ServerTimeResponse
{
    /// <summary>
    ///  サーバー時間を取得または設定します。
    /// </summary>
    [Required]
    public string ServerTime { get; set; } = string.Empty;
}
