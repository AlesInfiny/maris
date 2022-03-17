using Dressca.ApplicationCore.Baskets;

namespace Dressca.UnitTests.ApplicationCore.Baskets;

public class BasketTest
{
    [Fact]
    public void 正しくインスタンス化できる()
    {
        // Arrange & Act
        var basket = new Basket(Guid.NewGuid().ToString());

        // Assert
        Assert.NotNull(basket);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    public void 商品を1つ追加できる(int quantity)
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());

        // Act
        basket.AddItem(1L, 1000, quantity);

        // Assert
        Assert.Single(basket.Items);
    }

    [Fact]
    public void 複数の商品を追加できる()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());

        // Act
        basket.AddItem(1L, 1000);
        basket.AddItem(2L, 2000);

        // Assert
        Assert.Collection(
            basket.Items,
            item =>
            {
                Assert.Equal(1L, item.CatalogItemId);
                Assert.Equal(1, item.Quantity);
            },
            item =>
            {
                Assert.Equal(2L, item.CatalogItemId);
                Assert.Equal(1, item.Quantity);
            });
    }

    [Fact]
    public void 買い物かご内の商品の数量を加算しても買い物かご内の商品の種類が変わらない()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());

        // Act
        basket.AddItem(1L, 1000);
        basket.AddItem(1L, 1000, 9);

        // Assert
        Assert.Collection(
            basket.Items,
            item =>
            {
                Assert.Equal(1L, item.CatalogItemId);
            });
    }

    [Fact]
    public void 買い物かご内の商品の数量を加算できる()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());

        // Act
        basket.AddItem(1L, 1000);
        basket.AddItem(1L, 1000, 9);

        // Assert
        Assert.Collection(
            basket.Items,
            item =>
            {
                Assert.Equal(1L, item.CatalogItemId);
                Assert.Equal(10, item.Quantity);
            });
    }

    [Theory]
    [InlineData(10, -1)]
    [InlineData(10, -10)]
    public void 買い物かご内の商品の数量を減算しても商品が買い物かご内に残る(int firstQuantity, int additionalQuantity)
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());

        // Act
        basket.AddItem(1L, 1000, firstQuantity);
        basket.AddItem(1L, 1000, additionalQuantity);

        // Assert
        Assert.Single(basket.Items, item => item.CatalogItemId == 1L);
    }

    [Theory]
    [InlineData(10, -1)]
    [InlineData(10, -10)]
    public void 買い物かご内の商品の数量を減算できる(int firstQuantity, int additionalQuantity)
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());

        // Act
        basket.AddItem(1L, 1000, firstQuantity);
        basket.AddItem(1L, 1000, additionalQuantity);

        // Assert
        Assert.Collection(
            basket.Items,
            item =>
            {
                Assert.Equal(firstQuantity + additionalQuantity, item.Quantity);
            });
    }

    [Fact]
    public void 買い物かごにアイテムが1件も存在しないとき数量0のアイテムを除去する()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());

        // Act
        basket.RemoveEmptyItems();

        // Assert
        Assert.Empty(basket.Items);
    }

    [Fact]
    public void 買い物かごに数量1のアイテムが1件存在するとき数量0のアイテムを除去する()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());
        basket.AddItem(1L, 100, 1);

        // Act
        basket.RemoveEmptyItems();

        // Assert
        Assert.Collection(
            basket.Items,
            item =>
            {
                Assert.Equal(1L, item.CatalogItemId);
                Assert.Equal(1, item.Quantity);
            });
    }

    [Fact]
    public void 買い物かごに数量0のアイテムが1件存在するとき数量0のアイテムを除去する()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());
        basket.AddItem(1L, 100, 0);

        // Act
        basket.RemoveEmptyItems();

        // Assert
        Assert.Empty(basket.Items);
    }

    [Fact]
    public void 買い物かごに数量0のアイテムが2件存在するとき数量0のアイテムを除去する()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());
        basket.AddItem(1L, 100, 0);
        basket.AddItem(2L, 200, 0);

        // Act
        basket.RemoveEmptyItems();

        // Assert
        Assert.Empty(basket.Items);
    }

    [Fact]
    public void 買い物かごに数量0のアイテムが1件_数量1のアイテムが1件存在するとき_数量0のアイテムを除去する()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());
        basket.AddItem(1L, 100, 0);
        basket.AddItem(2L, 200, 1);

        // Act
        basket.RemoveEmptyItems();

        // Assert
        Assert.Collection(
            basket.Items,
            item =>
            {
                Assert.Equal(2L, item.CatalogItemId);
                Assert.Equal(1, item.Quantity);
            });
    }

    [Fact]
    public void 買い物かごに数量1のアイテムが2件存在するとき数量0のアイテムを除去する()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());
        basket.AddItem(1L, 100, 1);
        basket.AddItem(2L, 200, 1);

        // Act
        basket.RemoveEmptyItems();

        // Assert
        Assert.Collection(
            basket.Items,
            item =>
            {
                Assert.Equal(1L, item.CatalogItemId);
                Assert.Equal(1, item.Quantity);
            },
            item =>
            {
                Assert.Equal(2L, item.CatalogItemId);
                Assert.Equal(1, item.Quantity);
            });
    }

    [Fact]
    public void 数量0の商品を買い物かごから削除しても数量0でない商品は残る()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());
        basket.AddItem(1L, 1000);
        basket.AddItem(1L, 1000, -1);
        basket.AddItem(2L, 1000, 2);
        basket.AddItem(2L, 1000, -1);

        // Act
        basket.RemoveEmptyItems();

        // Assert
        Assert.Collection(
            basket.Items,
            item =>
            {
                Assert.Equal(2L, item.CatalogItemId);
                Assert.Equal(1, item.Quantity);
            });
    }

    [Fact]
    public void 数量は0未満にできない()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());

        // Act
        var action = () => basket.AddItem(1L, 1000, -1);

        // Assert
        var ex = Assert.Throws<ArgumentException>("value", action);
        Assert.StartsWith("買い物かごアイテムの数量は 0 未満にできません。", ex.Message);
    }

    [Theory]
    [InlineData(10, -11)]
    [InlineData(10, 2147483647)]
    public void 商品の数量を加減算して0未満にはできない(int firstQuantity, int additionalQuantity)
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());
        basket.AddItem(1L, 1000, firstQuantity);

        // Act
        var action = () => basket.AddItem(1L, 1000, additionalQuantity);

        // Assert
        var ex = Assert.Throws<ArgumentException>("value", action);
        Assert.StartsWith("買い物かごアイテムの数量は 0 未満にできません。", ex.Message);
    }

    [Fact]
    public void 購入者Idを後から変更できる()
    {
        // Arrange
        var buyerId1 = Guid.NewGuid().ToString();
        var buyerId2 = Guid.NewGuid().ToString();
        var basket = new Basket(buyerId1);

        // Act
        basket.SetNewBuyerId(buyerId2);

        // Assert
        Assert.Equal(buyerId2, basket.BuyerId);
    }

    [Fact]
    public void 買い物かごの購入者Idはnullにできない()
    {
        // Arrange & Act
        var action = () => new Basket(null!);

        // Assert
        Assert.Throws<ArgumentNullException>("value", action);
    }

    [Fact]
    public void 購入者Idはnullにできない()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());

        // Act
        var action = () => basket.SetNewBuyerId(null!);

        // Assert
        Assert.Throws<ArgumentNullException>("value", action);
    }

    [Fact]
    public void 買い物かご内に存在するカタログアイテムIdを渡す()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());
        basket.AddItem(1L, 1000m);

        // Act
        var result = basket.IsInCatalogItem(1L);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void 買い物かご内に存在しないカタログアイテムIdを渡す()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());
        basket.AddItem(1L, 1000m);

        // Act
        var result = basket.IsInCatalogItem(2L);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void 買い物かごアイテムの情報をもとにした会計情報を取得できる()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());
        basket.AddItem(1L, 1000m, 1);
        basket.AddItem(2L, 1500m, 2);

        // Act
        var account = basket.GetAccount();

        // Assert
        Assert.Equal(4000m, account.GetItemsTotalPrice());
    }
}
