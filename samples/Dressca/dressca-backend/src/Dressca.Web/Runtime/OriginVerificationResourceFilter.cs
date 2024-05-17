using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace Dressca.Web.Runtime;

/// <summary>
/// リクエストの Origin ヘッダーの値が許可されたオリジンであるかを確認するフィルターです。
/// 許可されたオリジンは構成設定から取得します。
/// </summary>
public class OriginVerificationResourceFilter : IResourceFilter
{
    private readonly IConfiguration config;

    /// <summary>
    /// <see cref="OriginVerificationResourceFilter"/> クラスの新しいインタンスを初期化します。
    /// </summary>
    /// <param name="config">アプリケーションの構成設定。</param>
    public OriginVerificationResourceFilter(IConfiguration config)
    {
        this.config = config;
    }

    /// <inheritdoc/>
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
    }

    /// <inheritdoc/>
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        var origin = context.HttpContext.Request.Headers["Origin"];

        if (origin.IsNullOrEmpty())
        {
            // GET または HEAD の場合、 Origin は設定されないので処理を終了する。
            if (context.HttpContext.Request.Method == "GET" ||
                context.HttpContext.Request.Method == "HEAD")
            {
                return;
            }

            // メソッドが GET および HEAD でない場合、 Origin が設定されていないリクエストには
            // 403 Forbidden を返す。
            context.Result = new ForbiddenObjectResult();
            return;
        }

        var section = this.config.GetSection("UserSettings:AllowedOrigins");

        // アプリケーション構成設定にオリジンが設定されていない場合は処理を終了する。
        if (section == null)
        {
            return;
        }

        var origins = section.Get<string[]>();

        if (origins == null || origins.Length == 0)
        {
            return;
        }

        if (origins.Contains<string>(origin!))
        {
            return;
        }

        context.Result = new ForbiddenObjectResult();
    }
}
