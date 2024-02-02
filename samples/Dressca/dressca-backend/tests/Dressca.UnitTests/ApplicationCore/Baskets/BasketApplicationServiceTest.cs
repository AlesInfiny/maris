using Dressca.ApplicationCore.Baskets;
using Maris.Diagnostics.Testing.Xunit;
using Xunit.Abstractions;

namespace Dressca.UnitTests.ApplicationCore.Baskets;

/// <summary>
///  買い物かごアプリケーションサービスの単体テストです。
/// </summary>
public class BasketApplicationServiceTest
{
    private readonly TestLoggerManager loggerManager;

    public BasketApplicationServiceTest(ITestOutputHelper testOutputHelper)
        => this.loggerManager = new TestLoggerManager(testOutputHelper);

    private static CancellationToken AnyToken => It.IsAny<CancellationToken>();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetOrCreateBasketForUserAsync_買い物かごの取得処理で購入者Idがnullまたは空白なら例外が発生する(string? nullOrEmptyBuyerId)
    {
        // Arrange
        var repo = Mock.Of<IBasketRepository>();
        var logger = this.loggerManager.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repo, logger);

        // Act
        var action = () => service.GetOrCreateBasketForUserAsync(nullOrEmptyBuyerId!);

        // Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>("buyerId", action);
        Assert.StartsWith("null または空の文字列を設定できません。", ex.Message);
    }

    [Fact]
    public async Task GetOrCreateBasketForUserAsync_買い物かごの取得処理で購入者Idに対応する買い物かご情報が存在しない場合買い物かごの作成処理としてリポジトリのGetWithBasketItemsを1度だけ呼出す()
    {
        // Arrange
        const string dummyBuyerId = "dummyId";
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync((Basket?)null);
        var logger = this.loggerManager.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        var basket = await service.GetOrCreateBasketForUserAsync(dummyBuyerId);

        // Assert
        repoMock.Verify(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken), Times.Once);
    }

    [Fact]
    public async Task GetOrCreateBasketForUserAsync_買い物かごの取得処理で購入者Idに対応する買い物かご情報が存在しない場合買い物かごの作成処理としてリポジトリのAddAsyncを1度だけ呼出す()
    {
        // Arrange
        const string buyerId = "not-exists-Id";
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(buyerId, AnyToken))
            .ReturnsAsync((Basket?)null);
        var logger = this.loggerManager.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        var basket = await service.GetOrCreateBasketForUserAsync(buyerId);

        // Assert
        repoMock.Verify(
            r => r.AddAsync(
                It.Is<Basket>(b => b.BuyerId == buyerId),
                AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task GetOrCreateBasketForUserAsync_買い物かごの取得処理で購入者Idに対応する買い物かご情報が存在しない場合AddAsyncで生成した買い物かごを取得できる()
    {
        // Arrange
        const string buyerId = "not-exists-Id";
        var newBasket = new Basket(buyerId);
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(buyerId, AnyToken))
            .ReturnsAsync((Basket?)null);
        repoMock
            .Setup(r => r.AddAsync(It.IsAny<Basket>(), AnyToken))
            .ReturnsAsync(newBasket);
        var logger = this.loggerManager.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        var actual = await service.GetOrCreateBasketForUserAsync(buyerId);

        // Assert
        Assert.Same(newBasket, actual);
    }

    [Fact]
    public async Task GetOrCreateBasketForUserAsync_買い物かごの取得処理はリポジトリのGetWithBasketItemsを1度だけ呼出す()
    {
        // Arrange
        var buyerId = Guid.NewGuid().ToString("D");
        var repoMock = new Mock<IBasketRepository>();
        var logger = this.loggerManager.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        var basket = await service.GetOrCreateBasketForUserAsync(buyerId);

        // Assert
        repoMock.Verify(r => r.GetWithBasketItemsAsync(buyerId, AnyToken), Times.Once);
    }

    [Fact]
    public async Task GetOrCreateBasketForUserAsync_買い物かごが取得できたときはリポジトリのAddAsyncを呼び出さない()
    {
        // Arrange
        var buyerId = Guid.NewGuid().ToString("D");
        var basket = new Basket(buyerId);
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(buyerId, AnyToken))
            .ReturnsAsync(basket);
        var logger = this.loggerManager.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        _ = await service.GetOrCreateBasketForUserAsync(buyerId);

        // Assert
        repoMock.Verify(r => r.AddAsync(It.IsAny<Basket>(), AnyToken), Times.Never);
    }

    [Fact]
    public async Task GetOrCreateBasketForUserAsync_買い物かごの取得処理はリポジトリのGetWithBasketItemsから取得した買い物かごの情報を返す()
    {
        // Arrange
        var buyerId = Guid.NewGuid().ToString("D");
        var basket = new Basket(buyerId);
        basket.AddItem(1L, 100m);
        basket.AddItem(2L, 200m);
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(buyerId, AnyToken))
            .ReturnsAsync(basket);
        var logger = this.loggerManager.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        var actual = await service.GetOrCreateBasketForUserAsync(buyerId);

        // Assert
        Assert.Equal(buyerId, actual.BuyerId);
        Assert.Collection(
            actual.Items,
            basketItem => Assert.Equal(1, basketItem.CatalogItemId),
            basketItem => Assert.Equal(2, basketItem.CatalogItemId));
    }

    [Fact]
    public async Task AddItemToBasketAsync_買い物かごへの商品追加処理はリポジトリのGetAsyncを1度だけ呼出す()
    {
        // Arrange
        const long basketId = 1;
        var basket = new Basket("dummy");
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetAsync(basketId, AnyToken))
            .ReturnsAsync(basket);
        var logger = this.loggerManager.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        await service.AddItemToBasketAsync(basketId, 1, 1000, 1);

        // Assert
        repoMock.Verify(r => r.GetAsync(basketId, AnyToken), Times.Once);
    }

    [Fact]
    public async Task AddItemToBasketAsync_買い物かごへの商品追加処理はリポジトリのUpdateAsyncを1度だけ呼出す()
    {
        // Arrange
        const long basketId = 1;
        var basket = new Basket("dummy");
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetAsync(basketId, AnyToken))
            .ReturnsAsync(basket);
        var logger = this.loggerManager.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        await service.AddItemToBasketAsync(basketId, 1, 1000, 1);

        // Assert
        repoMock.Verify(
            r => r.UpdateAsync(It.Is<Basket>(b => b.BuyerId == basket.BuyerId), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task AddItemToBasketAsync_買い物かごへの商品追加処理で買い物かごが見つからない場合は業務例外が発生する()
    {
        // Arrange
        var repo = Mock.Of<IBasketRepository>();
        var logger = this.loggerManager.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repo, logger);

        // Act
        var action = () => service.AddItemToBasketAsync(1, 1, 1000, 1);

        // Assert
        await Assert.ThrowsAsync<BasketNotFoundException>(action);
    }

    [Fact]
    public async Task AddItemToBasketAsync_買い物かごへの商品追加処理後に数量が0となる場合買い物かごアイテムは削除される()
    {
        // Arrange
        const long basketId = 1;
        const long catalogItemId = 10L;
        var basket = new Basket("dummy");
        basket.AddItem(catalogItemId, 1000m, 1);
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetAsync(basketId, AnyToken))
            .ReturnsAsync(basket);
        var logger = this.loggerManager.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        await service.AddItemToBasketAsync(basketId, catalogItemId, 1000, -1);

        // Assert
        repoMock.Verify(
            r => r.UpdateAsync(It.Is<Basket>(b => !b.Items.Any()), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task DeleteBasketAsync_買い物かごの削除処理はリポジトリのGetWithBasketItemsを1度だけ呼出す()
    {
        // Arrange
        const long basketId = 1;
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(basketId, AnyToken))
            .ReturnsAsync(new Basket("dummy"));
        var logger = this.loggerManager.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        await service.DeleteBasketAsync(basketId);

        // Assert
        repoMock.Verify(r => r.GetWithBasketItemsAsync(basketId, AnyToken), Times.Once);
    }

    [Fact]
    public async Task DeleteBasketAsync_買い物かごの削除処理はリポジトリのRemoveAsyncを1度だけ呼出す()
    {
        // Arrange
        const long basketId = 1;
        var buyerId = Guid.NewGuid().ToString("D");
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(basketId, AnyToken))
            .ReturnsAsync(new Basket(buyerId));
        var logger = this.loggerManager.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        await service.DeleteBasketAsync(basketId);

        // Assert
        repoMock.Verify(
            r => r.RemoveAsync(It.Is<Basket>(b => b.BuyerId == buyerId), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task DeleteBasketAsync_買い物かごの削除処理で買い物かごが見つからない場合は業務例外が発生する()
    {
        // Arrange
        var repo = Mock.Of<IBasketRepository>();
        var logger = this.loggerManager.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repo, logger);

        // Act
        var action = () => service.DeleteBasketAsync(1);

        // Assert
        await Assert.ThrowsAsync<BasketNotFoundException>(action);
    }

    [Fact]
    public async Task SetQuantitiesAsync_数量の設定処理で数量パラメータがnullならArgumentNullExceptionが発生する()
    {
        // Arrange
        var repo = Mock.Of<IBasketRepository>();
        var logger = this.loggerManager.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repo, logger);

        // Act
        var action = () => service.SetQuantitiesAsync(1, null!);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>("quantities", action);
    }

    [Fact]
    public async Task SetQuantitiesAsync_数量の設定処理で買い物かごが見つからないなら業務例外が発生する()
    {
        // Arrange
        var repo = Mock.Of<IBasketRepository>();
        var logger = this.loggerManager.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repo, logger);

        var quantities = new Dictionary<long, int> { { 1L, 1 }, { 2L, 1 }, { 3L, 1 } };

        // Act
        var action = () => service.SetQuantitiesAsync(1, quantities);

        // Assert
        await Assert.ThrowsAsync<BasketNotFoundException>(action);
    }

    [Fact]
    public async Task SetQuantitiesAsync_数量の設定処理はリポジトリのUpdateAsyncを1度だけ呼出す()
    {
        // Arrange
        const long basketId = 1L;
        var buyerId = Guid.NewGuid().ToString("D");
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(basketId, AnyToken))
            .ReturnsAsync(new Basket(buyerId));
        var logger = this.loggerManager.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        var quantities = new Dictionary<long, int> { { 1L, 5 } };

        // Act
        await service.SetQuantitiesAsync(basketId, quantities);

        // Assert
        repoMock.Verify(
            r => r.UpdateAsync(It.Is<Basket>(b => b.BuyerId == buyerId), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task SetQuantitiesAsync_買い物かごに存在しない商品を指定しても買い物かごには追加されない()
    {
        // Arrange
        const long basketId = 1L;
        var buyerId = Guid.NewGuid().ToString("D");
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(basketId, AnyToken))
            .ReturnsAsync(new Basket(buyerId));
        var logger = this.loggerManager.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        var quantities = new Dictionary<long, int> { { 1L, 5 } };

        // Act
        await service.SetQuantitiesAsync(basketId, quantities);

        // Assert
        repoMock.Verify(
            r => r.UpdateAsync(It.Is<Basket>(b => b.Items.Count == 0), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task SetQuantitiesAsync_買い物かごに存在する商品を指定すると買い物かごの商品数が更新される()
    {
        // Arrange
        const long basketId = 1L;
        var buyerId = Guid.NewGuid().ToString("D");
        const int newQuantity = 5;
        var basket = new Basket(buyerId);
        basket.AddItem(100L, 1000m);
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(basketId, AnyToken))
            .ReturnsAsync(basket);
        var logger = this.loggerManager.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        var quantities = new Dictionary<long, int> { { 100L, newQuantity } };

        // Act
        await service.SetQuantitiesAsync(basketId, quantities);

        // Assert
        repoMock.Verify(
            r => r.UpdateAsync(It.Is<Basket>(b => b.Items.First().Quantity == newQuantity), AnyToken),
            Times.Once);
    }
}
