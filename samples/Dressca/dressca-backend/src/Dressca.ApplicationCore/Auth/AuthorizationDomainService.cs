using Microsoft.Extensions.Logging;

namespace Dressca.ApplicationCore.Auth;

/// <summary>
/// 認証・認可のサービス。
/// </summary>
public class AuthorizationDomainService : IAuthorizationDomainService
{
    private readonly IUserSession session;
    private readonly ILogger<AuthorizationDomainService> logger;

    /// <summary>
    /// コンストラクター。
    /// </summary>
    /// <param name="session">ユーザーセッション。</param>
    /// <param name="logger">ロガー。</param>
    public AuthorizationDomainService(IUserSession session, ILogger<AuthorizationDomainService> logger)
    {
        this.session = session;
        this.logger = logger;
    }

    /// <inheritdoc/>
    public string GetLoginUserName()
    {
        return this.session.LoginUserName();
    }

    /// <inheritdoc/>
    public string GetLoginUserRole()
    {
        return this.session.LoginUserRole();
    }

    /// <inheritdoc/>
    public bool IsInRole(string role)
    {
        return this.session.IsInRole(role);
    }
}
