using Dressca.ApplicationCore.Auth;
using Microsoft.Extensions.Logging;

namespace Dressca.ApplicationCore.ApplicationService;

/// <summary>
/// 認証・認可のアプリケーションサービス。
/// </summary>
public class AuthApplicationService
{
    private readonly IUserRepository repository;
    private readonly ILogger<AuthApplicationService> logger;

    /// <summary>
    /// コンストラクター。
    /// </summary>
    /// <param name="repository">ユーザーレポジトリ。</param>
    /// <param name="logger">ロガー。</param>
    public AuthApplicationService(IUserRepository repository, ILogger<AuthApplicationService> logger)
    {
        this.repository = repository;
        this.logger = logger;
    }

    /// <summary>
    /// ユーザーを認証します。
    /// </summary>
    /// <param name="userName">ユーザー名。</param>
    /// <param name="password">パスワード。</param>
    /// <returns>ユーザー。</returns>
    public User? AuthenticateUser(string userName, string password)
    {
        var user = this.repository.FindUser(userName, password);
        return user;
    }
}
