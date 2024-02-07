﻿using Dressca.SystemCommon;
using Dressca.Web.Runtime;
using Maris.Logging.Testing.Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Xunit.Abstractions;

namespace Dressca.UnitTests.Web.Runtime;

public class BusinessExceptionDevelopmentFilterTest
{
    private readonly TestLoggerManager loggerManager;

    public BusinessExceptionDevelopmentFilterTest(ITestOutputHelper testOutputHelper)
        => this.loggerManager = new TestLoggerManager(testOutputHelper);

    [Fact]
    public void OnException_業務エラーの情報がActionResultの値に設定される()
    {
        // Arrange
        var filter = this.CreateFilter();
        var errorCode = "ERR_CODE";
        var errorMessage1 = "ERR_MESSAGE1";
        var errorMessage2 = "ERR_MESSAGE2";
        var businessError = new BusinessError(errorCode, errorMessage1, errorMessage2);
        var context = CreateExceptionContext(businessError);

        // Act
        filter.OnException(context);

        // Assert
        var result = Assert.IsType<BadRequestObjectResult>(context.Result);
        var value = Assert.IsType<ValidationProblemDetails>(result.Value);
        Assert.Collection(
            value.Errors,
            error =>
            {
                Assert.Equal(errorCode, error.Key);
                Assert.Collection(
                    error.Value,
                    message => Assert.Equal(errorMessage1, message),
                    message => Assert.Equal(errorMessage2, message));
            });
    }

    [Fact]
    public void OnException_業務例外のスタックトレースがdetailに設定されている()
    {
        // Arrange
        var filter = this.CreateFilter();
        var errorCode = "ERR_CODE";
        var errorMessage1 = "ERR_MESSAGE1";
        var errorMessage2 = "ERR_MESSAGE2";
        var businessError = new BusinessError(errorCode, errorMessage1, errorMessage2);
        var context = CreateExceptionContext(businessError);

        // Act
        filter.OnException(context);

        // Assert
        var result = Assert.IsType<BadRequestObjectResult>(context.Result);
        var value = Assert.IsType<ValidationProblemDetails>(result.Value);
        Assert.Equal(context.Exception.ToString(), value.Detail);
    }

    private static ExceptionContext CreateExceptionContext(BusinessError businessError)
    {
        var httpContext = new DefaultHttpContext();
        var routeData = new RouteData();
        var actionDescriptor = new ControllerActionDescriptor();
        return new ExceptionContext(
            new ActionContext(httpContext, routeData, actionDescriptor),
            new List<IFilterMetadata>())
        {
            Exception = new BusinessException(businessError),
        };
    }

    private BusinessExceptionDevelopmentFilter CreateFilter()
    {
        var problemDetailsFactory = new TestProblemDetailsFactory();
        var logger = this.loggerManager.CreateLogger<BusinessExceptionDevelopmentFilter>();
        return new BusinessExceptionDevelopmentFilter(problemDetailsFactory, logger);
    }

    private class TestProblemDetailsFactory : ProblemDetailsFactory
    {
        public override ProblemDetails CreateProblemDetails(HttpContext httpContext, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
            => throw new NotImplementedException();

        public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext, ModelStateDictionary modelStateDictionary, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
            => new(modelStateDictionary)
            {
                Status = statusCode,
                Type = type,
                Detail = detail,
                Instance = instance,
                Title = title,
            };
    }
}
