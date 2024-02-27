using Dressca.Web.Baskets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Time.Testing;

namespace Dressca.UnitTests.Web.Baskets;

public class BuyerIdFilterAttributeTest
{
    [Fact]
    public void Cookieの有効期限は7日間()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        var context = new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), Mock.Of<Controller>());
        var fakeTimeProvider = new FakeTimeProvider();
        var testCookieCreatedTime = new DateTimeOffset(2024, 4, 1, 09, 00, 00, new(9, 0, 0));
        fakeTimeProvider.SetUtcNow(testCookieCreatedTime);
        var filter = new BuyerIdFilterAttribute(fakeTimeProvider);

        // Act
        filter.OnActionExecuted(context);
        var setCookieString = httpContext.Response.Headers.SetCookie.ToString();

        // Assert
        Assert.Contains("expires=Mon, 08 Apr 2024 00:00:00 GMT;", setCookieString);
    }
}
