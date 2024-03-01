using Dressca.ApplicationCore.Assets;
using Dressca.Web.Assets;

namespace Dressca.UnitTests.Web.Assets;

public class AssetExtensionsTest
{
    [Fact]
    public void GetContentType_アセットがnull_ArgumentNullExceptionが発生する()
    {
        // Arrange
        Asset? asset = null;

        // Act
        var action = () => AssetExtensions.GetContentType(asset!);

        // Assert
        Assert.Throws<ArgumentNullException>("asset", action);
    }

    [Fact]
    public void GetContentType_アセットタイプがPNG_imagepngを取得できる()
    {
        // Arrange
        Asset asset = new Asset("asset-code", AssetTypes.Png);

        // Act
        var contentType = AssetExtensions.GetContentType(asset);

        // Assert
        Assert.Equal("image/png", contentType);
    }
}
