using Dressca.ApplicationCore.ApplicationService;
using Dressca.Web.Admin.Dto;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Dressca.Web.Admin.Controllers;

[Route("api/users")]
[ApiController]
[Produces("application/json")]
public class UsersController : Controller
{
    private readonly AuthorizationApplicationService authorizationService;

    public UsersController(AuthorizationApplicationService authorizationService)
    {
        this.authorizationService = authorizationService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [OpenApiOperation("getLoginUser")]
    public IActionResult getLoginUser()
    {
        var response = new UserResponse
        {
            UserName = this.authorizationService.GetLoginUserName(),
            Roles = this.authorizationService.GetLoginUserRoles().ToList()
        };
        return this.Ok(response);
    }
}
