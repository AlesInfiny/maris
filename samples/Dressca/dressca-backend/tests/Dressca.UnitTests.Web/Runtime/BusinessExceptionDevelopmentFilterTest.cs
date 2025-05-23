using Dressca.SystemCommon;
using Dressca.Web.Runtime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Dressca.UnitTests.Web.Runtime;

public class BusinessExceptionDevelopmentFilterTest(ITestOutputHelper testOutputHelper) : TestBase(testOutputHelper)
{
    [Fact]
    public void OnException_業務エラーの情報がActionResultの値に設定される()
    {
        // Arrange
        var filter = this.CreateFilter();
        var exceptionId = "ERR_CODE";
        var errorMessage1 = new ErrorMessage("ERR_MESSAGE1");
        var errorMessage2 = new ErrorMessage("ERR_MESSAGE2");
        var businessError = new BusinessError(exceptionId, errorMessage1, errorMessage2);
        var context = CreateExceptionContext(businessError);

        // Act
        filter.OnException(context);

        // Assert
        var result = Assert.IsType<BadRequestObjectResult>(context.Result);
        var value = Assert.IsType<ValidationProblemDetails>(result.Value);
        var error = Assert.Single(value.Errors, error => exceptionId.Equals(error.Key));
        Assert.Collection(
            error.Value,
            message => Assert.Equal(errorMessage1.Message, message),
            message => Assert.Equal(errorMessage2.Message, message));
    }

    [Fact]
    public void OnException_業務例外のスタックトレースがdetailに設定されている()
    {
        // Arrange
        var filter = this.CreateFilter();
        var exceptionId = "ERR_CODE";
        var errorMessage1 = new ErrorMessage("ERR_MESSAGE1");
        var errorMessage2 = new ErrorMessage("ERR_MESSAGE2");
        var businessError = new BusinessError(exceptionId, errorMessage1, errorMessage2);
        var context = CreateExceptionContext(businessError);

        // Act
        filter.OnException(context);

        // Assert
        var result = Assert.IsType<BadRequestObjectResult>(context.Result);
        var value = Assert.IsType<ValidationProblemDetails>(result.Value);
        Assert.Equal(context.Exception.ToString(), value.Detail);
    }

    [Fact]
    public void OnException_exceptionIdとexceptionValuesが設定される()
    {
        // Arrange
        var filter = this.CreateFilter();
        var expectedExceptionId = "ERR_CODE";
        string[] errorMessageValues = ["1", "2"];
        var errorMessage = new ErrorMessage("ERR_MESSAGE", errorMessageValues);
        var businessError = new BusinessError(expectedExceptionId, errorMessage);
        var context = CreateExceptionContext(businessError);

        // Act
        filter.OnException(context);

        // Assert
        var result = Assert.IsType<BadRequestObjectResult>(context.Result);
        var value = Assert.IsType<ValidationProblemDetails>(result.Value);
        var exceptionId = Assert.Single(value.Extensions, extension => extension.Key.Equals("exceptionId"));
        Assert.Equal(expectedExceptionId, exceptionId.Value);
        var exceptionValues = Assert.Single(value.Extensions, extension => extension.Key.Equals("exceptionValues"));
        Assert.Equal(errorMessageValues, exceptionValues.Value);
    }

    [Fact]
    public void OnException_情報ログが1件登録される()
    {
        // Arrange
        var filter = this.CreateFilter();
        var exceptionId = "ERR_CODE";
        var errorMessage1 = new ErrorMessage("ERR_MESSAGE1");
        var errorMessage2 = new ErrorMessage("ERR_MESSAGE2");
        var businessError = new BusinessError(exceptionId, errorMessage1, errorMessage2);
        var context = CreateExceptionContext(businessError);

        // Act
        filter.OnException(context);

        // Assert
        Assert.Equal(1, this.LogCollector.Count);
        var record = this.LogCollector.LatestRecord;
        Assert.Equal("業務エラーが発生しました。", record.Message);
        Assert.Equal(LogLevel.Information, record.Level);
        Assert.Equal(0001, record.Id);
        Assert.Same(context.Exception, record.Exception);
    }

    private static ExceptionContext CreateExceptionContext(BusinessError businessError)
    {
        var httpContext = new DefaultHttpContext();
        var routeData = new RouteData();
        var actionDescriptor = new ControllerActionDescriptor();
        return new ExceptionContext(
            new ActionContext(httpContext, routeData, actionDescriptor),
            [])
        {
            Exception = new BusinessException(businessError),
        };
    }

    private BusinessExceptionDevelopmentFilter CreateFilter()
    {
        var problemDetailsFactory = new TestProblemDetailsFactory();
        var logger = this.CreateTestLogger<BusinessExceptionDevelopmentFilter>();
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
