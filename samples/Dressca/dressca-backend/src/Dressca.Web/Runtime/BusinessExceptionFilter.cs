using System.Net;
using Dressca.SystemCommon;
using Dressca.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Dressca.Web.Runtime;

/// <summary>
///  <see cref="BusinessException"/> を HTTP 400 のレスポンスに変換する
///  <see cref="BusinessExceptionFilterBase"/> の実装クラスです。
///  本番環境で利用することを想定しています。
/// </summary>
public class BusinessExceptionFilter : BusinessExceptionFilterBase
{
    private readonly ProblemDetailsFactory problemDetailsFactory;

    /// <summary>
    ///  <see cref="BusinessExceptionFilter"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="problemDetailsFactory">RFC 準拠の問題詳細オブジェクトを構築するためのファクトリー。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="problemDetailsFactory"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public BusinessExceptionFilter(
        ProblemDetailsFactory problemDetailsFactory,
        ILogger<BusinessExceptionFilter> logger)
        : base(logger)
        => this.problemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(problemDetailsFactory));

    /// <inheritdoc/>
    protected override ProblemDetails CreateProblemDetails(ExceptionContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return this.problemDetailsFactory.CreateValidationProblemDetails(
                context.HttpContext,
                context.ModelState,
                statusCode: (int)HttpStatusCode.BadRequest,
                title: Messages.BusinessExceptionHandled);
    }
}
