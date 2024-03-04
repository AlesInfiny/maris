using System.Security.Claims;
using Dressca.Web.Dto.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

namespace Dressca.Web.Controllers;

/// <summary>
/// ユーザーに関連する情報にアクセスする API コントローラーです。
/// </summary>
[Route("api/users")]
[ApiController]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> logger;

    /// <summary>
    ///  <see cref="UsersController"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="logger">ロガー。</param>
    public UsersController(ILogger<UsersController> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// ログイン中のユーザー情報を取得します。
    /// </summary>
    /// <returns>ユーザー情報。</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<IActionResult> GetUser()
    {
        var userId = ClaimsPrincipalExtensions.GetNameIdentifierId(this.User);

        if (string.IsNullOrEmpty(userId))
        {
            return this.Unauthorized();
        }

        return this.Ok(new UserResponse { UserName = "山田　太郎", UserId = userId });
    }
}
