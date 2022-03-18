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
        var orderItem = new OrderItem(itemOrdered!, unitPrice, quantity);

        // Act
        var action = () => _ = orderItem.Order;

        // Assert
        var ex = Assert.Throws<InvalidOperationException>(action);
        Assert.Equal("Order プロパティが初期化されていません。", ex.Message);
    }
}
