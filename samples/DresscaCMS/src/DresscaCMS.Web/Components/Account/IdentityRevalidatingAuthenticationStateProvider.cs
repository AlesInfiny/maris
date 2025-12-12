using System.Security.Claims;
using DresscaCMS.Authentication.Infrastructures.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DresscaCMS.Web.Components.Account;

/// <summary>
/// サーバー側の認証状態プロバイダーで、接続されたユーザーのセキュリティスタンプを
/// インタラクティブ回線が接続されている間、30分ごとに再検証します。
/// </summary>
/// <param name="loggerFactory">ロガーファクトリ。</param>
/// <param name="scopeFactory">サービススコープファクトリ。</param>
/// <param name="options">Identity設定オプション。</param>
internal sealed class IdentityRevalidatingAuthenticationStateProvider(
        ILoggerFactory loggerFactory,
        IServiceScopeFactory scopeFactory,
        IOptions<IdentityOptions> options)
    : RevalidatingServerAuthenticationStateProvider(loggerFactory)
{
    /// <summary>
    /// 認証状態の再検証間隔を取得します。
    /// </summary>
    /// <value>30分。</value>
    protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

    /// <summary>
    /// 認証状態を非同期で検証します。
    /// </summary>
    /// <param name="authenticationState">検証する認証状態。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>
    /// 認証状態が有効な場合は<c>true</c>、それ以外の場合は<c>false</c>を返すタスク。
    /// </returns>
    /// <remarks>
    /// 最新のデータを取得するために、新しいスコープからユーザーマネージャーを取得します。
    /// </remarks>
    protected override async Task<bool> ValidateAuthenticationStateAsync(
        AuthenticationState authenticationState, CancellationToken cancellationToken)
    {
        // 最新のデータを取得するために、新しいスコープからユーザーマネージャーを取得します。
        await using var scope = scopeFactory.CreateAsyncScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        return await this.ValidateSecurityStampAsync(userManager, authenticationState.User);
    }

    /// <summary>
    /// セキュリティスタンプを非同期で検証します。
    /// </summary>
    /// <param name="userManager">アプリケーションユーザーマネージャー。</param>
    /// <param name="principal">検証するクレームプリンシパル。</param>
    /// <returns>
    /// セキュリティスタンプが一致する場合は<c>true</c>、
    /// ユーザーが存在しない場合は<c>false</c>、
    /// セキュリティスタンプがサポートされていない場合は<c>true</c>を返すタスク。
    /// </returns>
    private async Task<bool> ValidateSecurityStampAsync(UserManager<ApplicationUser> userManager, ClaimsPrincipal principal)
    {
        var user = await userManager.GetUserAsync(principal);
        if (user is null)
        {
            return false;
        }
        else if (!userManager.SupportsUserSecurityStamp)
        {
            return true;
        }
        else
        {
            var principalStamp = principal.FindFirstValue(options.Value.ClaimsIdentity.SecurityStampClaimType);
            var userStamp = await userManager.GetSecurityStampAsync(user);
            return principalStamp == userStamp;
        }
    }
}
