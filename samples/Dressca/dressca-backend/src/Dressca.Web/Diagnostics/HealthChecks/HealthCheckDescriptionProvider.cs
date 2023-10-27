using System.Reflection;
using Dressca.EfInfrastructure;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dressca.Web.Diagnostics.HealthChecks;

/// <summary>
///  ヘルスチェック用に <see cref="IApiDescriptionProvider"/> を実装したクラス。
/// </summary>
public class HealthCheckDescriptionProvider : IApiDescriptionProvider
{
    private readonly IModelMetadataProvider modelMetadataProvider;

    /// <summary>
    ///  <see cref="HealthCheckDescriptionProvider"/> の新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="modelMetadataProvider">モデルメタデータプロバイダー。</param>
    public HealthCheckDescriptionProvider(IModelMetadataProvider modelMetadataProvider)
    {
        this.modelMetadataProvider = modelMetadataProvider;
    }

    /// <summary>
    ///  <see cref="IApiDescriptionProvider"/> を実装したクラスが処理される順番を表します。
    /// </summary>
    public int Order => -1;

    /// <summary>
    ///  <see cref="ApiDescription"/> を生成あるいは変更します。
    /// </summary>
    /// <param name="context">API 情報を保持するコンテキスト。</param>
    public void OnProvidersExecuting(ApiDescriptionProviderContext context)
    {
    }

    /// <summary>
    ///  <see cref="OnProvidersExecuting(ApiDescriptionProviderContext)"/> の処理が終わった後に呼び出されます。
    /// </summary>
    /// <param name="context">API 情報を保持するコンテキスト。</param>
    public void OnProvidersExecuted(ApiDescriptionProviderContext context)
    {
        var actionDescriptor = new ControllerActionDescriptor
        {
            ControllerName = "HealthChecks",
            ActionName = "api/health",
            Parameters = new List<ParameterDescriptor>(),
            ControllerTypeInfo = new TypeDelegator(typeof(string)),
        };

        var apiDescription = new ApiDescription
        {
            ActionDescriptor = actionDescriptor,
            HttpMethod = HttpMethods.Get,
            RelativePath = "api/health",
        };

        var apiResponseType = new ApiResponseType
        {
            ApiResponseFormats = new List<ApiResponseFormat>
            {
                new ApiResponseFormat
                {
                    MediaType = "text/plain",
                    Formatter = new StringOutputFormatter(),
                },
            },
            Type = typeof(string),
            StatusCode = StatusCodes.Status200OK,
        };

        apiDescription.SupportedResponseTypes.Add(apiResponseType);
        context.Results.Add(apiDescription);
    }
}
