using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.Catalog;
using Dressca.ApplicationCore.Ordering;

namespace Dressca.UnitTests.ApplicationCore.Ordering;

public class OrderFactoryTest
{
    [Fact]
    public void CreateOrder_正しくインスタンスが生成される_basketItemに対応したorderItemが登録される()
    {
        // Arrange
        var basket = new Basket() { BuyerId = "dummyId" };
        var basketItem1 = new BasketItem() { CatalogItemId = 1L, UnitPrice = 1000m, Quantity = 1 };
        var basketItem2 = new BasketItem() { CatalogItemId = 2L, UnitPrice = 2000m, Quantity = 2 };
        basket.AddItem(basketItem1.CatalogItemId, basketItem1.UnitPrice, basketItem1.Quantity);
        basket.AddItem(basketItem2.CatalogItemId, basketItem2.UnitPrice, basketItem2.Quantity);

        var catalogItems = CreateDefaultCatalogItems();
        var shipTo = CreateDefaultShipTo();
        var factory = new OrderFactory();

        // Act
        var result = factory.CreateOrder(basket, catalogItems, shipTo);

        // Assert
        Assert.Collection(
            result.OrderItems,
            orderItem =>
            {
                Assert.Equal(basketItem1.Quantity, orderItem.Quantity);
                Assert.Equal(basketItem1.UnitPrice, orderItem.UnitPrice);

                var catalogItem = catalogItems.First(c => c.Id == basketItem1.CatalogItemId);
                Assert.Equal(catalogItem.ProductCode, orderItem.ItemOrdered.ProductCode);
                Assert.Equal(catalogItem.Name, orderItem.ItemOrdered.ProductName);
                Assert.Equal(catalogItem.Id, orderItem.ItemOrdered.CatalogItemId);
            },
            orderItem =>
            {
                Assert.Equal(basketItem2.Quantity, orderItem.Quantity);
                Assert.Equal(basketItem2.UnitPrice, orderItem.UnitPrice);

                var catalogItem = catalogItems.First(c => c.Id == basketItem2.CatalogItemId);
                Assert.Equal(catalogItem.ProductCode, orderItem.ItemOrdered.ProductCode);
                Assert.Equal(catalogItem.Name, orderItem.ItemOrdered.ProductName);
                Assert.Equal(catalogItem.Id, orderItem.ItemOrdered.CatalogItemId);
            });
    }

    [Fact]
    public void CreateOrder_basketがnullの場合_ArgumentExceptionが発生する()
    {
        // Arrange
        var catalogItems = CreateDefaultCatalogItems();
        var shipTo = CreateDefaultShipTo();
        var factory = new OrderFactory();

        // Act
        var action = () => factory.CreateOrder(null!, catalogItems, shipTo);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void CreateOrder_catalogItemsがnullの場合_ArgumentExceptionが発生する()
    {
        // Arrange
        var basket = new Basket() { BuyerId = "dummyId" };
        basket.AddItem(1L, 1000m, 2);
        var shipTo = CreateDefaultShipTo();
        var factory = new OrderFactory();

        // Act
        var action = () => factory.CreateOrder(basket, null!, shipTo);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void CreateOrder_shipToAddressがnullの場合_ArgumentExceptionが発生する()
    {
        // Arrange
        var basket = new Basket() { BuyerId = "dummyId" };
        basket.AddItem(1L, 1000m, 2);
        var catalogItems = CreateDefaultCatalogItems();
        var factory = new OrderFactory();

        // Act
        var action = () => factory.CreateOrder(basket, catalogItems, null!);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
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

    private static IReadOnlyList<CatalogItem> CreateDefaultCatalogItems()
    {
        var catalog = new List<CatalogItem>()
        {
            new() { CatalogCategoryId = 1L, CatalogBrandId = 3L, Description = "定番の無地ロングTシャツです。", Name = "クルーネック Tシャツ - ブラック", Price = 1980m, ProductCode = "C000000001", Id = 1L, RowVersion = [255] },
            new() { CatalogCategoryId = 1L, CatalogBrandId = 2L, Description = "暖かいのに着膨れしない起毛デニムです。", Name = "裏起毛 スキニーデニム", Price = 4800m, ProductCode = "C000000002", Id = 2L, RowVersion = [255] },
            new() { CatalogCategoryId = 1L, CatalogBrandId = 1L, Description = "あたたかく肌ざわりも良いウール100%のロングコートです。", Name = "ウールコート", Price = 49800m, ProductCode = "C000000003", Id = 3L, RowVersion = [255] },
        };
        return catalog;
    }
}
