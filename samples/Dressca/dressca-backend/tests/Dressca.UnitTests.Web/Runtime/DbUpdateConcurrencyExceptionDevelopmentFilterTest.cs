using Dressca.Web.Runtime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dressca.UnitTests.Web.Runtime;

public class DbUpdateConcurrencyExceptionDevelopmentFilterTest(ITestOutputHelper testOutputHelper) : TestBase(testOutputHelper)
{
    [Fact]
    public void OnException_409Conflictが設定される()
    {
        // Arrange
        var filter = this.CreateFilter();
        var dbUpdateConcurrencyEx = new DbUpdateConcurrencyException();
        var context = CreateExceptionContext(dbUpdateConcurrencyEx);

        // Act
        filter.OnException(context);

        // Assert
        Assert.IsType<ConflictObjectResult>(context.Result);
    }

    [Fact]
    public void OnException_スタックトレースがdetailに設定されている()
    {
        // Arrange
        var filter = this.CreateFilter();
        var dbUpdateConcurrencyEx = new DbUpdateConcurrencyException();
        var context = CreateExceptionContext(dbUpdateConcurrencyEx);

        // Act
        filter.OnException(context);

        // Assert
        var result = Assert.IsType<ConflictObjectResult>(context.Result);
        var value = Assert.IsType<ProblemDetails>(result.Value);
        Assert.Equal(context.Exception.ToString(), value.Detail);
    }

    [Fact]
    public void OnException_情報ログが1件登録される()
    {
        // Arrange
        var filter = this.CreateFilter();
        var dbUpdateConcurrencyEx = new DbUpdateConcurrencyException();
        var context = CreateExceptionContext(dbUpdateConcurrencyEx);

        // Act
        filter.OnException(context);

        // Assert
        Assert.Equal(1, this.LogCollector.Count);
        var record = this.LogCollector.LatestRecord;
        Assert.Equal("データベースの更新が競合しました。", record.Message);
        Assert.Equal(LogLevel.Information, record.Level);
        Assert.Equal(1001, record.Id);
        Assert.Same(context.Exception, record.Exception);
    }

    private static ExceptionContext CreateExceptionContext(DbUpdateConcurrencyException dbUpdateConcurrencyEx)
    {
        var httpContext = new DefaultHttpContext();
        var routeData = new RouteData();
        var actionDescriptor = new ControllerActionDescriptor();
        return new ExceptionContext(
            new ActionContext(httpContext, routeData, actionDescriptor),
            [])
        {
            Exception = dbUpdateConcurrencyEx,
        };
    }

    private DbUpdateConcurrencyExceptionDevelopmentFilter CreateFilter()
    {
        var problemDetailsFactory = new TestProblemDetailsFactory();
        var logger = this.CreateTestLogger<DbUpdateConcurrencyExceptionDevelopmentFilter>();
        return new DbUpdateConcurrencyExceptionDevelopmentFilter(problemDetailsFactory, logger);
    }

    private class TestProblemDetailsFactory : ProblemDetailsFactory
    {
        public override ProblemDetails CreateProblemDetails(HttpContext httpContext, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
            => new()
            {
                Status = statusCode,
                Title = title,
                Type = type,
                Detail = detail,
                Instance = instance,
            };

        public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext, ModelStateDictionary modelStateDictionary, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
            => throw new NotImplementedException();
    }
}
