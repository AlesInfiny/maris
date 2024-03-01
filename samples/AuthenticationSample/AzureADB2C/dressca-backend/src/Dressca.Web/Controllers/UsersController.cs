using System.Security.Claims;
using Dressca.Web.Dto.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

namespace Dressca.Web.Controllers;

[Route("api/users")]
[ApiController]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> logger;

    public UsersController(ILogger<UsersController> logger)
    {
        this.logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public async Task<IActionResult> GetByUserHomeAccountId()
    {
        return this.Ok(new UserResponse { UserName = "山田　太郎" });
    }
}
