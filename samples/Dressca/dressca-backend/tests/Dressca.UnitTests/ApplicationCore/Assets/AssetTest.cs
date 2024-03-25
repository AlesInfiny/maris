using Dressca.ApplicationCore.Assets;

namespace Dressca.UnitTests.ApplicationCore.Assets;

public class AssetTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_アセットコードがnullまたは空の文字列_ArgumentExceptionが発生する(string? assetCode)
    {
        // Arrange
        var assetType = AssetTypes.Png;

        // Act
        var action = () => new Asset { AssetCode = assetCode!, AssetType = assetType };

        // Assert
        var ex = Assert.Throws<ArgumentException>("value", action);
        Assert.StartsWith("null または空の文字列を設定できません。", ex.Message);
    }

    [Fact]
    public void Constructor_アセットタイプが未知_NotSupportedExceptionが発生する()
    {
        // Arrange
        var assetCode = "assetCode";
        var assetType = "NOT-SUPPORTED";

        // Act
        var action = () => new Asset { AssetCode = assetCode, AssetType = assetType };

        // Assert
        var ex = Assert.Throws<NotSupportedException>(action);
        Assert.Equal("アセットタイプ: NOT-SUPPORTED はサポートされていません。", ex.Message);
    }
}
