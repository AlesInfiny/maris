using System.Text;
using Dressca.Web.Runtime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace Dressca.UnitTests.Web.Runtime;

public class OriginVerificationResourceFilterTest(ITestOutputHelper testOutputHelper) : TestBase(testOutputHelper)
{
    [Fact]
    public void GETリクエストの場合Originが設定されていなくても403Forbiddenにならず正常終了する()
    {
        // Arrange
        var config = new Dictionary<string, string?>();
        config.Add("AppSettings:AllowedOrigins:0", "http://localhost:11111");
        var filter = new OriginVerificationResourceFilter(this.CreateConfig(config));
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Method = "GET";

        var context = this.CreateContext(httpContext);

        // Act
        filter.OnResourceExecuting(context);

        // Assert
        Assert.IsNotType<ForbiddenCodeResult>(context.Result);
    }

    [Fact]
    public void HEADリクエストの場合Originが設定されていなくても403Forbiddenにならず正常終了する()
    {
        // Arrange
        var config = new Dictionary<string, string?>();
        config.Add("AppSettings:AllowedOrigins:0", "http://localhost:11111");
        var filter = new OriginVerificationResourceFilter(this.CreateConfig(config));
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Method = "HEAD";
        var context = this.CreateContext(httpContext);

        // Act
        filter.OnResourceExecuting(context);

        // Assert
        Assert.IsNotType<ForbiddenCodeResult>(context.Result);
    }

    [Fact]
    public void Originが設定されていないPOSTリクエストでは403Forbiddenが返る()
    {
        // Arrange
        var config = new Dictionary<string, string?>();
        config.Add("AppSettings:AllowedOrigins:0", "http://localhost:11111");
        var filter = new OriginVerificationResourceFilter(this.CreateConfig(config));
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Method = "POST";
        var context = this.CreateContext(httpContext);

        // Act
        filter.OnResourceExecuting(context);

        // Assert
        Assert.IsType<ForbiddenCodeResult>(context.Result);
    }

    [Fact]
    public void アプリケーション構成設定に許可するオリジンが設定されている場合Originと一致すれば403Forbiddenにならず正常終了する()
    {
        // Arrange
        var config = new Dictionary<string, string?>();
        config.Add("AppSettings:AllowedOrigins:0", "http://localhost:11111");
        var filter = new OriginVerificationResourceFilter(this.CreateConfig(config));
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Method = "POST";
        httpContext.Request.Headers.Append("Origin", "http://localhost:11111");
        var context = this.CreateContext(httpContext);

        // Act
        filter.OnResourceExecuting(context);

        // Assert
        Assert.IsNotType<ForbiddenCodeResult>(context.Result);
    }

    [Fact]
    public void アプリケーション構成設定に許可するオリジンが設定されておりOriginと一致しない場合は403Forbiddenが返る()
    {
        // Arrange
        var config = new Dictionary<string, string?>();
        config.Add("AppSettings:AllowedOrigins:0", "http://localhost:11111");
        var filter = new OriginVerificationResourceFilter(this.CreateConfig(config));
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Method = "POST";
        httpContext.Request.Headers.Append("Origin", "http://localhost:11112");
        var context = this.CreateContext(httpContext);

        // Act
        filter.OnResourceExecuting(context);

        // Assert
        Assert.IsNotType<ForbiddenCodeResult>(context.Result);
    }

    private ResourceExecutingContext CreateContext(DefaultHttpContext httpContext)
    {
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        var resourceExecutedContext = new ResourceExecutingContext(actionContext, new List<IFilterMetadata>(), new List<IValueProviderFactory>());
        return resourceExecutedContext;
    }

    private IConfigurationRoot CreateConfig(Dictionary<string, string?> testConfiguration)
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(testConfiguration)
            .Build();
        return config;
    }
}
