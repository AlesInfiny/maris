using Dressca.ApplicationCore.Assets;

namespace Dressca.UnitTests.ApplicationCore.Assets;

public class AssetTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_アセットコードがnullまたは空の文字列の場合例外(string assetCode)
    {
        // Arrange
        var assetType = AssetTypes.Png;

        // Act
        var action = () => new Asset(assetCode, assetType);

        // Assert
        var ex = Assert.Throws<ArgumentException>("value", action);
        Assert.StartsWith("null または空の文字列を設定できません。", ex.Message);
    }

    [Fact]
    public void Constructor_アセットタイプが未知の場合例外()
    {
        // Arrange
        var assetCode = "assetCode";
        var assetType = "NOT-SUPPORTED";

        // Act
        var action = () => new Asset(assetCode, assetType);

        // Assert
        var ex = Assert.Throws<NotSupportedException>(action);
        Assert.Equal("アセットタイプ: NOT-SUPPORTED はサポートされていません。", ex.Message);
    }
}
