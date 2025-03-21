﻿using Dressca.ApplicationCore.Assets;
using Dressca.Web.Consumer.Assets;

namespace Dressca.UnitTests.Web.Consumer.Assets;

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
        Asset asset = new Asset { AssetCode = "asset-code", AssetType = AssetTypes.Png };

        // Act
        var contentType = AssetExtensions.GetContentType(asset);

        // Assert
        Assert.Equal("image/png", contentType);
    }
}
