﻿using Dressca.Web.BuyerIdExtensions;
using Microsoft.AspNetCore.Http;

namespace Dressca.UnitTests.Web.BuyerIdExtensions;

public class HttpContextExtensionsTest
{
    [Fact]
    public void 購入者IdがHttpContextに存在しない場合新たにGuid形式の購入者Idが発行される()
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
    public void 購入者Idが文字列型ではない場合新たにGuid形式の購入者Idが発行される()
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
    public void 購入者IdがGuidの文字列ではない場合新たにGuid形式の購入者Idが発行される(string? itemValue)
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
    public void 購入者IdがGuidの文字列の場合設定されている値を取得できる()
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
    public void 購入者Idを新たに追加できる()
    {
        // Arrange
        var buyerId = Guid.NewGuid().ToString();
        var items = new Dictionary<object, object?>();
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupProperty(httpContext => httpContext.Items, items);

        // Act
        HttpContextExtensions.SetBuyerId(httpContextMock.Object, buyerId);

        // Assert
        Assert.Collection(
            items,
            item =>
            {
                Assert.Equal("Dressca-BuyerId", item.Key);
                Assert.Equal(buyerId, item.Value);
            });
    }

    [Fact]
    public void 購入者Idを上書きできる()
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
        Assert.Collection(
            items,
            item =>
            {
                Assert.Equal("Dressca-BuyerId", item.Key);
                Assert.Equal(buyerId, item.Value);
            });
    }
}
