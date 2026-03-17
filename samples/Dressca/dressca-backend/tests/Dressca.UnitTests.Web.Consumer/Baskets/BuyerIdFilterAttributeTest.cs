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

        var applicationCookieBuilder = new ApplicationCookieBuilder(Options.Create(new WebServerOptions()));
        var filter = new BuyerIdFilterAttribute(buyerIdCookieName, applicationCookieBuilder, new CookiePolicyOptions());
        var expectedMaxAge = TimeSpan.FromDays(1);

        // Act
        filter.OnActionExecuted(context);
        var setCookieString = httpContext.Response.Headers.SetCookie.ToString();

        // Assert
        Assert.Contains($"Max-Age={expectedMaxAge.TotalSeconds}", setCookieString, StringComparison.CurrentCultureIgnoreCase);
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
        var expectedMaxAge = TimeSpan.FromDays(10);

        var webServerOptions = new WebServerOptions
        {
            CookieSettings = new List<CookieSetting>
            {
                new CookieSetting
                {
                    CookieName = buyerIdCookieName,
                    ExpiredDays = 10,
                    Domain = "example.com",
                    Path = "/api",
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

        var filter = new BuyerIdFilterAttribute(buyerIdCookieName, applicationCookieBuilder, cookieOptions);

        // Act
        filter.OnActionExecuted(context);
        var setCookieString = httpContext.Response.Headers.SetCookie.ToString();

        // Assert
        Assert.Contains($"Max-Age={expectedMaxAge.TotalSeconds}", setCookieString, StringComparison.CurrentCultureIgnoreCase);
        Assert.Contains("example.com", setCookieString, StringComparison.CurrentCultureIgnoreCase);
        Assert.Contains("Path=/api", setCookieString, StringComparison.CurrentCultureIgnoreCase);
        Assert.DoesNotContain("Secure", setCookieString, StringComparison.CurrentCultureIgnoreCase);
        Assert.DoesNotContain("HttpOnly", setCookieString, StringComparison.CurrentCultureIgnoreCase);
        Assert.Contains("SameSite=Lax", setCookieString, StringComparison.CurrentCultureIgnoreCase);
    }
}
