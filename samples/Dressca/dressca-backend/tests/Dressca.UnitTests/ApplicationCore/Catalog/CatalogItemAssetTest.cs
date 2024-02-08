using Dressca.ApplicationCore.Catalog;

namespace Dressca.UnitTests.ApplicationCore.Catalog;

public class CatalogItemAssetTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_アセットコードがnullまたは空の文字列_ArgumentExceptionが発生する(string? assetCode)
    {
        // Arrange
        var catalogItemId = 1L;

        // Act
        var action = () => new CatalogItemAsset(assetCode!, catalogItemId);

        // Assert
        var ex = Assert.Throws<ArgumentException>("value", action);
        Assert.StartsWith("null または空の文字列を設定できません。", ex.Message);
    }

    [Fact]
    public void CatalogItem_カタログアイテムが初期化されていない_InvalidOperationExceptionが発生する()
    {
        // Arrange
        string assetCode = "Asset Code";
        var catalogItemId = 1L;
        var itemAsset = new CatalogItemAsset(assetCode!, catalogItemId);

        // Act
        var action = () => _ = itemAsset.CatalogItem;

        // Assert
        var ex = Assert.Throws<InvalidOperationException>(action);
        Assert.StartsWith("CatalogItem プロパティが初期化されていません。", ex.Message);
    }
}
