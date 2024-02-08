using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.Assets;
using Maris.Logging.Testing.Xunit;
using Xunit.Abstractions;

namespace Dressca.UnitTests.ApplicationCore.Assets;

public class AssetApplicationServiceTest
{
    private readonly TestLoggerManager loggerManager;

    public AssetApplicationServiceTest(ITestOutputHelper testOutputHelper)
        => this.loggerManager = new TestLoggerManager(testOutputHelper);

    [Fact]
    public void GetAssetStreamInfo_リポジトリに指定したアセットコードの情報が見つからない場合は例外()
    {
        // Arrange
        var assetCode = "dummy";
        var repositoryMock = new Mock<IAssetRepository>();
        repositoryMock
            .Setup(r => r.Find(assetCode))
            .Returns((Asset?)null);
        var store = Mock.Of<IAssetStore>();
        var logger = this.loggerManager.CreateLogger<AssetApplicationService>();
        var service = new AssetApplicationService(repositoryMock.Object, store, logger);

        // Act
        var action = () => service.GetAssetStreamInfo(assetCode);

        // Assert
        var ex = Assert.Throws<AssetNotFoundException>(action);
        Assert.Equal("アセットコード: dummy のアセットが見つかりませんでした。", ex.Message);
    }

    [Fact]
    public void GetAssetStreamInfo_ストアに指定したアセットコードのストリームが見つからない場合は例外()
    {
        // Arrange
        var assetCode = "dummy";
        var asset = new Asset(assetCode, AssetTypes.Png);
        var repositoryMock = new Mock<IAssetRepository>();
        repositoryMock
            .Setup(r => r.Find(assetCode))
            .Returns(asset);
        var storeMock = new Mock<IAssetStore>();
        storeMock
            .Setup(s => s.GetStream(asset))
            .Returns((Stream?)null);
        var logger = this.loggerManager.CreateLogger<AssetApplicationService>();
        var service = new AssetApplicationService(repositoryMock.Object, storeMock.Object, logger);

        // Act
        var action = () => service.GetAssetStreamInfo(assetCode);

        // Assert
        var ex = Assert.Throws<AssetNotFoundException>(action);
        Assert.Equal("アセットコード: dummy のアセットが見つかりませんでした。", ex.Message);
    }

    [Fact]
    public void GetAssetStreamInfo_リポジトリから取得したアセット情報とストアから取得したストリームを取得できる()
    {
        // Arrange
        var assetCode = "assetCode";
        var asset = new Asset(assetCode, AssetTypes.Png);
        var repositoryMock = new Mock<IAssetRepository>();
        repositoryMock
            .Setup(r => r.Find(assetCode))
            .Returns(asset);
        var storeMock = new Mock<IAssetStore>();
        var stream = new MemoryStream();
        storeMock
            .Setup(s => s.GetStream(asset))
            .Returns(stream);
        var logger = this.loggerManager.CreateLogger<AssetApplicationService>();
        var service = new AssetApplicationService(repositoryMock.Object, storeMock.Object, logger);

        // Act
        var assetStreamInfo = service.GetAssetStreamInfo(assetCode);

        // Assert
        Assert.Same(asset, assetStreamInfo.Asset);
        Assert.Same(stream, assetStreamInfo.Stream);
    }
}
