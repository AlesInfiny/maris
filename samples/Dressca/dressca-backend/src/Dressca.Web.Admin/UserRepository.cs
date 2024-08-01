using Dressca.ApplicationCore.Auth;

namespace Dressca.Web.Admin;

/// <summary>
/// ユーザーのレポジトリーのHTTPContextによる実装クラス。
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly IHttpContextAccessor httpContextAccessor;

    /// <summary>
    /// コンストラクター。
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    public UserRepository(IHttpContextAccessor httpContextAccessor)
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


