using Dressca.ApplicationCore.Baskets;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace Dressca.UnitTests.ApplicationCore.Baskets;

public class BasketTest
{
    [Fact]
    public void Constructor_正しくインスタンス化できる()
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
    public void AddItem_商品を1つ追加できる(int quantity)
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());

        // Act
        basket.AddItem(1L, 1000, quantity);

        // Assert
        Assert.Single(basket.Items);
    }

    [Fact]
    public void AddItem_複数の商品を追加できる()
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
    public void AddItem_買い物かご内の商品の数量を加算しても買い物かご内の商品の種類が変わらない()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());

        // Act
        basket.AddItem(1L, 1000);
        basket.AddItem(1L, 1000, 9);

        // Assert
        Assert.Single(basket.Items, item => item.CatalogItemId == 1L);
    }

    [Fact]
    public void AddItem_買い物かご内の商品の数量を加算できる()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());

        // Act
        basket.AddItem(1L, 1000);
        basket.AddItem(1L, 1000, 9);

        // Assert
        var item = Assert.Single(basket.Items, item => item.CatalogItemId == 1L);
        Assert.Equal(10, item.Quantity);
    }

    [Theory]
    [InlineData(10, -1)]
    [InlineData(10, -10)]
    public void AddItem_買い物かご内の商品の数量を減算しても商品が買い物かご内に残る(int firstQuantity, int additionalQuantity)
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
    public void AddItem_買い物かご内の商品の数量を減算できる(int firstQuantity, int additionalQuantity)
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());

        // Act
        basket.AddItem(1L, 1000, firstQuantity);
        basket.AddItem(1L, 1000, additionalQuantity);

        // Assert
        Assert.Single(basket.Items, item => item.Quantity == firstQuantity + additionalQuantity);
    }

    [Fact]
    public void RemoveEmptyItems_買い物かごにアイテムが1件も存在しない_アイテムは0件のまま()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());

        // Act
        basket.RemoveEmptyItems();

        // Assert
        Assert.Empty(basket.Items);
    }

    [Fact]
    public void RemoveEmptyItems_買い物かごに数量1のアイテムが1件存在する_アイテムは変化しない()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());
        basket.AddItem(1L, 100, 1);

        // Act
        basket.RemoveEmptyItems();

        // Assert
        var item = Assert.Single(basket.Items, item => item.CatalogItemId == 1L);
        Assert.Equal(1, item.Quantity);
    }

    [Fact]
    public void RemoveEmptyItems_買い物かごに数量0のアイテムが1件存在する_数量0のアイテムを除去する()
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
    public void RemoveEmptyItems_買い物かごに数量0のアイテムが2件存在する_数量0のアイテムを除去する()
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
    public void RemoveEmptyItems_買い物かごに数量0のアイテムが1件_数量1のアイテムが1件存在する_数量0のアイテムを除去する()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());
        basket.AddItem(1L, 100, 0);
        basket.AddItem(2L, 200, 1);

        // Act
        basket.RemoveEmptyItems();

        // Assert
        var item = Assert.Single(basket.Items, item => item.CatalogItemId == 2L);
        Assert.Equal(1, item.Quantity);
    }

    [Fact]
    public void RemoveEmptyItems_買い物かごに数量1のアイテムが2件存在する_アイテムは変化しない()
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
    public void RemoveEmptyItems_数量0の商品を買い物かごから削除しても数量0でない商品は残る()
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
        var item = Assert.Single(basket.Items, item => item.CatalogItemId == 2L);
        Assert.Equal(1, item.Quantity);
    }

    [Fact]
    public void AddItem_数量は0未満にする_ArgumentExceptionが発生する()
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
    public void AddItem_商品の数量を加減算して0未満にする_ArgumentExceptionが発生する(int firstQuantity, int additionalQuantity)
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
    public void Constructor_買い物かごの購入者Idをnullにする_ArgumentNullExceptionが発生する()
    {
        // Arrange & Act
        var action = () => new Basket(null!);

        // Assert
        Assert.Throws<ArgumentNullException>("value", action);
    }

    [Fact]
    public void IsInCatalogItem_買い物かご内に存在するカタログアイテムIdを渡す_true()
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
    public void IsInCatalogItem_買い物かご内に存在しないカタログアイテムIdを渡す_false()
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
    public void GetAccount_買い物かごアイテムの情報をもとにした会計情報を取得できる()
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

    [Fact]
    public void IsEmpty_買い物かごアイテムが空_true()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());

        // Act
        var result = basket.IsEmpty();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_買い物かごにアイテムが存在する_false()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());
        basket.AddItem(1L, 1000m);

        // Act
        var result = basket.IsEmpty();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void SetItemsQuantity_買い物かご内に存在するアイテムの数量を指定_数量を設定できる()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());
        basket.AddItem(1L, 1000m);
        var itemId = 1L;
        var newQuantity = 5;
        var quantities = new Dictionary<long, int>() { { itemId, newQuantity } };

        // Act
        basket.SetItemsQuantity(quantities);

        // Assert
        Assert.True(basket.Items.First().Quantity == newQuantity);
    }

    [Fact]
    public void SetItemsQuantity_買い物かご内に存在しないアイテムの数量を指定_買い物かご内にアイテムが追加されない()
    {
        // Arrange
        var basket = new Basket(Guid.NewGuid().ToString());
        var itemId = 1L;
        basket.AddItem(itemId, 1000m);
        var quantities = new Dictionary<long, int>() { { 100L, 5 } };

        // Act
        basket.SetItemsQuantity(quantities);

        // Assert
        Assert.True(basket.Items.Count == 1);
        Assert.True(basket.Items.First().CatalogItemId == itemId);
    }
}
