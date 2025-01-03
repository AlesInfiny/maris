﻿using Dressca.ApplicationCore.Assets;

namespace Dressca.UnitTests.ApplicationCore.Assets;

public class AssetTypesTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void IsSupportedAssetType_アセットタイプがnullまたは空の文字列_false(string? assetType)
    {
        // Arrange: Do Nothing
        // Act
        var isSupported = AssetTypes.IsSupportedAssetType(assetType);

        // Assert
        Assert.False(isSupported);
    }

    [Theory]
    [InlineData(AssetTypes.Png)]
    public void IsSupportedAssetType_アセットタイプが定義済み_true(string assetType)
    {
        // Arrange: Do Nothing
        // Act
        var isSupported = AssetTypes.IsSupportedAssetType(assetType);

        // Assert
        Assert.True(isSupported);
    }
}
