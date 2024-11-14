using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Dressca.Web.Controllers;

/// <summary>
///  エラーの情報にアクセスする API コントローラーです。
///  このコントローラーは、例外ハンドラーで例外を検知したときに呼び出されることを想定しています。
/// </summary>
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    /// <summary>
    ///  開発環境におけるエラー情報取得のためのルートパスのリテラル値 ( /error-development ) です。
    /// </summary>
    private const string DevelopMentErrorRouteLiteral = "/error-development";

    /// <summary>
    ///  実行環境におけるエラー情報取得のためのルートパスのリテラル値 ( /error ) です。
    /// </summary>
    private const string ErrorRouteLiteral = "/error";

    /// <summary>
    ///  開発環境におけるエラー情報取得のためのルートパス（ /error-development ）を取得します。
    /// </summary>
    public static string DevelopmentErrorRoute => DevelopMentErrorRouteLiteral;

    /// <summary>
    ///  実行環境におけるエラー情報取得のためのルートパス ( /error )を取得します。
    /// </summary>
    public static string ErrorRoute => ErrorRouteLiteral;

    /// <summary>
    ///  開発環境でのエラーレスポンスを取得します。
    /// </summary>
    /// <param name="hostEnvironment">環境の情報。</param>
    /// <returns>エラーの詳細情報。</returns>
    [Route(DevelopMentErrorRouteLiteral)]
    public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
    {
        if (!hostEnvironment.IsDevelopment())
        {
            return this.NotFound();
        }

        var exceptionHandlerFeature = this.HttpContext.Features.Get<IExceptionHandlerFeature>();
        return this.Problem(
            detail: exceptionHandlerFeature?.Error.StackTrace,
            title: exceptionHandlerFeature?.Error.Message);
    }

    /// <summary>
    ///  実行環境でのエラーレスポンスを取得します。
    /// </summary>
    /// <returns>エラーの情報。</returns>
    [Route(ErrorRouteLiteral)]
    public IActionResult HandleError() => this.Problem();
}
