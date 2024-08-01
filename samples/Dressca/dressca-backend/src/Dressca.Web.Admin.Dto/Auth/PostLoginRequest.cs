using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Admin.Dto.Auth;

/// <summary>
/// 認証処理のリクエストデータを表します。
/// </summary>
public class PostLoginRequest
{
    /// <summary>
    /// ユーザー名。
    /// </summary>
    [Required]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// パスワード。
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
}
