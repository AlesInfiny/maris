using System.Globalization;
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
        var testCookieCreatedDateTime = new DateTimeOffset(2024, 4, 1, 00, 00, 00, TimeSpan.Zero);
        fakeTimeProvider.SetUtcNow(testCookieCreatedDateTime);
        var expectedDateTime = testCookieCreatedDateTime.AddDays(7);
        var formattedExpectedDateTime = expectedDateTime.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture);
        var filter = new BuyerIdFilterAttribute(fakeTimeProvider);

        // Act
        filter.OnActionExecuted(context);
        var setCookieString = httpContext.Response.Headers.SetCookie.ToString();

        // Assert
        Assert.Contains(formattedExpectedDateTime, setCookieString);
    }
}
