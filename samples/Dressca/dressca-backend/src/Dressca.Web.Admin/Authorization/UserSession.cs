using System.Security.Claims;
using Dressca.ApplicationCore.Auth;

namespace Dressca.Web.Admin.Authorization;

/// <summary>
/// ユーザーのセッション情報。
/// </summary>
public class UserSession : IUserSession
{
    private readonly IHttpContextAccessor httpContextAccessor;

    /// <summary>
    /// コンストラクター。
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    public UserSession(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc/>
    public string LoginUserName()
    {
        if (this.httpContextAccessor.HttpContext != null)
        {
            return this.httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name).First().Value;
        }
        return String.Empty;
    }

    /// <inheritdoc/>
    public string LoginUserRole()
    {
        if (this.httpContextAccessor.HttpContext != null)
        {
            return this.httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).First().Value;
        }
        return String.Empty;
    }

    /// <inheritdoc/>
    public bool IsInRole(string role)
    {
        if (this.httpContextAccessor.HttpContext != null)
        {
            return this.httpContextAccessor.HttpContext.User.IsInRole(role);
        }
        return false;
    }
}
