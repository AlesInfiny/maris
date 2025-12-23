using Microsoft.AspNetCore.Components.Authorization;

namespace DresscaCMS.Web.Extensions;

/// <summary>
///  <see cref="AuthenticationStateProvider"/> の拡張メソッドを提供します。
/// </summary>
public static class AuthenticationStateProviderExtensions
{
    /// <summary>
    ///  現在ログイン中のユーザー名を取得します。
    /// </summary>
    /// <param name="authenticationStateProvider">認証状態プロバイダー。</param>
    /// <returns>ユーザー名。取得できない場合は "不明なユーザー"。</returns>
    public static async Task<string> GetCurrentUserNameAsync(
        this AuthenticationStateProvider authenticationStateProvider)
    {
        ArgumentNullException.ThrowIfNull(authenticationStateProvider);

        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity?.IsAuthenticated == true)
        {
            return user.Identity.Name ?? "不明なユーザー";
        }

        return "不明なユーザー";
    }
}
