using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace Dressca.Web.Runtime;

/// <summary>
/// リクエストの Origin が許可されたものであるかを確認するフィルターです。
/// </summary>
public class OriginVerificationResourceFilter : IResourceFilter
{
    private readonly IConfiguration config;

    public OriginVerificationResourceFilter(IConfiguration config)
    {
        this.config = config;
    }

    /// <inheritdoc/>
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
    }

    /// <inheritdoc/>
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        var origin = context.HttpContext.Request.Headers["Origin"];

        if (origin.IsNullOrEmpty())
        {
            if (context.HttpContext.Request.Method == "GET" ||
                context.HttpContext.Request.Method == "HEAD")
            {
                return;
            }

            context.Result = new ForbiddenObjectResult();
            return;
        }

        var section = this.config.GetSection("UserSettings:AllowedOrigins");

        if (section == null)
        {
            return;
        }

        var origins = section.Get<string[]>();

        if (origins == null || origins.Length == 0)
        {
            return;
        }

        foreach (var allowedOrigin in origins)
        {
            if (origin == allowedOrigin)
            {
                return;
            }
        }

        context.Result = new ForbiddenObjectResult();
    }
}
