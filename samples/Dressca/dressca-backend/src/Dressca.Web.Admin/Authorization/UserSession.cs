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
    public IReadOnlyCollection<string> LoginUserRoles()
    {
        return this.httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value).ToList();
    }

    /// <inheritdoc/>
    public bool IsInRole(string role)
    {
        return this.httpContextAccessor.HttpContext.User.IsInRole(role);
    }

}


