using System.Linq.Expressions;
using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.DisplayItems;
using Dressca.ApplicationCore.Ordering;

namespace Dressca.UnitTests.ApplicationCore.ApplicationService;

public class ShoppingApplicationServiceTest(ITestOutputHelper testOutputHelper) : TestBase(testOutputHelper)
{
    private static CancellationToken AnyToken => It.IsAny<CancellationToken>();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetBasketItemsAsync_購入者Idがnullまたは空白_ArgumentExceptionが発生する(string? nullOrEmptyBuyerId)
    {
        // Arrange
        var basketRepo = Mock.Of<IBasketRepository>();
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var displayItemRepo = Mock.Of<IDisplayItemRepository>();
        var displayItemDomainService = Mock.Of<IDisplayItemDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo, orderRepo, orderFactory, displayItemRepo, displayItemDomainService, logger);

        // Act
        var action = () => service.GetBasketItemsAsync(nullOrEmptyBuyerId!);

        // Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>("buyerId", action);
        Assert.StartsWith("null または空の文字列を設定できません。", ex.Message);
    }

    [Fact]
    public async Task GetBasketItemsAsync_購入者Idがnullまたは空白ではない_陳列品リポジトリのFindAsyncを1度だけ呼び出す()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var displayItems = new List<DisplayItem>
         {
             CreateDisplayItem(new Guid("019b76da-a800-7004-8001-00000000000a"), "説明1", "ダミー商品1", "C000000001"),
         };
        var basket = new Basket { BuyerId = dummyBuyerId };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.AddAsync(It.IsAny<Basket>(), AnyToken))
            .ReturnsAsync(basket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var displayItemRepo = new Mock<IDisplayItemRepository>();
        displayItemRepo
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<DisplayItem, bool>>>(), AnyToken))
            .ReturnsAsync(displayItems.AsReadOnly());
        var displayItemDomainService = Mock.Of<IDisplayItemDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, displayItemRepo.Object, displayItemDomainService, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        await service.GetBasketItemsAsync(dummyBuyerId, cancellationToken);

        // Assert
        displayItemRepo.Verify(
            r => r.FindAsync(It.IsAny<Expression<Func<DisplayItem, bool>>>(), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task GetBasketItemsAsync_購入者Idがnullまたは空白ではない_買い物かごリポジトリのGetWithBasketItemsAsyncを1度だけ呼び出す()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var displayItems = new List<DisplayItem>
         {
             CreateDisplayItem(new Guid("019b76da-a800-7004-8001-00000000000a"), "説明1", "ダミー商品1", "C000000001"),
         };
        var basket = new Basket { BuyerId = dummyBuyerId };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.AddAsync(It.IsAny<Basket>(), AnyToken))
            .ReturnsAsync(basket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var displayItemRepo = new Mock<IDisplayItemRepository>();
        displayItemRepo
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<DisplayItem, bool>>>(), AnyToken))
            .ReturnsAsync(displayItems.AsReadOnly());
        var displayItemDomainService = Mock.Of<IDisplayItemDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, displayItemRepo.Object, displayItemDomainService, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        await service.GetBasketItemsAsync(dummyBuyerId, cancellationToken);

        // Assert
        basketRepo.Verify(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken), Times.Once);
    }

    [Fact]
    public async Task GetBasketItemsAsync_購入者Idに対応する買い物かごが存在しない_買い物かごリポジトリのAddAsyncを1度だけ呼び出す()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var displayItems = new List<DisplayItem>
         {
             CreateDisplayItem(new Guid("019b76da-a800-7004-8001-00000000000a"), "説明1", "ダミー商品1", "C000000001"),
         };
        var basket = new Basket { BuyerId = dummyBuyerId };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.AddAsync(It.IsAny<Basket>(), AnyToken))
            .ReturnsAsync(basket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var displayItemRepo = new Mock<IDisplayItemRepository>();
        displayItemRepo
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<DisplayItem, bool>>>(), AnyToken))
            .ReturnsAsync(displayItems.AsReadOnly());
        var displayItemDomainService = Mock.Of<IDisplayItemDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, displayItemRepo.Object, displayItemDomainService, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        await service.GetBasketItemsAsync(dummyBuyerId, cancellationToken);

        // Assert
        basketRepo.Verify(r => r.AddAsync(It.Is<Basket>(b => b.BuyerId == dummyBuyerId), AnyToken), Times.Once);
    }

    [Fact]
    public async Task GetBasketItemsAsync_買い物かごアイテムに対応する陳列品が削除されている_削除済みアイテムのIDが返却される()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var deletedDisplayItem1 = CreateDisplayItem(new Guid("019b76da-a800-7004-8001-00000000000a"), "説明1", "ダミー商品1", "C000000001", true);
        var deletedDisplayItem2 = CreateDisplayItem(new Guid("019b76da-a800-7004-8001-00000000000b"), "説明2", "ダミー商品2", "C000000002", true);
        var existingDisplayItem = CreateDisplayItem(new Guid("019b76da-a800-7004-8001-00000000000c"), "説明3", "ダミー商品3", "C000000003");
        var displayItems = new List<DisplayItem>
         {
             deletedDisplayItem1, deletedDisplayItem2, existingDisplayItem,
         };
        var basket = new Basket { BuyerId = dummyBuyerId };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.AddAsync(It.IsAny<Basket>(), AnyToken))
            .ReturnsAsync(basket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var displayItemRepo = new Mock<IDisplayItemRepository>();
        displayItemRepo
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<DisplayItem, bool>>>(), AnyToken))
            .ReturnsAsync(displayItems.AsReadOnly());
        var displayItemDomainService = Mock.Of<IDisplayItemDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, displayItemRepo.Object, displayItemDomainService, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var (basketResult, displayItemList, deletedItemIds) = await service.GetBasketItemsAsync(dummyBuyerId, cancellationToken);

        // Assert
        Assert.Collection(
            deletedItemIds,
            id => Assert.Equal(deletedDisplayItem1.Id, id),
            id => Assert.Equal(deletedDisplayItem2.Id, id));
    }

    [Fact]
    public async Task GetBasketItemsAsync_既存の買い物かごの一覧を取得_正しく買い物かごの情報が取得できる()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var deletedDisplayItem = CreateDisplayItem(new Guid("019b76da-a800-7004-8001-00000000000a"), "説明1", "ダミー商品1", "C000000001", true);
        var existingDisplayItem = CreateDisplayItem(new Guid("019b76da-a800-7004-8001-00000000000b"), "説明2", "ダミー商品2", "C000000002");
        var displayItems = new List<DisplayItem>
         {
             deletedDisplayItem, existingDisplayItem,
         };
        var basket = new Basket { BuyerId = dummyBuyerId };
        basket.AddItem(deletedDisplayItem.Id, deletedDisplayItem.Price, 1);
        basket.AddItem(existingDisplayItem.Id, existingDisplayItem.Price, 1);
        var basketItemList = basket.Items.ToList();

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.AddAsync(It.IsAny<Basket>(), AnyToken))
            .ReturnsAsync(basket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var displayItemRepo = new Mock<IDisplayItemRepository>();
        displayItemRepo
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<DisplayItem, bool>>>(), AnyToken))
            .ReturnsAsync(displayItems.AsReadOnly());
        var displayItemDomainService = Mock.Of<IDisplayItemDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, displayItemRepo.Object, displayItemDomainService, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var (basketResult, displayItemList, deletedItemIds) = await service.GetBasketItemsAsync(dummyBuyerId, cancellationToken);

        // Assert
        Assert.Equal(basket.BuyerId, basketResult.BuyerId);
        Assert.Collection(
            basketResult.Items,
            basketItem =>
            {
                Assert.Equal(basketItemList[0].DisplayItemId, basketItem.DisplayItemId);
                Assert.Equal(basketItemList[0].Quantity, basketItem.Quantity);
            },
            basketItem =>
            {
                Assert.Equal(basketItemList[1].DisplayItemId, basketItem.DisplayItemId);
                Assert.Equal(basketItemList[1].Quantity, basketItem.Quantity);
            });
    }

    [Fact]
    public async Task GetBasketItemsAsync_既存の買い物かごの一覧を取得_正しく陳列品の一覧が取得できる()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var deletedDisplayItem = CreateDisplayItem(new Guid("019b76da-a800-7004-8001-00000000000a"), "説明1", "ダミー商品1", "C000000001", true);
        var existingDisplayItem = CreateDisplayItem(new Guid("019b76da-a800-7004-8001-00000000000b"), "説明2", "ダミー商品2", "C000000002");
        var displayItems = new List<DisplayItem>
         {
             deletedDisplayItem, existingDisplayItem,
         };
        var basket = new Basket { BuyerId = dummyBuyerId };
        basket.AddItem(deletedDisplayItem.Id, deletedDisplayItem.Price, 1);
        basket.AddItem(existingDisplayItem.Id, existingDisplayItem.Price, 1);

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.AddAsync(It.IsAny<Basket>(), AnyToken))
            .ReturnsAsync(basket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var displayItemRepo = new Mock<IDisplayItemRepository>();
        displayItemRepo
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<DisplayItem, bool>>>(), AnyToken))
            .ReturnsAsync(displayItems.AsReadOnly());
        var displayItemDomainService = Mock.Of<IDisplayItemDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, displayItemRepo.Object, displayItemDomainService, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var (basketResult, displayItemList, deletedItemIds) = await service.GetBasketItemsAsync(dummyBuyerId, cancellationToken);

        // Assert
        Assert.Collection(
            displayItemList,
            displayItem =>
            {
                Assert.Equal(deletedDisplayItem.Id, displayItem.Id);
                Assert.Equal(deletedDisplayItem.IsDeleted, displayItem.IsDeleted);
            },
            displayItem =>
            {
                Assert.Equal(existingDisplayItem.Id, displayItem.Id);
                Assert.Equal(existingDisplayItem.IsDeleted, displayItem.IsDeleted);
            });
    }

    [Fact]
    public async Task GetBasketItemsAsync_買い物かごの一覧取得時に買い物かごを新規作成_正しく買い物かごの情報が取得できる()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var deletedDisplayItem = CreateDisplayItem(new Guid("019b76da-a800-7004-8001-00000000000a"), "説明1", "ダミー商品1", "C000000001", true);
        var existingDisplayItem = CreateDisplayItem(new Guid("019b76da-a800-7004-8001-00000000000b"), "説明2", "ダミー商品2", "C000000002");
        var displayItems = new List<DisplayItem>
         {
             deletedDisplayItem, existingDisplayItem,
         };
        var basket = new Basket { BuyerId = dummyBuyerId };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.AddAsync(It.IsAny<Basket>(), AnyToken))
            .ReturnsAsync(basket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var displayItemRepo = new Mock<IDisplayItemRepository>();
        displayItemRepo
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<DisplayItem, bool>>>(), AnyToken))
            .ReturnsAsync(displayItems.AsReadOnly());
        var displayItemDomainService = Mock.Of<IDisplayItemDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, displayItemRepo.Object, displayItemDomainService, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var (basketResult, displayItemList, deletedItemIds) = await service.GetBasketItemsAsync(dummyBuyerId, cancellationToken);

        // Assert
        Assert.Equal(basket.BuyerId, basketResult.BuyerId);
        Assert.Empty(basketResult.Items);
    }

    [Fact]
    public async Task GetBasketItemsAsync_既存の買い物かごの一覧を取得_陳列品の一覧が空のリストになる()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var basket = new Basket { BuyerId = dummyBuyerId };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.AddAsync(It.IsAny<Basket>(), AnyToken))
            .ReturnsAsync(basket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var displayItemRepo = new Mock<IDisplayItemRepository>();
        displayItemRepo
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<DisplayItem, bool>>>(), AnyToken))
            .ReturnsAsync(new List<DisplayItem>());
        var displayItemDomainService = Mock.Of<IDisplayItemDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, displayItemRepo.Object, displayItemDomainService, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var (basketResult, displayItemList, deletedItemIds) = await service.GetBasketItemsAsync(dummyBuyerId, cancellationToken);

        // Assert
        Assert.Empty(displayItemList);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SetBasketItemsQuantitiesAsync_購入者Idがnullまたは空白_ArgumentExceptionが発生する(string? nullOrEmptyBuyerId)
    {
        // Arrange
        var basketRepo = Mock.Of<IBasketRepository>();
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var displayItemRepo = Mock.Of<IDisplayItemRepository>();
        var displayItemDomainService = Mock.Of<IDisplayItemDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo, orderRepo, orderFactory, displayItemRepo, displayItemDomainService, logger);

        // Act
        var action = () => service.SetBasketItemsQuantitiesAsync(nullOrEmptyBuyerId!, new() { { new Guid("019b76da-a800-7004-8001-000000000001"), 1 } });

        // Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>("buyerId", action);
        Assert.StartsWith("null または空の文字列を設定できません。", ex.Message);
    }

    [Fact]
    public async Task SetBasketItemsQuantitiesAsync_買い物かご内に存在しない陳列品が数量設定対象_DisplayItemNotExistingInBasketExceptionを返す()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        dummyBasket.AddItem(new Guid("019b76da-a800-7004-8001-00000000000a"), 100);
        var quantities = new Dictionary<Guid, int>() { { Guid.Empty, 1 } };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync(dummyBasket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var displayItemRepo = Mock.Of<IDisplayItemRepository>();
        var displayItemDomainService = Mock.Of<IDisplayItemDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, displayItemRepo, displayItemDomainService, logger);

        // Act
        var action = () => service.SetBasketItemsQuantitiesAsync(dummyBuyerId, quantities);

        // Assert
        await Assert.ThrowsAsync<DisplayItemNotExistingInBasketException>(action);
    }

    [Fact]
    public async Task SetBasketItemsQuantitiesAsync_陳列品リポジトリに存在しない陳列品が数量設定対象_DisplayItemNotExistingInRepositoryExceptionを返す()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        dummyBasket.AddItem(new Guid("019b76da-a800-7004-8001-00000000000a"), 1000m);
        var quantities = new Dictionary<Guid, int>() { { new Guid("019b76da-a800-7004-8001-00000000000a"), 5 } };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync(dummyBasket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var displayItemRepo = Mock.Of<IDisplayItemRepository>();
        var displayItemDomainService = new Mock<IDisplayItemDomainService>();
        displayItemDomainService
            .Setup(d => d.ExistsAllAsync(quantities.Keys, AnyToken))
            .ReturnsAsync((false, new List<DisplayItem>().AsReadOnly()));
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, displayItemRepo, displayItemDomainService.Object, logger);

        // Act
        var action = () => service.SetBasketItemsQuantitiesAsync(dummyBuyerId, quantities);

        // Assert
        await Assert.ThrowsAsync<DisplayItemNotExistingInRepositoryException>(action);
    }

    [Fact]
    public async Task SetBasketItemsQuantitiesAsync_買い物かご内と陳列品リポジトリに存在する陳列品が数量設定対象_買い物かごリポジトリのUpdateAsyncを1度だけ呼び出す()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        dummyBasket.AddItem(new Guid("019b76da-a800-7004-8001-00000000000a"), 1000m);
        var quantities = new Dictionary<Guid, int>() { { new Guid("019b76da-a800-7004-8001-00000000000a"), 5 } };
        var displayItems = new List<DisplayItem>
         {
             CreateDisplayItem(new Guid("019b76da-a800-7004-8001-00000000000a"), "説明1", "ダミー商品1", "C000000001"),
         };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync(dummyBasket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var displayItemRepo = Mock.Of<IDisplayItemRepository>();
        var displayItemDomainService = new Mock<IDisplayItemDomainService>();
        displayItemDomainService
            .Setup(d => d.ExistsAllAsync(quantities.Keys, AnyToken))
            .ReturnsAsync((true, displayItems.AsReadOnly()));
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, displayItemRepo, displayItemDomainService.Object, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        await service.SetBasketItemsQuantitiesAsync(dummyBuyerId, quantities, cancellationToken);

        // Assert
        basketRepo.Verify(
            r => r.UpdateAsync(It.Is<Basket>(b => b.BuyerId == dummyBuyerId), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task SetBasketItemsQuantitiesAsync_買い物かご内と陳列品リポジトリに存在する陳列品が数量設定対象_買い物かごの商品数が更新される()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        dummyBasket.AddItem(new Guid("019b76da-a800-7004-8001-00000000000a"), 1000m);
        var newQuantity = 5;
        var quantities = new Dictionary<Guid, int>() { { new Guid("019b76da-a800-7004-8001-00000000000a"), newQuantity } };
        var displayItems = new List<DisplayItem>
         {
             CreateDisplayItem(new Guid("019b76da-a800-7004-8001-00000000000a"), "説明1", "ダミー商品1", "C000000001"),
         };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync(dummyBasket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var displayItemRepo = Mock.Of<IDisplayItemRepository>();
        var displayItemDomainService = new Mock<IDisplayItemDomainService>();
        displayItemDomainService
            .Setup(d => d.ExistsAllAsync(quantities.Keys, AnyToken))
            .ReturnsAsync((true, displayItems.AsReadOnly()));
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, displayItemRepo, displayItemDomainService.Object, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        await service.SetBasketItemsQuantitiesAsync(dummyBuyerId, quantities, cancellationToken);

        // Assert
        basketRepo.Verify(
            r => r.UpdateAsync(It.Is<Basket>(b => b.Items.First().Quantity == newQuantity), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task SetBasketItemsQuantitiesAsync_数量設定後に数量が0となる_買い物かごアイテムが削除される()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        dummyBasket.AddItem(new Guid("019b76da-a800-7004-8001-00000000000a"), 1000m);
        var quantities = new Dictionary<Guid, int>() { { new Guid("019b76da-a800-7004-8001-00000000000a"), 0 } };
        var displayItems = new List<DisplayItem>
         {
             CreateDisplayItem(new Guid("019b76da-a800-7004-8001-00000000000a"), "説明1", "ダミー商品1", "C000000001"),
         };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync(dummyBasket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var displayItemRepo = Mock.Of<IDisplayItemRepository>();
        var displayItemDomainService = new Mock<IDisplayItemDomainService>();
        displayItemDomainService
            .Setup(d => d.ExistsAllAsync(quantities.Keys, AnyToken))
            .ReturnsAsync((true, displayItems.AsReadOnly()));
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, displayItemRepo, displayItemDomainService.Object, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        await service.SetBasketItemsQuantitiesAsync(dummyBuyerId, quantities, cancellationToken);

        // Assert
        basketRepo.Verify(
            r => r.UpdateAsync(It.Is<Basket>(b => b.Items.Count == 0), AnyToken),
            Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task AddItemToBasketAsync_購入者Idがnullまたは空白_ArgumentExceptionが発生する(string? nullOrEmptyBuyerId)
    {
        // Arrange
        var basketRepo = Mock.Of<IBasketRepository>();
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var displayItemRepo = Mock.Of<IDisplayItemRepository>();
        var displayItemDomainService = Mock.Of<IDisplayItemDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo, orderRepo, orderFactory, displayItemRepo, displayItemDomainService, logger);

        // Act
        var action = () => service.AddItemToBasketAsync(nullOrEmptyBuyerId!, new Guid("019b76da-a800-7004-8001-00000000000a"), 5);

        // Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>("buyerId", action);
        Assert.StartsWith("null または空の文字列を設定できません。", ex.Message);
    }

    [Fact]
    public async Task AddItemToBasketAsync_陳列品リポジトリに存在しない陳列品が追加対象_DisplayItemNotExistingInRepositoryExceptionを返す()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        var quantities = new Dictionary<Guid, int>() { { new Guid("019b76da-a800-7004-8001-00000000000a"), 5 } };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync(dummyBasket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var displayItemRepo = Mock.Of<IDisplayItemRepository>();
        var displayItemDomainService = new Mock<IDisplayItemDomainService>();
        displayItemDomainService
            .Setup(d => d.ExistsAllAsync(quantities.Keys, AnyToken))
            .ReturnsAsync((false, new List<DisplayItem>().AsReadOnly()));
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, displayItemRepo, displayItemDomainService.Object, logger);

        // Act
        var action = () => service.AddItemToBasketAsync(dummyBuyerId, new Guid("019b76da-a800-7004-8001-00000000000a"), 5);

        // Assert
        await Assert.ThrowsAsync<DisplayItemNotExistingInRepositoryException>(action);
    }

    [Fact]
    public async Task AddItemToBasketAsync_陳列品リポジトリに存在する陳列品が追加対象_買い物かごリポジトリのUpdateAsyncを1度だけ呼び出す()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        var quantities = new Dictionary<Guid, int>() { { new Guid("019b76da-a800-7004-8001-00000000000a"), 5 } };
        var displayItems = new List<DisplayItem>
         {
             CreateDisplayItem(new Guid("019b76da-a800-7004-8001-00000000000a"), "説明1", "ダミー商品1", "C000000001"),
         };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync(dummyBasket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var displayItemRepo = Mock.Of<IDisplayItemRepository>();
        var displayItemDomainService = new Mock<IDisplayItemDomainService>();
        displayItemDomainService
            .Setup(d => d.ExistsAllAsync(quantities.Keys, AnyToken))
            .ReturnsAsync((true, displayItems.AsReadOnly()));
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, displayItemRepo, displayItemDomainService.Object, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        await service.AddItemToBasketAsync(dummyBuyerId, new Guid("019b76da-a800-7004-8001-00000000000a"), 5, cancellationToken);

        // Assert
        basketRepo.Verify(
            r => r.UpdateAsync(It.Is<Basket>(b => b.BuyerId == dummyBuyerId), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task AddItemToBasketAsync_陳列品リポジトリに存在する陳列品が追加対象_買い物かごに追加対象の商品が追加される()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        var quantities = new Dictionary<Guid, int>() { { new Guid("019b76da-a800-7004-8001-00000000000a"), 5 } };
        var displayItem = CreateDisplayItem(new Guid("019b76da-a800-7004-8001-00000000000a"), "説明1", "ダミー商品1", "C000000001");
        var displayItems = new List<DisplayItem> { displayItem };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync(dummyBasket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var displayItemRepo = Mock.Of<IDisplayItemRepository>();
        var displayItemDomainService = new Mock<IDisplayItemDomainService>();
        displayItemDomainService
            .Setup(d => d.ExistsAllAsync(quantities.Keys, AnyToken))
            .ReturnsAsync((true, displayItems.AsReadOnly()));
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, displayItemRepo, displayItemDomainService.Object, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        await service.AddItemToBasketAsync(dummyBuyerId, new Guid("019b76da-a800-7004-8001-00000000000a"), 5, cancellationToken);

        // Assert
        basketRepo.Verify(
            r => r.UpdateAsync(
            It.Is<Basket>(b => b.Items.Count == 1), AnyToken),
            Times.Once);
        basketRepo.Verify(
           r => r.UpdateAsync(
           It.Is<Basket>(b => b.Items.First().DisplayItemId == displayItem.Id), AnyToken),
           Times.Once);
        basketRepo.Verify(
           r => r.UpdateAsync(
           It.Is<Basket>(b => b.Items.First().Quantity == 5), AnyToken),
           Times.Once);
        basketRepo.Verify(
           r => r.UpdateAsync(
           It.Is<Basket>(b => b.Items.First().UnitPrice == displayItem.Price), AnyToken),
           Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CheckoutAsync_購入者Idがnullまたは空白_ArgumentExceptionが発生する(string? nullOrEmptyBuyerId)
    {
        // Arrange
        var shipTo = CreateDefaultShipTo();
        var basketRepo = Mock.Of<IBasketRepository>();
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var displayItemRepo = Mock.Of<IDisplayItemRepository>();
        var displayItemDomainService = Mock.Of<IDisplayItemDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo, orderRepo, orderFactory, displayItemRepo, displayItemDomainService, logger);

        // Act
        var action = () => service.CheckoutAsync(nullOrEmptyBuyerId!, shipTo);

        // Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>("buyerId", action);
        Assert.StartsWith("null または空の文字列を設定できません。", ex.Message);
    }

    [Fact]
    public async Task CheckoutAsync_買い物かごがnull_NullBasketOnCheckoutExceptionが発生する()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        var shipTo = CreateDefaultShipTo();

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync((Basket?)null);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var displayItemRepo = Mock.Of<IDisplayItemRepository>();
        var displayItemDomainService = Mock.Of<IDisplayItemDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, displayItemRepo, displayItemDomainService, logger);

        // Act
        var action = () => service.CheckoutAsync(dummyBuyerId, shipTo);

        // Assert
        var ex = await Assert.ThrowsAsync<NullBasketOnCheckoutException>(action);
    }

    [Fact]
    public async Task CheckoutAsync_買い物かごが空_EmptyBasketOnCheckoutExceptionが発生する()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        var shipTo = CreateDefaultShipTo();

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync(dummyBasket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var displayItemRepo = Mock.Of<IDisplayItemRepository>();
        var displayItemDomainService = Mock.Of<IDisplayItemDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, displayItemRepo, displayItemDomainService, logger);

        // Act
        var action = () => service.CheckoutAsync(dummyBuyerId, shipTo);

        // Assert
        await Assert.ThrowsAsync<EmptyBasketOnCheckoutException>(action);
    }

    [Fact]
    public async Task CheckoutAsync_買い物かごにアイテムが存在する_陳列品リポジトリのFindAsyncを1度だけ呼び出す()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        dummyBasket.AddItem(new Guid("019b76da-a800-7004-8001-00000000000a"), 1000);
        var shipTo = CreateDefaultShipTo();
        var displayItems = new List<DisplayItem>
         {
             CreateDisplayItem(new Guid("019b76da-a800-7004-8001-00000000000a"), "説明1", "ダミー商品1", "C000000001"),
         };
        var order = new Order(CreateDefaultOrderItems()) { BuyerId = dummyBuyerId, ShipToAddress = shipTo };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync(dummyBasket);
        basketRepo
            .Setup(r => r.RemoveAsync(dummyBasket, AnyToken))
            .Returns(Task.CompletedTask);
        var orderRepo = new Mock<IOrderRepository>();
        orderRepo
            .Setup(r => r.AddAsync(order, AnyToken))
            .ReturnsAsync(order);
        var orderFactory = new Mock<IOrderFactory>();
        orderFactory
            .Setup(f => f.CreateOrder(dummyBasket, displayItems, shipTo))
            .Returns(order);
        var displayItemRepo = new Mock<IDisplayItemRepository>();
        displayItemRepo
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<DisplayItem, bool>>>(), AnyToken))
            .ReturnsAsync(displayItems.AsReadOnly());
        var displayItemDomainService = Mock.Of<IDisplayItemDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo.Object, orderFactory.Object, displayItemRepo.Object, displayItemDomainService, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        await service.CheckoutAsync(dummyBuyerId, shipTo, cancellationToken);

        // Assert
        displayItemRepo.Verify(
            r => r.FindAsync(It.IsAny<Expression<Func<DisplayItem, bool>>>(), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task CheckoutAsync_買い物かごにアイテムが存在する_注文リポジトリのAddAsyncを1度だけ呼び出す()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        dummyBasket.AddItem(new Guid("019b76da-a800-7004-8001-00000000000a"), 1000);
        var shipTo = CreateDefaultShipTo();
        var displayItems = new List<DisplayItem>
         {
             CreateDisplayItem(new Guid("019b76da-a800-7004-8001-00000000000a"), "説明1", "ダミー商品1", "C000000001"),
         };
        var order = new Order(CreateDefaultOrderItems()) { BuyerId = dummyBuyerId, ShipToAddress = shipTo };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync(dummyBasket);
        basketRepo
            .Setup(r => r.RemoveAsync(dummyBasket, AnyToken))
            .Returns(Task.CompletedTask);
        var orderRepo = new Mock<IOrderRepository>();
        orderRepo
            .Setup(r => r.AddAsync(order, AnyToken))
            .ReturnsAsync(order);
        var orderFactory = new Mock<IOrderFactory>();
        orderFactory
            .Setup(f => f.CreateOrder(dummyBasket, displayItems, shipTo))
            .Returns(order);
        var displayItemRepo = new Mock<IDisplayItemRepository>();
        displayItemRepo
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<DisplayItem, bool>>>(), AnyToken))
            .ReturnsAsync(displayItems.AsReadOnly());
        var displayItemDomainService = Mock.Of<IDisplayItemDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo.Object, orderFactory.Object, displayItemRepo.Object, displayItemDomainService, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        await service.CheckoutAsync(dummyBuyerId, shipTo, cancellationToken);

        // Assert
        orderRepo.Verify(
            r => r.AddAsync(It.Is<Order>(o => o.BuyerId == dummyBuyerId), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task CheckoutAsync_買い物かごにアイテムが存在する_注文リポジトリのAddAsyncの引数にお届け先が正しく設定されている()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        dummyBasket.AddItem(new Guid("019b76da-a800-7004-8001-00000000000a"), 1000);
        var shipTo = CreateDefaultShipTo();
        var displayItems = new List<DisplayItem>
         {
             CreateDisplayItem(new Guid("019b76da-a800-7004-8001-00000000000a"), "説明1", "ダミー商品1", "C000000001"),
         };
        var order = new Order(CreateDefaultOrderItems()) { BuyerId = dummyBuyerId, ShipToAddress = shipTo };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync(dummyBasket);
        basketRepo
            .Setup(r => r.RemoveAsync(dummyBasket, AnyToken))
            .Returns(Task.CompletedTask);
        var orderRepo = new Mock<IOrderRepository>();
        orderRepo
            .Setup(r => r.AddAsync(order, AnyToken))
            .ReturnsAsync(order);
        var orderFactory = new Mock<IOrderFactory>();
        orderFactory
            .Setup(f => f.CreateOrder(dummyBasket, displayItems, shipTo))
            .Returns(order);
        var displayItemRepo = new Mock<IDisplayItemRepository>();
        displayItemRepo
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<DisplayItem, bool>>>(), AnyToken))
            .ReturnsAsync(displayItems.AsReadOnly());
        var displayItemDomainService = Mock.Of<IDisplayItemDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo.Object, orderFactory.Object, displayItemRepo.Object, displayItemDomainService, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        await service.CheckoutAsync(dummyBuyerId, shipTo, cancellationToken);

        // Assert
        orderRepo.Verify(
            r => r.AddAsync(It.Is<Order>(o => o.ShipToAddress == shipTo), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task CheckoutAsync_買い物かごにアイテムが存在する_注文リポジトリのAddAsyncの引数に注文アイテムが正しく設定されている()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        dummyBasket.AddItem(new Guid("019b76da-a800-7004-8001-00000000000a"), 1000);
        var shipTo = CreateDefaultShipTo();
        var displayItems = new List<DisplayItem>
         {
             CreateDisplayItem(new Guid("019b76da-a800-7004-8001-00000000000a"), "説明1", "ダミー商品1", "C000000001"),
         };
        var order = new Order(CreateDefaultOrderItems()) { BuyerId = dummyBuyerId, ShipToAddress = shipTo };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync(dummyBasket);
        basketRepo
            .Setup(r => r.RemoveAsync(dummyBasket, AnyToken))
            .Returns(Task.CompletedTask);
        var orderRepo = new Mock<IOrderRepository>();
        orderRepo
            .Setup(r => r.AddAsync(order, AnyToken))
            .ReturnsAsync(order);
        var orderFactory = new Mock<IOrderFactory>();
        orderFactory
            .Setup(f => f.CreateOrder(dummyBasket, displayItems, shipTo))
            .Returns(order);
        var displayItemRepo = new Mock<IDisplayItemRepository>();
        displayItemRepo
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<DisplayItem, bool>>>(), AnyToken))
            .ReturnsAsync(displayItems.AsReadOnly());
        var displayItemDomainService = Mock.Of<IDisplayItemDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo.Object, orderFactory.Object, displayItemRepo.Object, displayItemDomainService, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        await service.CheckoutAsync(dummyBuyerId, shipTo, cancellationToken);

        // Assert
        orderRepo.Verify(
            r => r.AddAsync(It.Is<Order>(o => o.OrderItems.Count == 1), AnyToken),
            Times.Once);
        orderRepo.Verify(
            r => r.AddAsync(It.Is<Order>(o => o.OrderItems.First().Quantity == 1), AnyToken),
            Times.Once);
        orderRepo.Verify(
            r => r.AddAsync(It.Is<Order>(o => o.OrderItems.First().UnitPrice == 1000m), AnyToken),
            Times.Once);
        orderRepo.Verify(
            r => r.AddAsync(It.Is<Order>(o => o.OrderItems.First().ItemOrdered.ProductName == "ダミー商品1"), AnyToken),
            Times.Once);
        orderRepo.Verify(
            r => r.AddAsync(It.Is<Order>(o => o.OrderItems.First().ItemOrdered.ProductCode == "C000000001"), AnyToken),
            Times.Once);
        orderRepo.Verify(
            r => r.AddAsync(It.Is<Order>(o => o.OrderItems.First().ItemOrdered.DisplayItemId == new Guid("019b76da-a800-7004-8001-000000000001")), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task CheckoutAsync_買い物かごにアイテムが存在する_買い物かごリポジトリのRemoveAsyncを1度だけ呼び出す()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        dummyBasket.AddItem(new Guid("019b76da-a800-7004-8001-00000000000a"), 1000);
        var shipTo = CreateDefaultShipTo();
        var displayItems = new List<DisplayItem>
         {
             CreateDisplayItem(new Guid("019b76da-a800-7004-8001-00000000000a"), "説明1", "ダミー商品1", "C000000001"),
         };
        var order = new Order(CreateDefaultOrderItems()) { BuyerId = dummyBuyerId, ShipToAddress = shipTo };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync(dummyBasket);
        basketRepo
            .Setup(r => r.RemoveAsync(dummyBasket, AnyToken))
            .Returns(Task.CompletedTask);
        var orderRepo = new Mock<IOrderRepository>();
        orderRepo
            .Setup(r => r.AddAsync(order, AnyToken))
            .ReturnsAsync(order);
        var orderFactory = new Mock<IOrderFactory>();
        orderFactory
            .Setup(f => f.CreateOrder(dummyBasket, displayItems, shipTo))
            .Returns(order);
        var displayItemRepo = new Mock<IDisplayItemRepository>();
        displayItemRepo
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<DisplayItem, bool>>>(), AnyToken))
            .ReturnsAsync(displayItems.AsReadOnly());
        var displayItemDomainService = Mock.Of<IDisplayItemDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo.Object, orderFactory.Object, displayItemRepo.Object, displayItemDomainService, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        await service.CheckoutAsync(dummyBuyerId, shipTo, cancellationToken);

        // Assert
        basketRepo.Verify(
            r => r.RemoveAsync(It.Is<Basket>(b => b.BuyerId == dummyBuyerId), AnyToken),
            Times.Once);
    }

    private static Address CreateDefaultAddress()
    {
        const string defaultPostalCode = "100-8924";
        const string defaultTodofuken = "東京都";
        const string defaultShikuchoson = "千代田区";
        const string defaultAzanaAndOthers = "永田町1-10-1";
        return new Address(defaultPostalCode, defaultTodofuken, defaultShikuchoson, defaultAzanaAndOthers);
    }

    private static ShipTo CreateDefaultShipTo()
    {
        const string defaultFullName = "国会　太郎";
        var address = CreateDefaultAddress();
        return new ShipTo(defaultFullName, address);
    }

    private static List<OrderItem> CreateDefaultOrderItems()
    {
        // Arrange
        const string productName = "ダミー商品1";
        const string productCode = "C000000001";

        var items = new List<OrderItem>()
        {
            new() { ItemOrdered = new DisplayItemOrdered(new Guid("019b76da-a800-7004-8001-000000000001"), productName, productCode), UnitPrice = 1000m, Quantity = 1 },
        };

        return items;
    }

    private static DisplayItem CreateDisplayItem(Guid id, string description, string name, string productCode, bool isDeleted = false)
    {
        return new DisplayItem
        {
            DisplayItemCategoryId = new Guid("019b76da-a800-7003-8001-000000000001"),
            DisplayItemBrandId = new Guid("019b76da-a800-7002-8001-000000000001"),
            Description = description,
            Name = name,
            Price = 1000m,
            ProductCode = productCode,
            Id = id,
            IsDeleted = isDeleted,
        };
    }
}
