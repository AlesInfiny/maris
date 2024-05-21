using Dressca.SystemCommon;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace Dressca.Web.Runtime;

/// <summary>
/// リクエストの Origin ヘッダーの値が許可されたオリジンであるかを確認するフィルターです。
/// 許可されたオリジンはアプリケーション構成設定から取得します。
/// 許可されたオリジンとリクエストの Origin ヘッダーの値が一致しない場合、 403 Forbidden を返します。
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
        config.ThrowIfNull(nameof(config));
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
            context.Result = new ForbiddenCodeResult();
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

        // アプリケーション構成設定にオリジンが設定されている場合、リクエストヘッダーの Origin と
        // 一致することを確認する。
        if (origins.Contains<string>(origin!))
        {
            return;
        }

        context.Result = new ForbiddenCodeResult();
    }
}
