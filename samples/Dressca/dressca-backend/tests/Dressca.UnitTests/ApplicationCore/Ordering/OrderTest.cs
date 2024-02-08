﻿using Dressca.ApplicationCore.Ordering;

namespace Dressca.UnitTests.ApplicationCore.Ordering;

public class OrderTest
{
    public static TheoryData<List<OrderItem>?> EmptyOrderItems => new()
    {
        null,
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
        var order = new Order(buyerId, shipTo, items);

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
        var action = () => new Order(buyerId!, shipTo, items);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void Constructor_住所がnull_ArgumentNullExceptionが発生する()
    {
        // Arrange
        var buyerId = Guid.NewGuid().ToString("D");
        var items = CreateDefaultOrderItems();

        // Act
        var action = () => new Order(buyerId, null!, items);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Theory]
    [MemberData(nameof(EmptyOrderItems))]
    public void Constructor_注文アイテムがnullまたは空のリスト_ArgumentExceptionが発生する(List<OrderItem>? emptyOrderItems)
    {
        // Arrange
        var buyerId = Guid.NewGuid().ToString("D");
        var shipTo = CreateDefaultShipTo();

        // Act
        var action = () => new Order(buyerId, shipTo, emptyOrderItems!);

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
        var order = new Order(buyerId, shipTo, items);

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
        var order = new Order(buyerId, shipTo, items);

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
        var order = new Order(buyerId, shipTo, items);

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
        var order = new Order(buyerId, shipTo, items);

        // Act
        var totalPrice = order.TotalPrice;

        // Assert
        Assert.Equal(4950m, totalPrice);
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
            new OrderItem(new CatalogItemOrdered(1, productName1, productCode1), 1000m, 1),
            new OrderItem(new CatalogItemOrdered(2, productName2, productCode2), 1500m, 2),
        };

        return items;
    }
}
