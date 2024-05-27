using System.Globalization;
using Dressca.Web.Baskets;
using Dressca.Web.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Time.Testing;

namespace Dressca.UnitTests.Web.Baskets;

public class BuyerIdFilterAttributeTest
{
    [Fact]
    public void 構成ファイルに設定がないときCookieの有効期限は7日間()
    {
        // Arrange
        var buyerIdCookieName = "Dressca-Bid";
        var httpContext = new DefaultHttpContext();
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        var context = new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), Mock.Of<Controller>());
        var fakeTimeProvider = new FakeTimeProvider();
        var testCookieCreatedDateTime = new DateTimeOffset(2024, 4, 1, 00, 00, 00, TimeSpan.Zero);
        fakeTimeProvider.SetUtcNow(testCookieCreatedDateTime);
        var expectedDateTime = testCookieCreatedDateTime.AddDays(7);
        var formattedExpectedDateTime = expectedDateTime.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture);
        var filter = new BuyerIdFilterAttribute(buyerIdCookieName, fakeTimeProvider, null);

        // Act
        filter.OnActionExecuted(context);
        var setCookieString = httpContext.Response.Headers.SetCookie.ToString();

        // Assert
        Assert.Contains(formattedExpectedDateTime, setCookieString);
    }

    [Fact]
    public void 構成ファイルに設定があるとき構成ファイルの内容がCookieに設定される()
    {
        // Arrange
        var buyerIdCookieName = "Dressca-Bid";
        var httpContext = new DefaultHttpContext();
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        var context = new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), Mock.Of<Controller>());
        var fakeTimeProvider = new FakeTimeProvider();
        var testCookieCreatedDateTime = new DateTimeOffset(2024, 4, 1, 00, 00, 00, TimeSpan.Zero);
        fakeTimeProvider.SetUtcNow(testCookieCreatedDateTime);
        var expectedDateTime = testCookieCreatedDateTime.AddDays(10);
        var formattedExpectedDateTime = expectedDateTime.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture);
        var options = new WebServerOptions();
        options.CookieSettings = new Dressca.Web.Configuration.CookieSettings( );
        options.CookieSettings.HttpOnly = false;
        options.CookieSettings.Secure = false;
        options.CookieSettings.SameSite = SameSiteMode.Lax;
        options.CookieSettings.ExpiredDays = 10;
        options.CookieSettings.Domain = "example.com";
        var filter = new BuyerIdFilterAttribute(buyerIdCookieName, fakeTimeProvider, options);

        // Act
        filter.OnActionExecuted(context);
        var setCookieString = httpContext.Response.Headers.SetCookie.ToString();

        // Assert
        Assert.Contains(formattedExpectedDateTime, setCookieString);
        Assert.DoesNotContain("Secure", setCookieString, StringComparison.InvariantCultureIgnoreCase);
        Assert.DoesNotContain("HttpOnly", setCookieString, StringComparison.InvariantCultureIgnoreCase);
        Assert.Contains("Lax", setCookieString, StringComparison.InvariantCultureIgnoreCase);
        Assert.Contains("example.com", setCookieString);
    }
}
