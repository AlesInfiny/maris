namespace Dressca.Web.Extensions;

/// <summary>
/// HTTP レスポンスヘッダーにセキュリティ関連の設定を追加するミドルウェアです。
/// </summary>
public class HttpSecurityHeadersMiddleware
{
    private readonly RequestDelegate next;

    /// <summary>
    /// <see cref="HttpSecurityHeadersMiddleware"/> クラスの新しいインスタンスを生成します。
    /// </summary>
    /// <param name="next">HTTP 要求を処理できる delegate</param>
    public HttpSecurityHeadersMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    /// <summary>
    /// <see cref="HttpSecurityHeadersMiddleware"/> のメイン ロジックを実行します。
    /// </summary>
    /// <param name="context">HTTP コンテキスト</param>
    /// <returns>パイプラインの次の処理</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.OnStarting(() =>
        {
            // コンテンツタイプを誤認識しないよう、HTTPレスポンスヘッダに「X-Content-Type-Options: nosniff」の設定を追加
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";

            // クリックジャッキング攻撃への対策として、HTTP レスポンスヘッダに、「Content-Security-Policy」を「frame-ancestors 'none'」に設定
            context.Response.Headers.ContentSecurityPolicy = "frame-ancestors 'none'";

            // レガシーブラウザー向けのクリックジャッキング攻撃への対策として、HTTP レスポンスヘッダに、「X-FRAME-OPTIONS」を「DENY」に設定
            context.Response.Headers["X-Frame-Options"] = "DENY";

            return Task.CompletedTask;
        });

        await this.next(context);
    }
}
