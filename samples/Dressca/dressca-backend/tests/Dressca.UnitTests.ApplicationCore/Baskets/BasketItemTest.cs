using Dressca.ApplicationCore.Baskets;

namespace Dressca.UnitTests.ApplicationCore.Baskets;

public class BasketItemTest
{
    [Fact]
    public void GetSubTotal_買い物かごアイテムの小計額は単価と数量の積になる()
    {
        // Arrange
        var catalogItemId = new Guid("01971a00-0000-7000-d000-000000000001");
        decimal unitPrice = 1000m;
        int quantity = 2;
        var item = new BasketItem { CatalogItemId = catalogItemId, UnitPrice = unitPrice, Quantity = quantity };

        // Act
        var subTotal = item.GetSubTotal();

        // Assert
        Assert.Equal(2000m, subTotal);
    }

    [Fact]
    public void Basket_買い物かごのナビゲーションプロパティが初期化されていない_InvalidOperationExceptionが発生する()
    {
        // Arrange
        var catalogItemId = new Guid("01971a00-0000-7000-d000-000000000001");
        decimal unitPrice = 1000m;
        int quantity = 2;
        var item = new BasketItem { CatalogItemId = catalogItemId, UnitPrice = unitPrice, Quantity = quantity };

        // Act
        var action = () => _ = item.Basket;

        // Assert
        var ex = Assert.Throws<InvalidOperationException>(action);
        Assert.Equal("Basket プロパティが初期化されていません。", ex.Message);
    }
}
