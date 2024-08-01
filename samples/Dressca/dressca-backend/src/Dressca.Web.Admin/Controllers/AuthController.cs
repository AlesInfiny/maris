using System.Security.Claims;
using Dressca.ApplicationCore.ApplicationService;
using Dressca.Web.Admin.Dto.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
namespace Dressca.Web.Admin.Controllers;

/// <summary>
/// 認証・認可のコントローラー。
/// </summary>
[Route("api/auth")]
[ApiController]
public class AuthController : Controller
{
    private readonly AuthApplicationService service;
    private readonly ILogger<AuthController> logger;

    /// <summary>
    /// 認証・認可のコントローラー。
    /// </summary>
    /// <param name="service"></param>
    /// <param name="logger"></param>
    public AuthController(AuthApplicationService service,
        ILogger<AuthController> logger)
    {
        this.service = service;
        this.logger = logger;
    }

    /// <summary>
    /// ログインします。
    /// </summary>
    /// <returns></returns>
    [Route("login")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("login")]
    public async Task<IActionResult> Login(PostLoginRequest postLoginRequest)
    {
        var user = service.AuthenticateUser(postLoginRequest.UserName, postLoginRequest.Password);
        if (user == null)
        {
            return Unauthorized();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Role, user.Role),
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            //AllowRefresh = <bool>,
            // Refreshing the authentication session should be allowed.

            //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            // The time at which the authentication ticket expires. A 
            // value set here overrides the ExpireTimeSpan option of 
            // CookieAuthenticationOptions set with AddCookie.

            //IsPersistent = true,
            // Whether the authentication session is persisted across 
            // multiple requests. When used with cookies, controls
            // whether the cookie's lifetime is absolute (matching the
            // lifetime of the authentication ticket) or session-based.

            //IssuedUtc = <DateTimeOffset>,
            // The time at which the authentication ticket was issued.

            //RedirectUri = <string>
            // The full path or absolute URI to be used as an http 
            // redirect response value.
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        logger.LogDebug("ログインしました。");

        return Ok();
    }

    /// <summary>
    /// ログアウトします。
    /// </summary>
    /// <returns></returns>
    [Route("logout")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {

        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        logger.LogDebug("ログアウトしました。");

        return Ok();
    }
}
