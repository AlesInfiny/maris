using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Admin.Dto.Users;

/// <summary>
/// ユーザー情報のレスポンスを表します。
/// </summary>
public class GetLoginUserResponse
{
    /// <summary>
    ///  ユーザー名を取得または設定します。
    /// </summary>
    [Required]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    ///  ロールを取得または設定します。
    /// </summary>
    [Required]
    public string Role { get; set; } = string.Empty;
}
