using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dressca.Web.Authorization;

/// <summary>
///  開発環境用の認証ハンドラーです。
/// </summary>
public class DummyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    /// <summary>
    ///  <see cref="DummyAuthenticationHandler"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="options">認証スキームのオプション。</param>
    /// <param name="logger">ロガー。</param>
    /// <param name="encoder">URLのエンコーダー。</param>
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
            new Claim(ClaimTypes.Role, "Admin")
        ];
        var identity = new ClaimsIdentity(claims, this.Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, this.Scheme.Name);

        // 認証は常に成功させます。
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
