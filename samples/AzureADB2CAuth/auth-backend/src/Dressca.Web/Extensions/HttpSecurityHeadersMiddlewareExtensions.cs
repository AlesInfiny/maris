namespace Dressca.Web.Extensions;

/// <summary>
/// <see cref="IApplicationBuilder"/> の拡張メソッドを提供します。
/// </summary>
public static class HttpSecurityHeadersMiddlewareExtensions
{
    /// <summary>
    /// <see cref="HttpSecurityHeadersMiddleware"/> を使用します。
    /// </summary>
    /// <param name="builder">アプリケーションの要求（HTTP リクエスト）処理パイプラインを構成（設定）するための仕組みを提供するクラス</param>
    /// <returns><see cref="IApplicationBuilder"/> のインスタンス</returns>
    public static IApplicationBuilder UseSecuritySettings(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HttpSecurityHeadersMiddleware>();
    }
}
