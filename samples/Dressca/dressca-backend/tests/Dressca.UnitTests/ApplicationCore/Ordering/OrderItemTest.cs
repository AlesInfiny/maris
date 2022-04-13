using Dressca.ApplicationCore.Ordering;

namespace Dressca.UnitTests.ApplicationCore.Ordering;

public class OrderItemTest
{
    [Fact]
    public void 注文されたカタログアイテムがnullの場合例外()
    {
        // Arrange
        CatalogItemOrdered? itemOrdered = null;
        decimal unitPrice = 1000m;
        int quantity = 1;

        // Act
        var action = () => new OrderItem(itemOrdered!, unitPrice, quantity);

        // Assert
        Assert.Throws<ArgumentNullException>("value", action);
    }

    [Fact]
    public void 注文情報が初期化されていない場合例外()
    {
        // Arrange
        CatalogItemOrdered itemOrdered = new CatalogItemOrdered(1L, "製品1", "A00000001");
        decimal unitPrice = 1000m;
        int quantity = 1;
        var orderItem = new OrderItem(itemOrdered, unitPrice, quantity);

        // Act
        var action = () => _ = orderItem.Order;

        // Assert
        var ex = Assert.Throws<InvalidOperationException>(action);
        Assert.Equal("Order プロパティが初期化されていません。", ex.Message);
    }

    [Fact]
    public void 注文アイテムアセットにnullを追加しようとすると例外()
    {
        // Arrange
        CatalogItemOrdered itemOrdered = new CatalogItemOrdered(1L, "製品1", "A00000001");
        decimal unitPrice = 1000m;
        int quantity = 1;
        var orderItem = new OrderItem(itemOrdered, unitPrice, quantity);
        IEnumerable<OrderItemAsset>? orderItemAssets = null;

        // Act
        var action = () => orderItem.AddAssets(orderItemAssets!);

        // Assert
        Assert.Throws<ArgumentNullException>("orderItemAssets", action);
    }

    [Fact]
    public void 注文アイテムアセットに追加した情報が取得できる()
    {
        // Arrange
        CatalogItemOrdered itemOrdered = new CatalogItemOrdered(1L, "製品1", "A00000001");
        decimal unitPrice = 1000m;
        int quantity = 1;
        var orderItem = new OrderItem(itemOrdered, unitPrice, quantity);
        var orderItemAssets = new List<OrderItemAsset>
        {
            new("asset-code-1", orderItem.Id),
            new("asset-code-2", orderItem.Id),
        };

        // Act
        orderItem.AddAssets(orderItemAssets);

        // Assert
        Assert.Collection(
            orderItem.Assets,
            itemAsset => Assert.Equal("asset-code-1", itemAsset.AssetCode),
            itemAsset => Assert.Equal("asset-code-2", itemAsset.AssetCode));
    }

    [Fact]
    public void 注文アイテムの小計を取得できる()
    {
        // Arrange
        CatalogItemOrdered itemOrdered = new CatalogItemOrdered(1L, "製品1", "A00000001");
        decimal unitPrice = 1000m;
        int quantity = 2;
        var orderItem = new OrderItem(itemOrdered, unitPrice, quantity);

        // Act
        var subTotal = orderItem.GetSubTotal();

        // Assert
        Assert.Equal(2000m, subTotal);
    }
}
