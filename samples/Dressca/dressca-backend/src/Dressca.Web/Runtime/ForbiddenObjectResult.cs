using Microsoft.AspNetCore.Mvc;

namespace Dressca.Web.Runtime;

public class ForbiddenObjectResult : StatusCodeResult
{
    public ForbiddenObjectResult()
        : base(StatusCodes.Status403Forbidden)
    {
    }
}
