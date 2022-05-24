using Dressca.ApplicationCore.Baskets;

namespace Dressca.UnitTests.ApplicationCore.Baskets;

public class BasketItemTest
{
    [Fact]
    public void GetSubTotal_買い物かごアイテムの小計額は単価と数量の積になる()
    {
        // Arrange
        long catalogItemId = 1L;
        decimal unitPrice = 1000m;
        int quantity = 2;
        var item = new BasketItem(catalogItemId, unitPrice, quantity);

        // Act
        var subTotal = item.GetSubTotal();

        // Assert
        Assert.Equal(2000m, subTotal);
    }

    [Fact]
    public void Basket_買い物かごのナビゲーションプロパティが初期化されていない場合例外()
    {
        // Arrange
        long catalogItemId = 1L;
        decimal unitPrice = 1000m;
        int quantity = 2;
        var item = new BasketItem(catalogItemId, unitPrice, quantity);

        // Act
        var action = () => _ = item.Basket;

        // Assert
        var ex = Assert.Throws<InvalidOperationException>(action);
        Assert.Equal("Basket プロパティが初期化されていません。", ex.Message);
    }
}
