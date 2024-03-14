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
public class UsersController(ILogger<UsersController> logger) : ControllerBase
{
    private readonly ILogger<UsersController> logger = logger;
    private readonly TimeProvider timeProvider;

    /// <summary>
    /// ログイン中のユーザー情報を取得します。
    /// </summary>
    /// <returns>ユーザー情報。</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<IActionResult> GetUserAsync()
    {
        var userId = this.User.GetNameIdentifierId();

        if (string.IsNullOrEmpty(userId))
        {
            // [Authorize]属性があるためuserIdは取得できるはずで、通常このifには入らない。
            return this.Unauthorized();
        }

        return this.Ok(new UserResponse { UserId = userId });
    }
}
