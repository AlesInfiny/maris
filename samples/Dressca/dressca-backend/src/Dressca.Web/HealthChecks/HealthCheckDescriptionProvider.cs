using System.Reflection;
using Microsoft.AspNetCore.Http;
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
    ///  ヘルスチェックAPIの相対パス "api/health" 。
    /// </summary>
    public static readonly string HealthCheckRelativePath = "api/health";

    private readonly IModelMetadataProvider modelMetadataProvider;

    /// <summary>
    ///  <see cref="HealthCheckDescriptionProvider"/> の新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="modelMetadataProvider">モデルメタデータプロバイダー。</param>
    /// <exception cref="ArgumentNullException"><paramref name="modelMetadataProvider"/> が <see langword="null"/> です。</exception>
    public HealthCheckDescriptionProvider(IModelMetadataProvider modelMetadataProvider)
    {
        this.modelMetadataProvider = modelMetadataProvider ?? throw new ArgumentNullException(nameof(modelMetadataProvider));
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
            Parameters = [],
            ControllerTypeInfo = new TypeDelegator(typeof(HealthCheckControllerMetadata)),
            MethodInfo = HealthCheckControllerMetadata.GetApiMethodInfo(),
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

        var normalGetApiResponseType = new ApiResponseType
        {
            ApiResponseFormats =
            [
                new ApiResponseFormat
                {
                    MediaType = "text/plain",
                    Formatter = new StringOutputFormatter(),
                },
            ],
            Type = typeof(string),
            StatusCode = StatusCodes.Status200OK,
        };

        var errorGetApiResponseType = new ApiResponseType
        {
            ApiResponseFormats =
            [
                new ApiResponseFormat
                {
                    MediaType = "text/plain",
                    Formatter = new StringOutputFormatter(),
                },
            ],
            Type = typeof(string),
            StatusCode = StatusCodes.Status503ServiceUnavailable,
        };

        var normalHeadApiResponseType = new ApiResponseType
        {
            StatusCode = StatusCodes.Status200OK,
        };

        var errorHeadApiResponseType = new ApiResponseType
        {
            StatusCode = StatusCodes.Status503ServiceUnavailable,
        };

        getApiDescription.SupportedResponseTypes.Add(normalGetApiResponseType);
        getApiDescription.SupportedResponseTypes.Add(errorGetApiResponseType);
        headApiDescription.SupportedResponseTypes.Add(normalHeadApiResponseType);
        headApiDescription.SupportedResponseTypes.Add(errorHeadApiResponseType);

        context.Results.Add(getApiDescription);
        context.Results.Add(headApiDescription);
    }

    /// <summary>
    ///  ヘルスチェック API の形式を模したメタデータを提供します。
    /// </summary>
    private class HealthCheckControllerMetadata
    {
        /// <summary>
        ///  ヘルスチェック API を表す <see cref="MethodInfo"/> を取得します。
        /// </summary>
        /// <returns>ヘルスチェック API を表す <see cref="MethodInfo"/> 。</returns>
        /// <exception cref="InvalidOperationException">
        ///  ヘルスチェック API の <see cref="MethodInfo"/> を取得できませんでした。
        /// </exception>
        internal static MethodInfo GetApiMethodInfo()
        {
            return typeof(HealthCheckControllerMetadata)
                .GetMethod(nameof(Health), BindingFlags.NonPublic | BindingFlags.Static)
                ?? throw new InvalidOperationException("HealthCheckMethodNotFound");
        }

        private static string Health() => string.Empty;
    }
}
