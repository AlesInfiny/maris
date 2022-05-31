﻿using Dressca.SystemCommon;
using Dressca.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
            this.logger.LogInformation(businessEx, Messages.BusinessExceptionHandled);
            var errors = businessEx.GetErrors();
            foreach (var (errorCode, errorMessage) in errors)
            {
                context.ModelState.AddModelError(errorCode, errorMessage);
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
