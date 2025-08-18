using Dressca.Web.Dto.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using NSwag.Annotations;

namespace Dressca.Web.Controllers;

/// <summary>
///  ユーザーに関連する情報にアクセスする API コントローラーです。
/// </summary>
[Route("api/users")]
[ApiController]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    /// <summary>
    ///  認証済みユーザーのユーザー情報を取得します。
    /// </summary>
    /// <returns>ユーザー情報。</returns>
    /// <response code="200">成功。</response>
    /// <response code="401">認証されていない。</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    [OpenApiOperation("getUser")]
    public IActionResult GetUser()
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
