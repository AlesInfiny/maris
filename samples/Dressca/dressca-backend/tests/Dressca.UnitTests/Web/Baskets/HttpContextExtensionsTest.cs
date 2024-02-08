using Dressca.Web.Baskets;
using Microsoft.AspNetCore.Http;

namespace Dressca.UnitTests.Web.Baskets;

public class HttpContextExtensionsTest
{
    [Fact]
    public void GetBuyerId_購入者IdがHttpContextに存在しない_新たにGuid形式の購入者Idが発行される()
    {
        // Arrange
        var items = new Dictionary<object, object?>();
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupProperty(httpContext => httpContext.Items, items);

        // Act
        var buyerId = HttpContextExtensions.GetBuyerId(httpContextMock.Object);

        // Assert
        Assert.True(Guid.TryParse(buyerId, out _));
    }

    [Fact]
    public void GetBuyerId_購入者Idが文字列型ではない_新たにGuid形式の購入者Idが発行される()
    {
        // Arrange
        var items = new Dictionary<object, object?>
        {
            { "Dressca-BuyerId", DateTimeOffset.Now },
        };
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupProperty(httpContext => httpContext.Items, items);

        // Act
        var buyerId = HttpContextExtensions.GetBuyerId(httpContextMock.Object);

        // Assert
        Assert.True(Guid.TryParse(buyerId, out _));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("not-guid-value")]
    public void GetBuyerId_購入者IdがGuidの文字列ではない_新たにGuid形式の購入者Idが発行される(string? itemValue)
    {
        // Arrange
        var items = new Dictionary<object, object?>
        {
            { "Dressca-BuyerId", itemValue },
        };
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupProperty(httpContext => httpContext.Items, items);

        // Act
        var buyerId = HttpContextExtensions.GetBuyerId(httpContextMock.Object);

        // Assert
        Assert.True(Guid.TryParse(buyerId, out _));
    }

    [Fact]
    public void GetBuyerId_購入者IdがGuidの文字列_設定されている値を取得できる()
    {
        // Arrange
        var buyerId = Guid.NewGuid().ToString();
        var items = new Dictionary<object, object?>
        {
            { "Dressca-BuyerId", buyerId },
        };
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupProperty(httpContext => httpContext.Items, items);

        // Act
        var actual = HttpContextExtensions.GetBuyerId(httpContextMock.Object);

        // Assert
        Assert.Equal(buyerId, actual);
    }

    [Fact]
    public void SetBuyerId_購入者Idを新たに追加できる()
    {
        // Arrange
        var buyerId = Guid.NewGuid().ToString();
        var items = new Dictionary<object, object?>();
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupProperty(httpContext => httpContext.Items, items);

        // Act
        HttpContextExtensions.SetBuyerId(httpContextMock.Object, buyerId);

        // Assert
        var item = Assert.Single(items, item => "Dressca-BuyerId".Equals(item.Key));
        Assert.Equal(buyerId, item.Value);
    }

    [Fact]
    public void SetBuyerId_購入者Idを上書きできる()
    {
        // Arrange
        var buyerId = Guid.NewGuid().ToString();
        var items = new Dictionary<object, object?>
        {
            { "Dressca-BuyerId", "dummy" },
        };
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupProperty(httpContext => httpContext.Items, items);

        // Act
        HttpContextExtensions.SetBuyerId(httpContextMock.Object, buyerId);

        // Assert
        var item = Assert.Single(items, item => "Dressca-BuyerId".Equals(item.Key));
        Assert.Equal(buyerId, item.Value);
    }
}
