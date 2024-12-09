using System.Net;
using Dressca.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dressca.Web.Runtime;

/// <summary>
///  <see cref="DbUpdateConcurrencyException"/> を HTTP 409 のレスポンスに変換する
///  <see cref="DbUpdateConcurrencyExceptionFilter"/> の実装クラスです。
///  開発環境で利用することを想定しています。
/// </summary>
public class DbUpdateConcurrencyExceptionDevelopmentFilter : DbUpdateConcurrencyExceptionFilterBase
{
    private readonly ProblemDetailsFactory problemDetailsFactory;

    /// <summary>
    ///  <see cref="DbUpdateConcurrencyExceptionFilter"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="problemDetailsFactory">>RFC準拠の問題詳細オブジェクトを構築するためのファクトリー。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="problemDetailsFactory"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public DbUpdateConcurrencyExceptionDevelopmentFilter(
        ProblemDetailsFactory problemDetailsFactory,
        ILogger<DbUpdateConcurrencyExceptionDevelopmentFilter> logger)
    : base(logger)
    => this.problemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(problemDetailsFactory));

    /// <inheritdoc/>
    protected override ProblemDetails CreateProblemDetails(ExceptionContext context)
    {
        // 開発環境用のフィルターでは例外のスタックトレースも返却します。
        ArgumentNullException.ThrowIfNull(context);
        return this.problemDetailsFactory.CreateProblemDetails(
                context.HttpContext,
                statusCode: (int)HttpStatusCode.Conflict,
                title: Messages.DbUpdateConcurrencyOccurred,
                detail: context.Exception.ToString());
    }
}
