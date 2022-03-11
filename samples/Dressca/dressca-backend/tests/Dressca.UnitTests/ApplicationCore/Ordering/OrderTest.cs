using Dressca.ApplicationCore.Ordering;

namespace Dressca.UnitTests.ApplicationCore.Ordering;

public class OrderTest
{
    [Fact]
    public void 正しくインスタンス化できる()
    {
        // Arrange
        const string buyerId = "taro.kokkai @example.com";

        var shipTo = CreateDefaultShipTo();

        // Act
        var order = new Order(buyerId, shipTo, new List<OrderItem>());

        // Assert
        Assert.NotNull(order);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void 購入者Idは必須(string? buyerId)
    {
        // Arrange
        var shipTo = CreateDefaultShipTo();

        // Act
        var action = () => new Order(buyerId!, shipTo, new List<OrderItem>());

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void 住所は必須()
    {
        // Arrange
        const string buyerId = "taro.kokkai @example.com";

        // Act
        var action = () => new Order(buyerId, null!, new List<OrderItem>());

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void 注文アイテムは必須()
    {
        // Arrange
        const string buyerId = "taro.kokkai @example.com";

        var shipTo = CreateDefaultShipTo();

        // Act
        var action = () => new Order(buyerId, shipTo, null!);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void 合計金額が正しく計算できる()
    {
        // Arrange
        const string productName1 = "ダミー商品1";
        const string productCode1 = "C000000001";

        const string productName2 = "ダミー商品2";
        const string productCode2 = "C000000002";

        var items = new List<OrderItem>()
        {
            new OrderItem(new CatalogItemOrdered(1L, productName1, productCode1), 1000m, 1),
            new OrderItem(new CatalogItemOrdered(2L, productName2, productCode2), 2000m, 2),
        };

        const string buyerId = "taro.kokkai @example.com";

        var shipTo = CreateDefaultShipTo();

        var order = new Order(buyerId, shipTo, items);

        // Act
        var totalPrice = order.Total();

        // Assert
        Assert.Equal(5000m, totalPrice);
    }

    [Fact]
    public void 注文アイテムが0件なら合計金額は0()
    {
        // Arrange
        const string buyerId = "taro.kokkai @example.com";

        var shipTo = CreateDefaultShipTo();

        var order = new Order(buyerId, shipTo, new List<OrderItem>());

        // Act
        var totalPrice = order.Total();

        // Assert
        Assert.Equal(0m, totalPrice);
    }

    private static Address CreateDefaultAddress()
    {
        const string defaultPostalCode = "100-8924";
        const string defaultTodofuken = "東京都";
        const string defaultShikuchoson = "千代田区";
        const string defaultAzanaAndOthers = "永田町1-10-1";

        return new Address(defaultPostalCode, defaultTodofuken, defaultShikuchoson, defaultAzanaAndOthers);
    }

    private static ShipTo CreateDefaultShipTo()
    {
        const string defaultFullName = "国会　太郎";
        var address = CreateDefaultAddress();
        return new ShipTo(defaultFullName, address);
    }
}
