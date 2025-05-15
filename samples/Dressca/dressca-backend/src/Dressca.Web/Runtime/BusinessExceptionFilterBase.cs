using Dressca.SystemCommon;
using Dressca.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Dressca.Web.Runtime;

/// <summary>
///  <see cref="BusinessException"/> を HTTP 400 のレスポンスに変換する
///  <see cref="IExceptionFilter"/> の基底クラスです。
/// </summary>
public abstract class BusinessExceptionFilterBase : IExceptionFilter
{
    private readonly ILogger logger;

    /// <summary>
    ///  <see cref="BusinessExceptionFilterBase"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    protected BusinessExceptionFilterBase(ILogger<BusinessExceptionFilterBase> logger)
        => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc/>
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is BusinessException businessEx)
        {
            this.logger.LogInformation(Events.BusinessExceptionHandled, businessEx, Messages.BusinessExceptionHandled);
            var errors = businessEx.GetBusinessErrors;

            foreach (var error in errors)
            {
                context.ModelState.AddModelError(error.ErrorCode, string.Join(",", error.ErrorMessages));
            }

            var validationProblem = this.CreateProblemDetails(context);
            context.Result = new BadRequestObjectResult(validationProblem);
        }
    }

    /// <summary>
    ///  <see cref="ProblemDetails"/> を生成します。
    /// </summary>
    /// <param name="context">例外フィルターのためのコンテキスト情報。</param>
    /// <returns>問題詳細のオブジェクト。</returns>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="context"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    protected abstract ProblemDetails CreateProblemDetails(ExceptionContext context);
}
