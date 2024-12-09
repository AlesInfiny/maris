using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace Dressca.Web.Authorization;

/// <summary>
/// 認可ミドルウェアのデフォルト動作を変更するカスタムハンドラー。
/// ステータスコードを変更します。
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
        // 認可の結果がForbiddenの場合、 デフォルトのステータスコード403を404に詰め替えます。
        if (authorizeResult.Forbidden)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        // デフォルトのハンドラーの実装にフォールバックします。
        await this.defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}
