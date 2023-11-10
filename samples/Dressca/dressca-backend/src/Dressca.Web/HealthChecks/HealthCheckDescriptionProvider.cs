using System.Reflection;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dressca.Web.HealthChecks;

/// <summary>
///  ヘルスチェック用に <see cref="IApiDescriptionProvider"/> を実装したクラス。
/// </summary>
public class HealthCheckDescriptionProvider : IApiDescriptionProvider
{
    /// <summary>
    ///  ヘルスチェックAPIの相対パス。
    /// </summary>
    public static readonly string HealthCheckRelativePath = "api/health";

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
            ActionName = HealthCheckRelativePath,
            Parameters = new List<ParameterDescriptor>(),
            ControllerTypeInfo = new TypeDelegator(typeof(string)),
        };

        var getApiDescription = new ApiDescription
        {
            ActionDescriptor = actionDescriptor,
            HttpMethod = HttpMethods.Get,
            RelativePath = HealthCheckRelativePath,
        };

        var headApiDescription = new ApiDescription
        {
            ActionDescriptor = actionDescriptor,
            HttpMethod = HttpMethods.Head,
            RelativePath = HealthCheckRelativePath,
        };

        var normalApiResponseType = new ApiResponseType
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

        var errorApiResponseType = new ApiResponseType
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
            StatusCode = StatusCodes.Status503ServiceUnavailable,
        };

        getApiDescription.SupportedResponseTypes.Add(normalApiResponseType);
        getApiDescription.SupportedResponseTypes.Add(errorApiResponseType);
        headApiDescription.SupportedResponseTypes.Add(normalApiResponseType);
        headApiDescription.SupportedResponseTypes.Add(errorApiResponseType);

        context.Results.Add(getApiDescription);
        context.Results.Add(headApiDescription);
    }
}
