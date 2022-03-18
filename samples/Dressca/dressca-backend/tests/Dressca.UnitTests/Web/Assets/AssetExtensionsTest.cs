using Dressca.ApplicationCore.Assets;
using Dressca.Web.Assets;

namespace Dressca.UnitTests.Web.Assets;

public class AssetExtensionsTest
{
    [Fact]
    public void アセットがnullの場合例外()
    {
        // Arrange
        Asset? asset = null;

        // Act
        var action = () => AssetExtensions.GetContentType(asset!);

        // Assert
        Assert.Throws<ArgumentNullException>("asset", action);
    }

    [Fact]
    public void アセットタイプがPNGの場合()
    {
        // Arrange
        Asset asset = new Asset("asset-code", AssetTypes.Png);

        // Act
        var contentType = AssetExtensions.GetContentType(asset);

        // Assert
        Assert.Equal("image/png", contentType);
    }
}
