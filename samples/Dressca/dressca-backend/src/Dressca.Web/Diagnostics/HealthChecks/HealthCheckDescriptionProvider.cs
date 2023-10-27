using System.Reflection;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dressca.Web.Diagnostics.HealthChecks;

public class HealthCheckDescriptionProvider : IApiDescriptionProvider
{
    private readonly IModelMetadataProvider _modelMetadataProvider;

    public HealthCheckDescriptionProvider(IModelMetadataProvider modelMetadataProvider)
    {
        _modelMetadataProvider = modelMetadataProvider;
    }

    public int Order => -1;

    public void OnProvidersExecuting(ApiDescriptionProviderContext context)
    { }

    public void OnProvidersExecuted(ApiDescriptionProviderContext context)
    {
        var actionDescriptor = new ControllerActionDescriptor
        {
            ControllerName = "HealthChecks",
            ActionName = "health",
            Parameters = new List<ParameterDescriptor>(),
            EndpointMetadata = new List<object>(),
            ActionConstraints = new List<IActionConstraintMetadata>(),
            DisplayName = "Health check endpoint",
            Properties = new Dictionary<object, object?>(),
            BoundProperties = new List<ParameterDescriptor>(),
            FilterDescriptors = new List<FilterDescriptor>(),
            ControllerTypeInfo = new TypeDelegator(typeof(string))
        };

        var apiDescription = new ApiDescription
        {
            ActionDescriptor = actionDescriptor,
            HttpMethod = HttpMethods.Get,
            RelativePath = "health",
            GroupName = "v1",
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
