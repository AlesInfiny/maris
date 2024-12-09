using Dressca.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dressca.Web.Runtime;

/// <summary>
///  <see cref="DbUpdateConcurrencyException"/> を HTTP 409 のレスポンスに変換する
///  <see cref="IExceptionFilter"/> の基底クラスです。
/// </summary>
public abstract class DbUpdateConcurrencyExceptionFilterBase : IExceptionFilter
{
    private readonly ILogger logger;

    /// <summary>
    ///  <see cref="DbUpdateConcurrencyExceptionFilterBase"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    protected DbUpdateConcurrencyExceptionFilterBase(ILogger<DbUpdateConcurrencyExceptionFilterBase> logger)
        => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc/>
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is DbUpdateConcurrencyException dbUpdateConcurrencyEx)
        {
            this.logger.LogInformation(Events.DbUpdateConcurrencyOccurred, dbUpdateConcurrencyEx, LogMessages.DbUpdateConcurrencyOccurred);

            var problemDetail = this.CreateProblemDetails(context);
            context.Result = new ConflictObjectResult(problemDetail);
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
