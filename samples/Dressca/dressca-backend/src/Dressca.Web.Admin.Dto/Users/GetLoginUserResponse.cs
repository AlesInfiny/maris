using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Admin.Dto.Users;

/// <summary>
///  ユーザー情報のレスポンスを表します。
/// </summary>
public class GetLoginUserResponse
{
    /// <summary>
    ///  ユーザー名を取得または設定します。
    /// </summary>
    [Required]
    public required string UserName { get; set; }

    /// <summary>
    ///  ロールを取得または設定します。
    /// </summary>
    [Required]
    public required string[] Roles { get; set; }
}
