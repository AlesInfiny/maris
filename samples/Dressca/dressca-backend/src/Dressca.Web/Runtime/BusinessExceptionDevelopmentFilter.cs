using System.Net;
using Dressca.SystemCommon;
using Dressca.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Dressca.Web.Runtime;

/// <summary>
///  <see cref="BusinessException"/> を HTTP 400 のレスポンスに変換する
///  <see cref="BusinessExceptionFilterBase"/> の実装クラスです。
///  開発環境で利用することを想定しています。
/// </summary>
public class BusinessExceptionDevelopmentFilter : BusinessExceptionFilterBase
{
    private readonly ProblemDetailsFactory problemDetailsFactory;

    /// <summary>
    ///  <see cref="BusinessExceptionDevelopmentFilter"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="problemDetailsFactory">RFC準拠の問題詳細オブジェクトを構築するためのファクトリー。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="problemDetailsFactory"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public BusinessExceptionDevelopmentFilter(
        ProblemDetailsFactory problemDetailsFactory,
        ILogger<BusinessExceptionDevelopmentFilter> logger)
        : base(logger)
        => this.problemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(problemDetailsFactory));

    /// <inheritdoc/>
    protected override ProblemDetails CreateProblemDetails(ExceptionContext context)
    {
        // 開発用のフィルターでは例外のスタックトレースも返却する。
        ArgumentNullException.ThrowIfNull(context);
        var problemDetails = this.problemDetailsFactory.CreateValidationProblemDetails(
                context.HttpContext,
                context.ModelState,
                statusCode: (int)HttpStatusCode.BadRequest,
                title: Messages.BusinessExceptionHandled,
                detail: context.Exception.ToString());

        if (context.Exception is BusinessException businessEx)
        {
            // 暫定の実装として、1つ目のBusinessErrorのexceptionIdとexceptionValuesを設定
            problemDetails.Extensions.Add("exceptionId", businessEx.GetBusinessErrors.FirstOrDefault()?.ExceptionId ?? string.Empty);
            problemDetails.Extensions.Add("exceptionValues", businessEx.GetBusinessErrors.FirstOrDefault()?.ErrorMessages.FirstOrDefault()?.ErrorMessageValues ?? []);
        }

        return problemDetails;
    }
}
