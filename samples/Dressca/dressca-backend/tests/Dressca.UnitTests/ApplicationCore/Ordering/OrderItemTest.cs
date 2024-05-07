using Dressca.ApplicationCore.Ordering;

namespace Dressca.UnitTests.ApplicationCore.Ordering;

public class OrderItemTest
{
    [Fact]
    public void Order_注文情報が初期化されていない_InvalidOperationExceptionが発生する()
    {
        // Arrange
        CatalogItemOrdered itemOrdered = new CatalogItemOrdered(1L, "製品1", "A00000001");
        decimal unitPrice = 1000m;
        int quantity = 1;
        var orderItem = new OrderItem { ItemOrdered = itemOrdered, UnitPrice = unitPrice, Quantity = quantity };

        // Act
        var action = () => _ = orderItem.Order;

        // Assert
        var ex = Assert.Throws<InvalidOperationException>(action);
        Assert.Equal("Order プロパティが初期化されていません。", ex.Message);
    }

    [Fact]
    public void AddAssets_注文アイテムアセットにnullを追加する_ArgumentNullExceptionが発生する()
    {
        // Arrange
        CatalogItemOrdered itemOrdered = new CatalogItemOrdered(1L, "製品1", "A00000001");
        decimal unitPrice = 1000m;
        int quantity = 1;
        var orderItem = new OrderItem { ItemOrdered = itemOrdered, UnitPrice = unitPrice, Quantity = quantity };
        IEnumerable<OrderItemAsset>? orderItemAssets = null;

        // Act
        var action = () => orderItem.AddAssets(orderItemAssets!);

        // Assert
        Assert.Throws<ArgumentNullException>("orderItemAssets", action);
    }

    [Fact]
    public void AddAssets_注文アイテムアセットに追加した情報が取得できる()
    {
        // Arrange
        CatalogItemOrdered itemOrdered = new CatalogItemOrdered(1L, "製品1", "A00000001");
        decimal unitPrice = 1000m;
        int quantity = 1;
        var orderItem = new OrderItem { ItemOrdered = itemOrdered, UnitPrice = unitPrice, Quantity = quantity };
        var orderItemAssets = new List<OrderItemAsset>
        {
            new() { AssetCode = "asset-code-1", OrderItemId = orderItem.Id },
            new() { AssetCode = "asset-code-2", OrderItemId = orderItem.Id },
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
    public void GetSubTotal_注文アイテムの小計を取得できる()
    {
        // Arrange
        CatalogItemOrdered itemOrdered = new CatalogItemOrdered(1L, "製品1", "A00000001");
        decimal unitPrice = 1000m;
        int quantity = 2;
        var orderItem = new OrderItem { ItemOrdered = itemOrdered, UnitPrice = unitPrice, Quantity = quantity };

        // Act
        var subTotal = orderItem.GetSubTotal();

        // Assert
        Assert.Equal(2000m, subTotal);
    }
}
