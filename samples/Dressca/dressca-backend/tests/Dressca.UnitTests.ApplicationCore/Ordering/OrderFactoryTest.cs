using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.DisplayItems;
using Dressca.ApplicationCore.Ordering;

namespace Dressca.UnitTests.ApplicationCore.Ordering;

public class OrderFactoryTest
{
    [Fact]
    public void CreateOrder_正しくインスタンスが生成される_basketItemに対応したorderItemが登録される()
    {
        // Arrange
        var item1 = new Guid("019b76da-a800-7004-8001-000000000001");
        var item2 = new Guid("019b76da-a800-7004-8001-000000000002");
        var basket = new Basket() { BuyerId = "dummyId" };
        var basketItem1 = new BasketItem() { DisplayItemId = item1, UnitPrice = 1000m, Quantity = 1 };
        var basketItem2 = new BasketItem() { DisplayItemId = item2, UnitPrice = 2000m, Quantity = 2 };
        basket.AddItem(basketItem1.DisplayItemId, basketItem1.UnitPrice, basketItem1.Quantity);
        basket.AddItem(basketItem2.DisplayItemId, basketItem2.UnitPrice, basketItem2.Quantity);

        var displayItems = CreateDefaultDisplayItems();
        var shipTo = CreateDefaultShipTo();
        var factory = new OrderFactory();

        // Act
        var result = factory.CreateOrder(basket, displayItems, shipTo);

        // Assert
        Assert.Collection(
            result.OrderItems,
            orderItem =>
            {
                Assert.Equal(basketItem1.Quantity, orderItem.Quantity);
                Assert.Equal(basketItem1.UnitPrice, orderItem.UnitPrice);

                var displayItem = displayItems.First(c => c.Id == basketItem1.DisplayItemId);
                Assert.Equal(displayItem.ProductCode, orderItem.ItemOrdered.ProductCode);
                Assert.Equal(displayItem.Name, orderItem.ItemOrdered.ProductName);
                Assert.Equal(displayItem.Id, orderItem.ItemOrdered.DisplayItemId);
            },
            orderItem =>
            {
                Assert.Equal(basketItem2.Quantity, orderItem.Quantity);
                Assert.Equal(basketItem2.UnitPrice, orderItem.UnitPrice);

                var displayItem = displayItems.First(c => c.Id == basketItem2.DisplayItemId);
                Assert.Equal(displayItem.ProductCode, orderItem.ItemOrdered.ProductCode);
                Assert.Equal(displayItem.Name, orderItem.ItemOrdered.ProductName);
                Assert.Equal(displayItem.Id, orderItem.ItemOrdered.DisplayItemId);
            });
    }

    [Fact]
    public void CreateOrder_basketがnullの場合_ArgumentNullExceptionが発生する()
    {
        // Arrange
        var displayItems = CreateDefaultDisplayItems();
        var shipTo = CreateDefaultShipTo();
        var factory = new OrderFactory();

        // Act
        var action = () => factory.CreateOrder(null!, displayItems, shipTo);

        // Assert
        Assert.Throws<ArgumentNullException>("basket", action);
    }

    [Fact]
    public void CreateOrder_displayItemsがnullの場合_ArgumentNullExceptionが発生する()
    {
        // Arrange
        var item1 = new Guid("019b76da-a800-7004-8001-000000000001");
        var basket = new Basket() { BuyerId = "dummyId" };
        basket.AddItem(item1, 1000m, 2);
        var shipTo = CreateDefaultShipTo();
        var factory = new OrderFactory();

        // Act
        var action = () => factory.CreateOrder(basket, null!, shipTo);

        // Assert
        Assert.Throws<ArgumentNullException>("displayItems", action);
    }

    [Fact]
    public void CreateOrder_shipToAddressがnullの場合_ArgumentNullExceptionが発生する()
    {
        // Arrange
        var item1 = new Guid("019b76da-a800-7004-8001-000000000001");
        var basket = new Basket() { BuyerId = "dummyId" };
        basket.AddItem(item1, 1000m, 2);
        var displayItems = CreateDefaultDisplayItems();
        var factory = new OrderFactory();

        // Act
        var action = () => factory.CreateOrder(basket, displayItems, null!);

        // Assert
        Assert.Throws<ArgumentNullException>("shipToAddress", action);
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

    private static IReadOnlyList<DisplayItem> CreateDefaultDisplayItems()
    {
        var category1 = new Guid("019b76da-a800-7003-8001-000000000001");
        var brand1 = new Guid("019b76da-a800-7002-8001-000000000001");
        var brand2 = new Guid("019b76da-a800-7002-8001-000000000002");
        var brand3 = new Guid("019b76da-a800-7002-8001-000000000003");
        var item1 = new Guid("019b76da-a800-7004-8001-000000000001");
        var item2 = new Guid("019b76da-a800-7004-8001-000000000002");
        var item3 = new Guid("019b76da-a800-7004-8001-000000000003");
        var displayItem = new List<DisplayItem>()
        {
            new() { DisplayItemCategoryId = category1, DisplayItemBrandId = brand3, Description = "定番の無地ロングTシャツです。", Name = "クルーネック Tシャツ - ブラック", Price = 1980m, ProductCode = "C000000001", Id = item1 },
            new() { DisplayItemCategoryId = category1, DisplayItemBrandId = brand2, Description = "暖かいのに着膨れしない起毛デニムです。", Name = "裏起毛 スキニーデニム", Price = 4800m, ProductCode = "C000000002", Id = item2 },
            new() { DisplayItemCategoryId = category1, DisplayItemBrandId = brand1, Description = "あたたかく肌ざわりも良いウール100%のロングコートです。", Name = "ウールコート", Price = 49800m, ProductCode = "C000000003", Id = item3 },
        };
        return displayItem;
    }
}
