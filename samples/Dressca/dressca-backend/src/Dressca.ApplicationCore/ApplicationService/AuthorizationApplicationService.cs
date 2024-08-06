using Dressca.ApplicationCore.Auth;
using Microsoft.Extensions.Logging;

namespace Dressca.ApplicationCore.ApplicationService;

/// <summary>
/// 認可のアプリケーションサービス。
/// </summary>
public class AuthorizationApplicationService
{
    private readonly IUserSession session;
    private readonly ILogger<AuthorizationApplicationService> logger;

    /// <summary>
    /// コンストラクター。
    /// </summary>
    /// <param name="session">ユーザーセッション。</param>
    /// <param name="logger">ロガー。</param>
    public AuthorizationApplicationService(IUserSession session, ILogger<AuthorizationApplicationService> logger)
    {
        this.session = session;
        this.logger = logger;
    }

    /// <summary>
    /// ログイン中のユーザー名を取得します。
    /// </summary>
    /// <returns>ユーザー名。</returns>
    public string GetLoginUserName()
    {
        return this.session.LoginUserName();
    }

    /// <summary>
    /// ログイン中のユーザーのロールの一覧を取得します。
    /// </summary>
    /// <returns>ログイン中のユーザーのロール。</returns>
    public IReadOnlyCollection<string> GetLoginUserRoles()
    {
        return this.session.LoginUserRoles();
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
