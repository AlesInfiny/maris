using System.Linq;
using Dressca.ApplicationCore.Baskets;
using Dressca.TestLibrary.Xunit.Logging;
using Xunit.Abstractions;

namespace Dressca.UnitTests.ApplicationCore.Baskets;

/// <summary>
///  買い物かごアプリケーションサービスの単体テストです。
/// </summary>
public class BasketApplicationServiceTest
{
    private readonly XunitLoggerFactory loggerFactory;

    public BasketApplicationServiceTest(ITestOutputHelper testOutputHelper)
        => this.loggerFactory = XunitLoggerFactory.Create(testOutputHelper);

    private static CancellationToken AnyToken => It.IsAny<CancellationToken>();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task 買い物かごの取得処理でユーザーIdがnullまたは空白なら例外が発生する(string? userId)
    {
        // Arrange
        var repo = Mock.Of<IBasketRepository>();
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repo, logger);

        // Act
        var action = () => service.GetOrCreateBasketForUser(userId!);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(action);
    }

    [Fact]
    public async Task 買い物かごの取得処理でユーザーIdに対応する買い物かご情報が存在しない場合買い物かごの作成処理としてリポジトリのGetWithBasketItemsを1度だけ呼出す()
    {
        // Arrange
        const string dummyId = "dummyId";
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(dummyId, AnyToken))
            .ReturnsAsync((Basket?)null);
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        var basket = await service.GetOrCreateBasketForUser(dummyId);

        // Assert
        repoMock.Verify(r => r.GetWithBasketItemsAsync(dummyId, AnyToken), Times.Once);
    }

    [Fact]
    public async Task 買い物かごの取得処理でユーザーIdに対応する買い物かご情報が存在しない場合買い物かごの作成処理としてリポジトリのAddAsyncを1度だけ呼出す()
    {
        // Arrange
        const string userId = "not-exists-Id";
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(userId, AnyToken))
            .ReturnsAsync((Basket?)null);
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        var basket = await service.GetOrCreateBasketForUser(userId);

        // Assert
        repoMock.Verify(
            r => r.AddAsync(
                It.Is<Basket>(b => b.BuyerId == userId),
                AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task 買い物かごの取得処理でユーザーIdに対応する買い物かご情報が存在しない場合AddAsyncで生成した買い物かごを取得できる()
    {
        // Arrange
        const string userId = "not-exists-Id";
        var newBasket = new Basket(userId);
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(userId, AnyToken))
            .ReturnsAsync((Basket?)null);
        repoMock
            .Setup(r => r.AddAsync(It.IsAny<Basket>(), AnyToken))
            .ReturnsAsync(newBasket);
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        var actual = await service.GetOrCreateBasketForUser(userId);

        // Assert
        Assert.Same(newBasket, actual);
    }

    [Fact]
    public async Task 買い物かごの取得処理はリポジトリのGetWithBasketItemsを1度だけ呼出す()
    {
        // Arrange
        const string userId = "user1";
        var repoMock = new Mock<IBasketRepository>();
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        var basket = await service.GetOrCreateBasketForUser(userId);

        // Assert
        repoMock.Verify(r => r.GetWithBasketItemsAsync(userId, AnyToken), Times.Once);
    }

    [Fact]
    public async Task 買い物かごが取得できたときはリポジトリのAddAsyncを呼び出さない()
    {
        // Arrange
        const string userId = "user1";
        var basket = new Basket(userId);
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(userId, AnyToken))
            .ReturnsAsync(basket);
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        _ = await service.GetOrCreateBasketForUser(userId);

        // Assert
        repoMock.Verify(r => r.AddAsync(It.IsAny<Basket>(), AnyToken), Times.Never);
    }

    [Fact]
    public async Task 買い物かごの取得処理はリポジトリのGetWithBasketItemsから取得した買い物かごの情報を返す()
    {
        // Arrange
        const string userId = "user1";
        var basket = new Basket(userId);
        basket.AddItem(1L, 100m);
        basket.AddItem(2L, 200m);
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(userId, AnyToken))
            .ReturnsAsync(basket);
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        var actual = await service.GetOrCreateBasketForUser(userId);

        // Assert
        Assert.Equal(userId, actual.BuyerId);
        Assert.Collection(
            actual.Items,
            basketItem => Assert.Equal(1, basketItem.CatalogItemId),
            basketItem => Assert.Equal(2, basketItem.CatalogItemId));
    }

    [Fact]
    public async Task 買い物かごへの商品追加処理はリポジトリのGetAsyncを1度だけ呼出す()
    {
        // Arrange
        const long basketId = 1;
        var basket = new Basket("dummy");
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetAsync(basketId, AnyToken))
            .ReturnsAsync(basket);
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        await service.AddItemToBasket(basketId, 1, 1000, 1);

        // Assert
        repoMock.Verify(r => r.GetAsync(basketId, AnyToken), Times.Once);
    }

    [Fact]
    public async Task 買い物かごへの商品追加処理はリポジトリのUpdateAsyncを1度だけ呼出す()
    {
        // Arrange
        const long basketId = 1;
        var basket = new Basket("dummy");
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetAsync(basketId, AnyToken))
            .ReturnsAsync(basket);
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        await service.AddItemToBasket(basketId, 1, 1000, 1);

        // Assert
        repoMock.Verify(
            r => r.UpdateAsync(It.Is<Basket>(b => b.BuyerId == basket.BuyerId), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task 買い物かごへの商品追加処理で買い物かごが見つからない場合は業務例外が発生する()
    {
        // Arrange
        var repo = Mock.Of<IBasketRepository>();
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repo, logger);

        // Act
        var action = () => service.AddItemToBasket(1, 1, 1000, 1);

        // Assert
        await Assert.ThrowsAsync<BasketNotFoundException>(action);
    }

    [Fact]
    public async Task 買い物かごの削除処理はリポジトリのGetWithBasketItemsを1度だけ呼出す()
    {
        // Arrange
        const long basketId = 1;
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(basketId, AnyToken))
            .ReturnsAsync(new Basket("dummy"));
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        await service.DeleteBasket(basketId);

        // Assert
        repoMock.Verify(r => r.GetWithBasketItemsAsync(basketId, AnyToken), Times.Once);
    }

    [Fact]
    public async Task 買い物かごの削除処理はリポジトリのRemoveAsyncを1度だけ呼出す()
    {
        // Arrange
        const long basketId = 1;
        const string buyerId = "user1";
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(basketId, AnyToken))
            .ReturnsAsync(new Basket(buyerId));
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        await service.DeleteBasket(basketId);

        // Assert
        repoMock.Verify(
            r => r.RemoveAsync(It.Is<Basket>(b => b.BuyerId == buyerId), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task 買い物かごの削除処理で買い物かごが見つからない場合は業務例外が発生する()
    {
        // Arrange
        var repo = Mock.Of<IBasketRepository>();
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repo, logger);

        // Act
        var action = () => service.DeleteBasket(1);

        // Assert
        await Assert.ThrowsAsync<BasketNotFoundException>(action);
    }

    [Fact]
    public async Task 数量の設定処理で数量パラメータがnullならArgumentNullExceptionが発生する()
    {
        // Arrange
        var repo = Mock.Of<IBasketRepository>();
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repo, logger);

        // Act
        var action = () => service.SetQuantities(1, null!);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>("quantities", action);
    }

    [Fact]
    public async Task 数量の設定処理で買い物かごが見つからないなら業務例外が発生する()
    {
        // Arrange
        var repo = Mock.Of<IBasketRepository>();
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repo, logger);

        var quantities = new Dictionary<long, int> { { 1L, 1 }, { 2L, 1 }, { 3L, 1 } };

        // Act
        var action = () => service.SetQuantities(1, quantities);

        // Assert
        await Assert.ThrowsAsync<BasketNotFoundException>(action);
    }

    [Fact]
    public async Task 数量の設定処理はリポジトリのUpdateAsyncを1度だけ呼出す()
    {
        // Arrange
        const long basketId = 1L;
        const string buyerId = "user1";
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(basketId, AnyToken))
            .ReturnsAsync(new Basket(buyerId));
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        var quantities = new Dictionary<long, int> { { 1L, 5 } };

        // Act
        await service.SetQuantities(basketId, quantities);

        // Assert
        repoMock.Verify(
            r => r.UpdateAsync(It.Is<Basket>(b => b.BuyerId == buyerId), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task 買い物かごに存在しない商品を指定しても買い物かごには追加されない()
    {
        // Arrange
        const long basketId = 1L;
        const string buyerId = "user1";
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(basketId, AnyToken))
            .ReturnsAsync(new Basket(buyerId));
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        var quantities = new Dictionary<long, int> { { 1L, 5 } };

        // Act
        await service.SetQuantities(basketId, quantities);

        // Assert
        repoMock.Verify(
            r => r.UpdateAsync(It.Is<Basket>(b => b.Items.Count == 0), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task 買い物かごに存在する商品を指定すると買い物かごの商品数が更新される()
    {
        // Arrange
        const long basketId = 1L;
        const string buyerId = "user1";
        const int newQuantity = 5;
        var basket = new Basket(buyerId);
        basket.AddItem(100L, 1000m);
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(basketId, AnyToken))
            .ReturnsAsync(basket);
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        var quantities = new Dictionary<long, int> { { 100L, newQuantity } };

        // Act
        await service.SetQuantities(basketId, quantities);

        // Assert
        repoMock.Verify(
            r => r.UpdateAsync(It.Is<Basket>(b => b.Items.First().Quantity == newQuantity), AnyToken),
            Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task 買い物かごの移動処理で匿名ユーザーIdがnullまたは空文字ならArgumentExceptionが発生する(string? anonymousId)
    {
        // Arrange
        var repo = Mock.Of<IBasketRepository>();
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repo, logger);

        const string userId = "user1";

        // Act
        var action = () => service.TransferBasket(anonymousId!, userId);

        // Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>("anonymousId", action);
        Assert.StartsWith("null または空の文字列を設定できません。", ex.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task 買い物かごの移動処理で移動先ユーザーIdがnullまたは空文字ならArgumentExceptionが発生する(string? userId)
    {
        // Arrange
        var repo = Mock.Of<IBasketRepository>();
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repo, logger);

        const string anonymousId = "anonymous1";

        // Act
        var action = () => service.TransferBasket(anonymousId, userId!);

        // Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>("userId", action);
        Assert.StartsWith("null または空の文字列を設定できません。", ex.Message);
    }

    [Fact]
    public async Task 買い物かごの移動処理で匿名ユーザーの買い物かごが見つからないなら買い物かごの取得処理は1度しか呼出されない()
    {
        // Arrange
        var repoMock = new Mock<IBasketRepository>();
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        const string anonymousId = "anonymous1";
        const string userId = "user1";

        // Act
        await service.TransferBasket(anonymousId, userId);

        // Assert
        repoMock.Verify(r => r.GetWithBasketItemsAsync(anonymousId, AnyToken), Times.Once);
    }

    [Fact]
    public async Task 買い物かごの移動処理で移動先ユーザーの買い物かごが見つからないなら買い物かごの作成処理としてリポジトリのAddAsyncを1度だけ呼出す()
    {
        // Arrange
        const string anonymousId = "anonymous1";
        const string userId = "user1";
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(anonymousId, AnyToken))
            .ReturnsAsync(new Basket(anonymousId));
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        await service.TransferBasket(anonymousId, userId);

        // Assert
        repoMock.Verify(
            r => r.AddAsync(It.Is<Basket>(b => b.BuyerId == userId), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task 買い物かごの移動処理で移動先ユーザーの買い物かごが見つからないなら買い物かごの作成処理としてリポジトリのGetWithBasketItemsを2度呼出す()
    {
        // Arrange
        const string anonymousId = "anonymous1";
        const string userId = "user1";
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(anonymousId, AnyToken))
            .ReturnsAsync(new Basket(anonymousId));
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        await service.TransferBasket(anonymousId, userId);

        // Assert
        repoMock.Verify(r => r.GetWithBasketItemsAsync(anonymousId, AnyToken), Times.Once);
        repoMock.Verify(r => r.GetWithBasketItemsAsync(userId, AnyToken), Times.Once);
    }

    [Fact]
    public async Task 買い物かごの移動処理で移動先ユーザーの買い物かごが見つからないなら買い物かごの作成処理としてリポジトリのUpdateAsyncを1度だけ呼出す()
    {
        // Arrange
        const string anonymousId = "anonymous1";
        const string userId = "user1";
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(anonymousId, AnyToken))
            .ReturnsAsync(new Basket(anonymousId));
        repoMock
            .Setup(r => r.AddAsync(It.Is<Basket>(b => b.BuyerId == userId), AnyToken))
            .ReturnsAsync(new Basket(userId));
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        await service.TransferBasket(anonymousId, userId);

        // Assert
        repoMock.Verify(
            r => r.UpdateAsync(It.Is<Basket>(b => b.BuyerId == userId), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task 買い物かごの移動処理で移動先ユーザーの買い物かごが見つからないなら買い物かごの作成処理としてリポジトリのRemoveAsyncを1度だけ呼出す()
    {
        // Arrange
        const string anonymousId = "anonymous1";
        const string userId = "user1";
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(anonymousId, AnyToken))
            .ReturnsAsync(new Basket(anonymousId));
        repoMock
            .Setup(r => r.AddAsync(It.Is<Basket>(b => b.BuyerId == userId), AnyToken))
            .ReturnsAsync(new Basket(userId));
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        await service.TransferBasket(anonymousId, userId);

        // Assert
        repoMock.Verify(
            r => r.RemoveAsync(It.Is<Basket>(b => b.BuyerId == anonymousId), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task 買い物かごの移動処理はリポジトリのGetWithBasketItemsを2度呼出す()
    {
        // Arrange
        const string anonymousId = "anonymous1";
        const string userId = "user1";
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(anonymousId, AnyToken))
            .ReturnsAsync(new Basket(anonymousId));
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(userId, AnyToken))
            .ReturnsAsync(new Basket(userId));
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        await service.TransferBasket(anonymousId, userId);

        // Assert
        repoMock.Verify(r => r.GetWithBasketItemsAsync(anonymousId, AnyToken), Times.Once);
        repoMock.Verify(r => r.GetWithBasketItemsAsync(userId, AnyToken), Times.Once);
    }

    [Fact]
    public async Task 買い物かごの移動処理はリポジトリのUpdateAsyncを1度だけ呼出す()
    {
        // Arrange
        const string anonymousId = "anonymous1";
        const string userId = "user1";
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(anonymousId, AnyToken))
            .ReturnsAsync(new Basket(anonymousId));
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(userId, AnyToken))
            .ReturnsAsync(new Basket(userId));
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        await service.TransferBasket(anonymousId, userId);

        // Assert
        repoMock.Verify(
            r => r.UpdateAsync(It.Is<Basket>(b => b.BuyerId == userId), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task 買い物かごの移動処理はリポジトリのRemoveAsyncを1度だけ呼出す()
    {
        // Arrange
        const string anonymousId = "anonymous1";
        const string userId = "user1";
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(anonymousId, AnyToken))
            .ReturnsAsync(new Basket(anonymousId));
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(userId, AnyToken))
            .ReturnsAsync(new Basket(userId));
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        await service.TransferBasket(anonymousId, userId);

        // Assert
        repoMock.Verify(
            r => r.RemoveAsync(It.Is<Basket>(b => b.BuyerId == anonymousId), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task 匿名ユーザーの買い物かごの情報が別のユーザーの買い物かごに統合される()
    {
        // Arrange
        const string anonymousId = "anonymous1";
        const string userId = "user1";
        const long catalogItemId = 10L;
        const decimal price = 1000m;
        const int anonymous_quantity = 5;
        var anonymousBasket = new Basket(anonymousId);
        anonymousBasket.AddItem(catalogItemId, price, anonymous_quantity);
        const int user_quantity = 2;
        var userBasket = new Basket(userId);
        userBasket.AddItem(catalogItemId, price, user_quantity);
        var repoMock = new Mock<IBasketRepository>();
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(anonymousId, AnyToken))
            .ReturnsAsync(anonymousBasket);
        repoMock
            .Setup(r => r.GetWithBasketItemsAsync(userId, AnyToken))
            .ReturnsAsync(userBasket);
        var logger = this.loggerFactory.CreateLogger<BasketApplicationService>();
        var service = new BasketApplicationService(repoMock.Object, logger);

        // Act
        await service.TransferBasket(anonymousId, userId);

        // Assert
        repoMock.Verify(
            r => r.UpdateAsync(
                It.Is<Basket>(b =>
                    b.Items.First().CatalogItemId == catalogItemId &&
                    b.Items.First().UnitPrice == price &&
                    b.Items.First().Quantity == anonymous_quantity + user_quantity),
                AnyToken),
            Times.Once);
    }
}
