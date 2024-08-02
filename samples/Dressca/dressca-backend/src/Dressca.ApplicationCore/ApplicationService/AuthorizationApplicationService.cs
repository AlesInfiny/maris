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
    /// ユーザーが特定のロールに属しているか確認します。
    /// </summary>
    /// <param name="role">ロール。</param>
    /// <returns>属しているかどうか。</returns>
    public bool IsInRole(string role)
    {
        return this.session.IsInRole(role);
    }
}
