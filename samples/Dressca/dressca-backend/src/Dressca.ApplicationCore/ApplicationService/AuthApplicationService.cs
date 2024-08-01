using Dressca.ApplicationCore.Auth;
using Microsoft.Extensions.Logging;

namespace Dressca.ApplicationCore.ApplicationService;

/// <summary>
/// 認証・認可のアプリケーションサービス。
/// </summary>
public class AuthApplicationService
{
    private readonly IUserRepository repository;
    private readonly IUserSession session;
    private readonly ILogger<AuthApplicationService> logger;

    /// <summary>
    /// コンストラクター。
    /// </summary>
    /// <param name="repository">ユーザーレポジトリ。</param>
    /// <param name="session">ユーザーセッション。</param>
    /// <param name="logger">ロガー。</param>
    public AuthApplicationService(IUserRepository repository, IUserSession session, ILogger<AuthApplicationService> logger)
    {
        this.repository = repository;
        this.session = session;
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

    /// <summary>
    /// ログイン中のユーザーのロールを取得します。
    /// </summary>
    /// <returns>ロールのリスト。</returns>
    public IReadOnlyCollection<string> GetLoginUserRoles()
    {
        var roles = this.session.LoginUserRoles();
        return roles;
    }

    /// <summary>
    /// ユーザーが特定のロールに属しているか確認します。
    /// </summary>
    /// <param name="role">ロール。</param>
    /// <returns>属しているかどうか。</returns>
    public bool IsInRole(string role)
    {
        return this.session.IsInRole(role);
    }
}
