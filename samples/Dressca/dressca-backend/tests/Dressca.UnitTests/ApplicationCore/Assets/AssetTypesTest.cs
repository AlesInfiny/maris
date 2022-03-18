using Dressca.ApplicationCore.Assets;

namespace Dressca.UnitTests.ApplicationCore.Assets;

public class AssetTypesTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void アセットタイプがnullまたは空の文字列の場合(string assetType)
    {
        // Arrange: Do Nothing
        // Act
        var isSupported = AssetTypes.IsSupportedAssetType(assetType);

        // Assert
        Assert.False(isSupported);
    }

    [Theory]
    [InlineData(AssetTypes.Png)]
    public void アセットタイプが定義済みの場合(string assetType)
    {
        // Arrange: Do Nothing
        // Act
        var isSupported = AssetTypes.IsSupportedAssetType(assetType);

        // Assert
        Assert.True(isSupported);
    }
}
