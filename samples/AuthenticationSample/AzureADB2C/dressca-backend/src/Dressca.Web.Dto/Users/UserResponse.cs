using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Dto.Users;

/// <summary>
/// ユーザー情報のレスポンスデータを表します。
/// </summary>
public class UserResponse
{
    /// <summary>
    /// ユーザー名を取得または設定します。
    /// </summary>
    [Required]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// ユーザー ID を取得または設定します。
    /// </summary>
    [Required]
    public string UserId { get; set; } = string.Empty;
}
