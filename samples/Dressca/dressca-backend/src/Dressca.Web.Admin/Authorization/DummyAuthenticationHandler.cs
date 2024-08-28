using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace DummyAuthentication.Web;

/// <summary>
/// 開発環境用の認証ハンドラー。
/// </summary>
internal class DummyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <param name="logger"></param>
    /// <param name="encoder"></param>
    public DummyAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    /// <inheritdoc/>
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // ダミーのユーザー名とロール名を設定します。
        Claim[] claims = [
            new Claim(ClaimTypes.Name, "dummy_user"),
            new Claim(ClaimTypes.Role, "TEST")
        ];
        var identity = new ClaimsIdentity(claims, this.Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, this.Scheme.Name);

        // 認証は常に成功させます。
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
