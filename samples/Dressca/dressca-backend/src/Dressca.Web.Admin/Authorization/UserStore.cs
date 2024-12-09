using System.Security.Claims;
using Dressca.ApplicationCore.Authorization;

namespace Dressca.Web.Admin.Authorization;

/// <summary>
/// ユーザーのセッション情報を表すクラスです。
/// </summary>
public class UserStore : IUserStore
{
    /// <summary>
    ///  <see cref="HttpContext"/>の情報にアクセスするためのクラス。
    /// </summary>
    private readonly IHttpContextAccessor httpContextAccessor;

    /// <summary>
    ///   <see cref="UserStore"/>のインスタンスを初期化します。
    /// </summary>
    /// <param name="httpContextAccessor"><see cref="HttpContext"/>の情報にアクセスするためのクラス。</param>
    public UserStore(IHttpContextAccessor httpContextAccessor)
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

        return string.Empty;
    }

    /// <inheritdoc/>
    public string LoginUserRole()
    {
        if (this.httpContextAccessor.HttpContext != null)
        {
            return this.httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).First().Value;
        }

        return string.Empty;
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
