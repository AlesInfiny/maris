using Dressca.ApplicationCore.Authorization;
using Dressca.Web.Admin.Dto.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Dressca.Web.Admin.Controllers;

/// <summary>
/// ユーザーの情報にアクセスするコントローラー。
/// </summary>
[Route("api/users")]
[ApiController]
[Produces("application/json")]
public class UsersController : Controller
{
    private readonly IUserStore userStore;

    /// <summary>
    /// コンストラクター。
    /// </summary>
    /// <param name="userStore">ユーザー情報のストア。</param>
    public UsersController(IUserStore userStore)
    {
        this.userStore = userStore;
    }

    /// <summary>
    /// ログイン中のユーザーの情報を取得します。
    /// </summary>
    /// <returns>ユーザーの情報。</returns>
    [HttpGet]
    [ProducesResponseType(typeof(GetLoginUserResponse), StatusCodes.Status200OK)]
    [OpenApiOperation("getLoginUser")]
    [Authorize]
    public IActionResult GetLoginUser()
    {
        var response = new
        {
            UserName = this.userStore.LoginUserName(),
            Role = this.userStore.LoginUserRole(),
        };
        return this.Ok(response);
    }
}
