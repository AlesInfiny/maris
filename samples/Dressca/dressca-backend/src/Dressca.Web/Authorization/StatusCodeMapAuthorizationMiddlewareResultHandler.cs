using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace Dressca.Web.Authorization;

/// <summary>
///  認可ミドルウェアのデフォルト動作を変更するカスタムハンドラーです。
///  デフォルト動作で返却されるステータスコードを変更します。
/// </summary>
public class StatusCodeMapAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

    /// <inheritdoc/>
    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        // 認可の結果が Forbidden の場合、認可の構造をクライアントに知られないように、
        // デフォルトのステータスコード 403 を 404 に詰め替えます。
        if (authorizeResult.Forbidden)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        // デフォルトのハンドラーの実装にフォールバックします。
        await this.defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}
