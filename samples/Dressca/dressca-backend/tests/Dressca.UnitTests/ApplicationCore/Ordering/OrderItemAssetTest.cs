using Dressca.ApplicationCore.Ordering;

namespace Dressca.UnitTests.ApplicationCore.Ordering;

public class OrderItemAssetTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_アセットコードがnullまたは空の文字列_ArgumentExceptionが発生する(string? assetCode)
    {
        // Arrange
        var orderItemId = 1L;

        // Act
        var action = () => new OrderItemAsset(assetCode!, orderItemId);

        // Assert
        var ex = Assert.Throws<ArgumentException>("value", action);
        Assert.StartsWith("null または空の文字列を設定できません。", ex.Message);
    }

    [Fact]
    public void OrderItem_注文アイテムが初期化されていない_InvalidOperationExceptionが発生する()
    {
        // Arrange
        string assetCode = "Asset Code";
        var orderItemId = 1L;
        var itemAsset = new OrderItemAsset(assetCode!, orderItemId);

        // Act
        var action = () => _ = itemAsset.OrderItem;

        // Assert
        var ex = Assert.Throws<InvalidOperationException>(action);
        Assert.StartsWith("OrderItem プロパティが初期化されていません。", ex.Message);
    }
}
