using System.Globalization;
using Dressca.Web.Configuration;
using Dressca.Web.Consumer.Baskets;
using Dressca.Web.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;

namespace Dressca.UnitTests.Web.Consumer.Baskets;

public class BuyerIdFilterAttributeTest
{
    [Fact]
    public void 構成ファイルに設定がないときはCookieの各属性に既定値が設定される()
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

        var applicationCookieBuilder = new ApplicationCookieBuilder(Options.Create(new WebServerOptions()));
        var filter = new BuyerIdFilterAttribute(buyerIdCookieName, fakeTimeProvider, applicationCookieBuilder, new CookiePolicyOptions());

        // Act
        filter.OnActionExecuted(context);
        var setCookieString = httpContext.Response.Headers.SetCookie.ToString();

        // Assert
        Assert.Contains(formattedExpectedDateTime, setCookieString);
        Assert.DoesNotContain("Domain", setCookieString, StringComparison.CurrentCultureIgnoreCase);
        Assert.DoesNotContain("Secure", setCookieString, StringComparison.CurrentCultureIgnoreCase);
        Assert.DoesNotContain("HttpOnly", setCookieString, StringComparison.CurrentCultureIgnoreCase);
        Assert.DoesNotContain("SameSite", setCookieString, StringComparison.CurrentCultureIgnoreCase);
    }

    [Fact]
    public void 構成ファイルに設定があるとき構成ファイルのDomainとExpiredDaysがCookieに設定される()
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

        var webServerOptions = new WebServerOptions
        {
            CookieSettings = new List<CookieSetting>
            {
                new CookieSetting
                {
                    CookieName = buyerIdCookieName,
                    ExpiredDays = 10,
                    Domain = "example.com",
                },
            },
        };
        var applicationCookieBuilder = new ApplicationCookieBuilder(Options.Create(webServerOptions));
        CookiePolicyOptions cookieOptions = new CookiePolicyOptions
        {
            Secure = CookieSecurePolicy.None,
            HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.None,
            MinimumSameSitePolicy = SameSiteMode.Lax,
        };

        var filter = new BuyerIdFilterAttribute(buyerIdCookieName, fakeTimeProvider, applicationCookieBuilder, cookieOptions);

        // Act
        filter.OnActionExecuted(context);
        var setCookieString = httpContext.Response.Headers.SetCookie.ToString();

        // Assert
        Assert.Contains(formattedExpectedDateTime, setCookieString, StringComparison.CurrentCultureIgnoreCase);
        Assert.Contains("example.com", setCookieString);
        Assert.DoesNotContain("Secure", setCookieString, StringComparison.CurrentCultureIgnoreCase);
        Assert.DoesNotContain("HttpOnly", setCookieString, StringComparison.CurrentCultureIgnoreCase);
        Assert.Contains("SameSite=Lax", setCookieString, StringComparison.CurrentCultureIgnoreCase);
    }
}
