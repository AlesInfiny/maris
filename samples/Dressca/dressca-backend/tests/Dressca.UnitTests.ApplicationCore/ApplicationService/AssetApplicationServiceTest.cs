using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.Assets;

namespace Dressca.UnitTests.ApplicationCore.ApplicationService;

public class AssetApplicationServiceTest(ITestOutputHelper testOutputHelper) : TestBase(testOutputHelper)
{
    [Fact]
    public async Task GetAssetStreamInfoAsync_リポジトリに指定したアセットコードの情報が見つからない_AssetNotFoundExceptionが発生する()
    {
        // Arrange
        var assetCode = "dummy";
        var repositoryMock = new Mock<IAssetRepository>();
        repositoryMock
            .Setup(r => r.FindAsync(assetCode))
            .ReturnsAsync((Asset?)null);
        var store = Mock.Of<IAssetStore>();
        var logger = this.CreateTestLogger<AssetApplicationService>();
        var service = new AssetApplicationService(repositoryMock.Object, store, logger);

        // Act
        var action = () => service.GetAssetStreamInfoAsync(assetCode);

        // Assert
        var ex = await Assert.ThrowsAsync<AssetNotFoundException>(action);
        Assert.Contains("アセットコード: dummy のアセットが見つかりませんでした。", ex.Message);
    }

    [Fact]
    public async Task GetAssetStreamInfoAsync_ストアに指定したアセットコードのストリームが見つからない場合_AssetNotFoundExceptionが発生する()
    {
        // Arrange
        var assetCode = "dummy";
        var asset = new Asset { AssetCode = assetCode, AssetType = AssetTypes.Png };
        var repositoryMock = new Mock<IAssetRepository>();
        repositoryMock
            .Setup(r => r.FindAsync(assetCode))
            .ReturnsAsync(asset);
        var storeMock = new Mock<IAssetStore>();
        storeMock
            .Setup(s => s.GetStream(asset))
            .Returns((Stream?)null);
        var logger = this.CreateTestLogger<AssetApplicationService>();
        var service = new AssetApplicationService(repositoryMock.Object, storeMock.Object, logger);

        // Act
        var action = () => service.GetAssetStreamInfoAsync(assetCode);

        // Assert
        var ex = await Assert.ThrowsAsync<AssetNotFoundException>(action);
        Assert.Contains("アセットコード: dummy のアセットが見つかりませんでした。", ex.Message);
    }

    [Fact]
    public async Task GetAssetStreamInfoAsync_リポジトリから取得したアセット情報とストアから取得したストリームを取得できる()
    {
        // Arrange
        var assetCode = "assetCode";
        var asset = new Asset { AssetCode = assetCode, AssetType = AssetTypes.Png };
        var repositoryMock = new Mock<IAssetRepository>();
        repositoryMock
            .Setup(r => r.FindAsync(assetCode))
            .ReturnsAsync(asset);
        var storeMock = new Mock<IAssetStore>();
        var stream = new MemoryStream();
        storeMock
            .Setup(s => s.GetStream(asset))
            .Returns(stream);
        var logger = this.CreateTestLogger<AssetApplicationService>();
        var service = new AssetApplicationService(repositoryMock.Object, storeMock.Object, logger);

        // Act
        var assetStreamInfo = await service.GetAssetStreamInfoAsync(assetCode);

        // Assert
        Assert.Same(asset, assetStreamInfo.Asset);
        Assert.Same(stream, assetStreamInfo.Stream);
    }
}
