using System.Globalization;
using Dressca.Web.Configuration;
using Dressca.Web.Consumer.Baskets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Time.Testing;

namespace Dressca.UnitTests.Web.Consumer.Baskets;

public class BuyerIdFilterAttributeTest
{
    [Fact]
    public void 構成ファイルに設定がないときCookieの有効期限は1日間()
    {
        // Arrange
        var buyerIdCookieName = "Dressca-Bid";
        var httpContext = new DefaultHttpContext();
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        var context = new ActionExecutedContext(actionContext, [], Mock.Of<Controller>());
        var fakeTimeProvider = new FakeTimeProvider();
        var testCookieCreatedDateTime = new DateTimeOffset(2024, 4, 1, 00, 00, 00, TimeSpan.Zero);
        fakeTimeProvider.SetUtcNow(testCookieCreatedDateTime);
        var expectedDateTime = testCookieCreatedDateTime.AddDays(1);
        var formattedExpectedDateTime = expectedDateTime.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture);
        var filter = new BuyerIdFilterAttribute(buyerIdCookieName, fakeTimeProvider, new WebServerOptions());

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
        var context = new ActionExecutedContext(actionContext, [], Mock.Of<Controller>());
        var fakeTimeProvider = new FakeTimeProvider();
        var testCookieCreatedDateTime = new DateTimeOffset(2024, 4, 1, 00, 00, 00, TimeSpan.Zero);
        fakeTimeProvider.SetUtcNow(testCookieCreatedDateTime);
        var expectedDateTime = testCookieCreatedDateTime.AddDays(10);
        var formattedExpectedDateTime = expectedDateTime.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture);
        var options = new WebServerOptions
        {
            CookieSettings = new CookieSettings
            {
                HttpOnly = false,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                ExpiredDays = 10,
                Domain = "example.com",
            },
        };
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
