using Dressca.ApplicationCore.Ordering;
using Microsoft.Extensions.Time.Testing;

namespace Dressca.UnitTests.ApplicationCore.Ordering;

public class OrderTest
{
    public static TheoryData<List<OrderItem>?> EmptyOrderItems => new()
    {
        (List<OrderItem>?)null,
        new List<OrderItem>(),
    };

    [Fact]
    public void Constructor_正しくインスタンス化できる()
    {
        // Arrange
        var buyerId = Guid.NewGuid().ToString("D");

        var shipTo = CreateDefaultShipTo();
        var items = CreateDefaultOrderItems();

        // Act
        var order = new Order(items) { BuyerId = buyerId, ShipToAddress = shipTo };

        // Assert
        Assert.NotNull(order);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_購入者Idがnullまたは空の文字列_ArgumentExceptionが発生する(string? buyerId)
    {
        // Arrange
        var shipTo = CreateDefaultShipTo();
        var items = CreateDefaultOrderItems();

        // Act
        var action = () => new Order(items) { BuyerId = buyerId!, ShipToAddress = shipTo };

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Theory]
    [MemberData(nameof(EmptyOrderItems))]
    public void Constructor_注文アイテムがnullまたは空のリスト_ArgumentExceptionが発生する(List<OrderItem>? emptyOrderItems)
    {
        // Arrange
        var buyerId = Guid.NewGuid().ToString("D");
        var shipTo = CreateDefaultShipTo();

        // Act
        var action = () => new Order(emptyOrderItems!) { BuyerId = buyerId, ShipToAddress = shipTo };

        // Assert
        var ex = Assert.Throws<ArgumentException>("orderItems", action);
        Assert.StartsWith("null または空のリストを設定できません。", ex.Message);
    }

    [Fact]
    public void TotalItemsPrice_商品の税抜き合計金額が正しく計算できる()
    {
        // Arrange
        var buyerId = Guid.NewGuid().ToString("D");
        var shipTo = CreateDefaultShipTo();
        var items = CreateDefaultOrderItems();
        var order = new Order(items) { BuyerId = buyerId, ShipToAddress = shipTo };

        // Act
        var totalPrice = order.TotalItemsPrice;

        // Assert
        Assert.Equal(4000m, totalPrice);
    }

    [Fact]
    public void DeliveryCharge_商品の送料が正しく計算できる()
    {
        // Arrange
        var buyerId = Guid.NewGuid().ToString("D");
        var shipTo = CreateDefaultShipTo();
        var items = CreateDefaultOrderItems();
        var order = new Order(items) { BuyerId = buyerId, ShipToAddress = shipTo };

        // Act
        var deliveryCharge = order.DeliveryCharge;

        // Assert
        Assert.Equal(500m, deliveryCharge);
    }

    [Fact]
    public void ConsumptionTax_商品の消費税額が正しく計算できる()
    {
        // Arrange
        var buyerId = Guid.NewGuid().ToString("D");
        var shipTo = CreateDefaultShipTo();
        var items = CreateDefaultOrderItems();
        var order = new Order(items) { BuyerId = buyerId, ShipToAddress = shipTo };

        // Act
        var tax = order.ConsumptionTax;

        // Assert
        Assert.Equal(450m, tax);
    }

    [Fact]
    public void TotalPrice_商品の税込み合計金額が正しく計算できる()
    {
        // Arrange
        var buyerId = Guid.NewGuid().ToString("D");
        var shipTo = CreateDefaultShipTo();
        var items = CreateDefaultOrderItems();
        var order = new Order(items) { BuyerId = buyerId, ShipToAddress = shipTo };

        // Act
        var totalPrice = order.TotalPrice;

        // Assert
        Assert.Equal(4950m, totalPrice);
    }

    [Fact]
    public void HasMatchingBuyerId_指定の購入者Idと一致_true()
    {
        // Arrange
        var buyerId = Guid.NewGuid().ToString("D");
        var shipTo = CreateDefaultShipTo();
        var items = CreateDefaultOrderItems();
        var order = new Order(items) { BuyerId = buyerId, ShipToAddress = shipTo };

        // Act
        var result = order.HasMatchingBuyerId(buyerId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasMatchingBuyerId_指定の購入者Idと一致しない_false()
    {
        // Arrange
        var buyerId = Guid.NewGuid().ToString("D");
        var shipTo = CreateDefaultShipTo();
        var items = CreateDefaultOrderItems();
        var order = new Order(items) { BuyerId = buyerId, ShipToAddress = shipTo };

        var unmatchingBuyerId = Guid.NewGuid().ToString("D");

        // Act
        var result = order.HasMatchingBuyerId(unmatchingBuyerId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Constructor_OrderDateが注文時のシステム時刻と等しい()
    {
        // Arrange
        var buyerId = Guid.NewGuid().ToString("D");
        var shipTo = CreateDefaultShipTo();
        var items = CreateDefaultOrderItems();
        var fakeTimeProvider = new FakeTimeProvider();
        var testOrderTime = new DateTimeOffset(2024, 4, 1, 00, 00, 00, new TimeSpan(9, 0, 0));
        fakeTimeProvider.SetUtcNow(testOrderTime);

        // Act
        var order = new Order(items, fakeTimeProvider) { BuyerId = buyerId, ShipToAddress = shipTo };

        // Assert
        Assert.Equal(testOrderTime, order.OrderDate);
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

    private static List<OrderItem> CreateDefaultOrderItems()
    {
        // Arrange
        const string productName1 = "ダミー商品1";
        const string productCode1 = "C000000001";

        const string productName2 = "ダミー商品2";
        const string productCode2 = "C000000002";

        var items = new List<OrderItem>()
        {
            new() { ItemOrdered = new CatalogItemOrdered(1, productName1, productCode1), UnitPrice = 1000m, Quantity = 1 },
            new() { ItemOrdered = new CatalogItemOrdered(2, productName2, productCode2), UnitPrice = 1500m, Quantity = 2 },
        };

        return items;
    }
}
