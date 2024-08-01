using Dressca.ApplicationCore.Auth;

namespace Dressca.Web.Admin;

/// <summary>
/// ユーザーのレポジトリーの実装クラス。
/// </summary>
/// <remarks>
/// サンプルのため、必ず管理者のユーザーを返却します。
/// </remarks>
public class EfUserRepository : IUserRepository
{
    /// <inheritdoc/>
    public User? FindUser(string userName, string password)
    {
        return new User
        { Name = "admin@example.com", Role = "Administrator" };
    }
}
