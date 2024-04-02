using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Dto.ServerTime;

/// <summary>
/// サーバー時間のレスポンスデータを表します。
/// </summary>
public class ServerTimeResponse
{
    /// <summary>
    /// サーバー時間。
    /// </summary>
    [Required]
    public string ServerTime { get; set; } = string.Empty;
}
