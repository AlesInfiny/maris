using Dressca.ApplicationCore.Auth;
using Dressca.Web.Admin.Dto;
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
    private readonly IAuthorizationDomainService authorizationService;

    /// <summary>
    /// コンストラクター。
    /// </summary>
    /// <param name="authorizationService">認可サービス。</param>
    public UsersController(IAuthorizationDomainService authorizationService)
    {
        this.authorizationService = authorizationService;
    }

    /// <summary>
    /// ログイン中のユーザーの情報を取得します。
    /// </summary>
    /// <returns>ユーザーの情報。</returns>
    [HttpGet]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [OpenApiOperation("getLoginUser")]
    [Authorize]
    public IActionResult getLoginUser()
    {
        var response = new
        {
            UserName = this.authorizationService.GetLoginUserName(),
            Role = this.authorizationService.GetLoginUserRole()
        };
        return this.Ok(response);
    }
}
