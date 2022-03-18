using Dressca.ApplicationCore.Catalog;

namespace Dressca.UnitTests.ApplicationCore.Catalog;

public class CatalogItemAssetTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void アセットコードがnullまたは空の文字列の場合例外(string? assetCode)
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
    public void カタログアイテムIdが0以下の場合例外()
    {
        // Arrange
        string assetCode = "Asset Code";
        var catalogItemId = 0L;

        // Act
        var action = () => new CatalogItemAsset(assetCode!, catalogItemId);

        // Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>("value", action);
        Assert.StartsWith("カタログアイテム ID は 0 以下にできません。", ex.Message);
        Assert.Equal(catalogItemId, ex.ActualValue);
    }

    [Fact]
    public void カタログアイテムが初期化されていない場合例外()
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
