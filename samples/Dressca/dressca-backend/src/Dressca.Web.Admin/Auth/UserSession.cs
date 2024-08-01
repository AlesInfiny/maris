using Dressca.ApplicationCore.Auth;

namespace Dressca.Web.Admin.Auth;

/// <summary>
/// ユーザーのセッションのHTTPContextによる実装クラス。
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
    public User? FindUser(string userName, string password)
    {
        return new User
        { Name = "管理者", Role = "Administrator" };
    }
}


