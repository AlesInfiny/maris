using System.Linq.Expressions;
using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.Catalog;
using Dressca.ApplicationCore.Ordering;
using Xunit.Abstractions;

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
        var catalogRepo = Mock.Of<ICatalogRepository>();
        var catalogDomainService = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo, orderRepo, orderFactory, catalogRepo, catalogDomainService, logger);

        // Act
        var action = () => service.GetBasketItemsAsync(nullOrEmptyBuyerId!);

        // Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>("buyerId", action);
        Assert.StartsWith("null または空の文字列を設定できません。", ex.Message);
    }

    [Fact]
    public async Task GetBasketItemsAsync_購入者Idがnullまたは空白ではない_カタログリポジトリのFindAsyncを1度だけ呼び出す()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var catalogItems = new List<CatalogItem>
         {
             new() { CatalogCategoryId = 100L, CatalogBrandId = 110L, Description = "説明1", Name = "ダミー商品1", Price = 1000m, ProductCode = "C000000001", Id = 10L },
         };
        var basket = new Basket { BuyerId = dummyBuyerId };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.AddAsync(It.IsAny<Basket>(), AnyToken))
            .ReturnsAsync(basket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var catalogRepo = new Mock<ICatalogRepository>();
        catalogRepo
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken))
            .ReturnsAsync(catalogItems.AsReadOnly());
        var catalogDomainService = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, catalogRepo.Object, catalogDomainService, logger);

        // Act
        await service.GetBasketItemsAsync(dummyBuyerId);

        // Assert
        catalogRepo.Verify(
            r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task GetBasketItemsAsync_購入者Idがnullまたは空白ではない_買い物かごリポジトリのGetWithBasketItemsAsyncを1度だけ呼び出す()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var catalogItems = new List<CatalogItem>
         {
             new() { CatalogCategoryId = 100L, CatalogBrandId = 110L, Description = "説明1", Name = "ダミー商品1", Price = 1000m, ProductCode = "C000000001", Id = 10L },
         };
        var basket = new Basket { BuyerId = dummyBuyerId };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.AddAsync(It.IsAny<Basket>(), AnyToken))
            .ReturnsAsync(basket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var catalogRepo = new Mock<ICatalogRepository>();
        catalogRepo
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken))
            .ReturnsAsync(catalogItems.AsReadOnly());
        var catalogDomainService = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, catalogRepo.Object, catalogDomainService, logger);

        // Act
        await service.GetBasketItemsAsync(dummyBuyerId);

        // Assert
        basketRepo.Verify(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken), Times.Once);
    }

    [Fact]
    public async Task GetBasketItemsAsync_購入者Idに対応する買い物かごが存在しない_買い物かごリポジトリのAddAsyncを1度だけ呼び出す()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var catalogItems = new List<CatalogItem>
         {
             new() { CatalogCategoryId = 100L, CatalogBrandId = 110L, Description = "説明1", Name = "ダミー商品1", Price = 1000m, ProductCode = "C000000001", Id = 10L },
         };
        var basket = new Basket { BuyerId = dummyBuyerId };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.AddAsync(It.IsAny<Basket>(), AnyToken))
            .ReturnsAsync(basket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var catalogRepo = new Mock<ICatalogRepository>();
        catalogRepo
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken))
            .ReturnsAsync(catalogItems.AsReadOnly());
        var catalogDomainService = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, catalogRepo.Object, catalogDomainService, logger);

        // Act
        await service.GetBasketItemsAsync(dummyBuyerId);

        // Assert
        basketRepo.Verify(r => r.AddAsync(It.Is<Basket>(b => b.BuyerId == dummyBuyerId), AnyToken), Times.Once);
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
        var catalogRepo = Mock.Of<ICatalogRepository>();
        var catalogDomainService = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo, orderRepo, orderFactory, catalogRepo, catalogDomainService, logger);

        // Act
        var action = () => service.SetBasketItemsQuantitiesAsync(nullOrEmptyBuyerId!, new() { { 1, 1 } });

        // Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>("buyerId", action);
        Assert.StartsWith("null または空の文字列を設定できません。", ex.Message);
    }

    [Fact]
    public async Task SetBasketItemsQuantitiesAsync_買い物かご内に存在しないカタログアイテムが数量設定対象_CatalogItemNotExistingInBasketExceptionを返す()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        dummyBasket.AddItem(10, 100);
        var quantities = new Dictionary<long, int>() { { 0, 1 } };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync(dummyBasket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var catalogRepo = Mock.Of<ICatalogRepository>();
        var catalogDomainService = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, catalogRepo, catalogDomainService, logger);

        // Act
        var action = () => service.SetBasketItemsQuantitiesAsync(dummyBuyerId, quantities);

        // Assert
        await Assert.ThrowsAsync<CatalogItemNotExistingInBasketException>(action);
    }

    [Fact]
    public async Task SetBasketItemsQuantitiesAsync_カタログリポジトリに存在しないカタログアイテムが数量設定対象_CatalogItemNotExistingInRepositoryExceptionを返す()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        dummyBasket.AddItem(10L, 1000m);
        var quantities = new Dictionary<long, int>() { { 10L, 5 } };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync(dummyBasket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var catalogRepo = Mock.Of<ICatalogRepository>();
        var catalogDomainService = new Mock<ICatalogDomainService>();
        catalogDomainService
            .Setup(d => d.ExistsAllAsync(quantities.Keys, AnyToken))
            .ReturnsAsync((false, new List<CatalogItem>().AsReadOnly()));
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, catalogRepo, catalogDomainService.Object, logger);

        // Act
        var action = () => service.SetBasketItemsQuantitiesAsync(dummyBuyerId, quantities);

        // Assert
        await Assert.ThrowsAsync<CatalogItemNotExistingInRepositoryException>(action);
    }

    [Fact]
    public async Task SetBasketItemsQuantitiesAsync_買い物かご内とカタログリポジトリに存在するカタログアイテムが数量設定対象_買い物かごリポジトリのUpdateAsyncを1度だけ呼び出す()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        dummyBasket.AddItem(10L, 1000m);
        var quantities = new Dictionary<long, int>() { { 10L, 5 } };
        var catalogItems = new List<CatalogItem>
         {
             new() { CatalogCategoryId = 100L, CatalogBrandId = 110L, Description = "説明1", Name = "ダミー商品1", Price = 1000m, ProductCode = "C000000001", Id = 10L },
         };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync(dummyBasket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var catalogRepo = Mock.Of<ICatalogRepository>();
        var catalogDomainService = new Mock<ICatalogDomainService>();
        catalogDomainService
            .Setup(d => d.ExistsAllAsync(quantities.Keys, AnyToken))
            .ReturnsAsync((true, catalogItems.AsReadOnly()));
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, catalogRepo, catalogDomainService.Object, logger);

        // Act
        await service.SetBasketItemsQuantitiesAsync(dummyBuyerId, quantities);

        // Assert
        basketRepo.Verify(
            r => r.UpdateAsync(It.Is<Basket>(b => b.BuyerId == dummyBuyerId), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task SetBasketItemsQuantitiesAsync_買い物かご内とカタログリポジトリに存在するカタログアイテムが数量設定対象_買い物かごの商品数が更新される()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        dummyBasket.AddItem(10L, 1000m);
        var newQuantity = 5;
        var quantities = new Dictionary<long, int>() { { 10L, newQuantity } };
        var catalogItems = new List<CatalogItem>
         {
             new() { CatalogCategoryId = 100L, CatalogBrandId = 110L, Description = "説明1", Name = "ダミー商品1", Price = 1000m, ProductCode = "C000000001", Id = 10L },
         };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync(dummyBasket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var catalogRepo = Mock.Of<ICatalogRepository>();
        var catalogDomainService = new Mock<ICatalogDomainService>();
        catalogDomainService
            .Setup(d => d.ExistsAllAsync(quantities.Keys, AnyToken))
            .ReturnsAsync((true, catalogItems.AsReadOnly()));
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, catalogRepo, catalogDomainService.Object, logger);

        // Act
        await service.SetBasketItemsQuantitiesAsync(dummyBuyerId, quantities);

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
        dummyBasket.AddItem(10L, 1000m);
        var quantities = new Dictionary<long, int>() { { 10L, 0 } };
        var catalogItems = new List<CatalogItem>
         {
             new() { CatalogCategoryId = 100L, CatalogBrandId = 110L, Description = "説明1", Name = "ダミー商品1", Price = 1000m, ProductCode = "C000000001", Id = 10L },
         };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync(dummyBasket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var catalogRepo = Mock.Of<ICatalogRepository>();
        var catalogDomainService = new Mock<ICatalogDomainService>();
        catalogDomainService
            .Setup(d => d.ExistsAllAsync(quantities.Keys, AnyToken))
            .ReturnsAsync((true, catalogItems.AsReadOnly()));
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, catalogRepo, catalogDomainService.Object, logger);

        // Act
        await service.SetBasketItemsQuantitiesAsync(dummyBuyerId, quantities);

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
        var catalogRepo = Mock.Of<ICatalogRepository>();
        var catalogDomainService = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo, orderRepo, orderFactory, catalogRepo, catalogDomainService, logger);

        // Act
        var action = () => service.AddItemToBasketAsync(nullOrEmptyBuyerId!, 10L, 5);

        // Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>("buyerId", action);
        Assert.StartsWith("null または空の文字列を設定できません。", ex.Message);
    }

    [Fact]
    public async Task AddItemToBasketAsync_カタログリポジトリに存在しないカタログアイテムが追加対象_CatalogItemNotExistingInRepositoryExceptionを返す()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        var quantities = new Dictionary<long, int>() { { 10L, 5 } };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync(dummyBasket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var catalogRepo = Mock.Of<ICatalogRepository>();
        var catalogDomainService = new Mock<ICatalogDomainService>();
        catalogDomainService
            .Setup(d => d.ExistsAllAsync(quantities.Keys, AnyToken))
            .ReturnsAsync((false, new List<CatalogItem>().AsReadOnly()));
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, catalogRepo, catalogDomainService.Object, logger);

        // Act
        var action = () => service.AddItemToBasketAsync(dummyBuyerId, 10L, 5);

        // Assert
        await Assert.ThrowsAsync<CatalogItemNotExistingInRepositoryException>(action);
    }

    [Fact]
    public async Task AddItemToBasketAsync_カタログリポジトリに存在するカタログアイテムが追加対象_買い物かごリポジトリのUpdateAsyncを1度だけ呼び出す()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        var quantities = new Dictionary<long, int>() { { 10L, 5 } };
        var catalogItems = new List<CatalogItem>
         {
             new() { CatalogCategoryId = 100L, CatalogBrandId = 110L, Description = "説明1", Name = "ダミー商品1", Price = 1000m, ProductCode = "C000000001", Id = 10L },
         };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync(dummyBasket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var catalogRepo = Mock.Of<ICatalogRepository>();
        var catalogDomainService = new Mock<ICatalogDomainService>();
        catalogDomainService
            .Setup(d => d.ExistsAllAsync(quantities.Keys, AnyToken))
            .ReturnsAsync((true, catalogItems.AsReadOnly()));
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, catalogRepo, catalogDomainService.Object, logger);

        // Act
        await service.AddItemToBasketAsync(dummyBuyerId, 10L, 5);

        // Assert
        basketRepo.Verify(
            r => r.UpdateAsync(It.Is<Basket>(b => b.BuyerId == dummyBuyerId), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task AddItemToBasketAsync_カタログリポジトリに存在するカタログアイテムが追加対象_買い物かごに追加対象の商品が追加される()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        var quantities = new Dictionary<long, int>() { { 10L, 5 } };
        var catalogItem = new CatalogItem() { CatalogCategoryId = 100L, CatalogBrandId = 110L, Description = "説明1", Name = "ダミー商品1", Price = 1000m, ProductCode = "C000000001", Id = 10L };
        var catalogItems = new List<CatalogItem> { catalogItem };

        var basketRepo = new Mock<IBasketRepository>();
        basketRepo
            .Setup(r => r.GetWithBasketItemsAsync(dummyBuyerId, AnyToken))
            .ReturnsAsync(dummyBasket);
        var orderRepo = Mock.Of<IOrderRepository>();
        var orderFactory = Mock.Of<IOrderFactory>();
        var catalogRepo = Mock.Of<ICatalogRepository>();
        var catalogDomainService = new Mock<ICatalogDomainService>();
        catalogDomainService
            .Setup(d => d.ExistsAllAsync(quantities.Keys, AnyToken))
            .ReturnsAsync((true, catalogItems.AsReadOnly()));
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, catalogRepo, catalogDomainService.Object, logger);

        // Act
        await service.AddItemToBasketAsync(dummyBuyerId, 10L, 5);

        // Assert
        basketRepo.Verify(
            r => r.UpdateAsync(
            It.Is<Basket>(b => b.Items.Count == 1), AnyToken),
            Times.Once);
        basketRepo.Verify(
            r => r.UpdateAsync(
            It.Is<Basket>(b => b.Items.ToList().Exists(item => (item.CatalogItemId == catalogItem.Id) && (item.Quantity == 5) && (item.UnitPrice == catalogItem.Price))), AnyToken),
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
        var catalogRepo = Mock.Of<ICatalogRepository>();
        var catalogDomainService = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo, orderRepo, orderFactory, catalogRepo, catalogDomainService, logger);

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
        var catalogRepo = Mock.Of<ICatalogRepository>();
        var catalogDomainService = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, catalogRepo, catalogDomainService, logger);

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
        var catalogRepo = Mock.Of<ICatalogRepository>();
        var catalogDomainService = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo, orderFactory, catalogRepo, catalogDomainService, logger);

        // Act
        var action = () => service.CheckoutAsync(dummyBuyerId, shipTo);

        // Assert
        await Assert.ThrowsAsync<EmptyBasketOnCheckoutException>(action);
    }

    [Fact]
    public async Task CheckoutAsync_買い物かごにアイテムが存在する_カタログリポジトリのFindAsyncを1度だけ呼び出す()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        dummyBasket.AddItem(10L, 1000);
        var shipTo = CreateDefaultShipTo();
        var catalogItems = new List<CatalogItem>
         {
             new() { CatalogCategoryId = 100L, CatalogBrandId = 110L, Description = "説明1", Name = "ダミー商品1", Price = 1000m, ProductCode = "C000000001", Id = 10L },
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
            .Setup(f => f.CreateOrder(dummyBasket, catalogItems, shipTo))
            .Returns(order);
        var catalogRepo = new Mock<ICatalogRepository>();
        catalogRepo
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken))
            .ReturnsAsync(catalogItems.AsReadOnly());
        var catalogDomainService = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo.Object, orderFactory.Object, catalogRepo.Object, catalogDomainService, logger);

        // Act
        await service.CheckoutAsync(dummyBuyerId, shipTo);

        // Assert
        catalogRepo.Verify(
            r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task CheckoutAsync_買い物かごにアイテムが存在する_注文リポジトリのAddAsyncを1度だけ呼び出す()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        dummyBasket.AddItem(10L, 1000);
        var shipTo = CreateDefaultShipTo();
        var catalogItems = new List<CatalogItem>
         {
             new() { CatalogCategoryId = 100L, CatalogBrandId = 110L, Description = "説明1", Name = "ダミー商品1", Price = 1000m, ProductCode = "C000000001", Id = 10L },
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
            .Setup(f => f.CreateOrder(dummyBasket, catalogItems, shipTo))
            .Returns(order);
        var catalogRepo = new Mock<ICatalogRepository>();
        catalogRepo
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken))
            .ReturnsAsync(catalogItems.AsReadOnly());
        var catalogDomainService = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo.Object, orderFactory.Object, catalogRepo.Object, catalogDomainService, logger);

        // Act
        await service.CheckoutAsync(dummyBuyerId, shipTo);

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
        dummyBasket.AddItem(10L, 1000);
        var shipTo = CreateDefaultShipTo();
        var catalogItems = new List<CatalogItem>
         {
             new() { CatalogCategoryId = 100L, CatalogBrandId = 110L, Description = "説明1", Name = "ダミー商品1", Price = 1000m, ProductCode = "C000000001", Id = 10L },
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
            .Setup(f => f.CreateOrder(dummyBasket, catalogItems, shipTo))
            .Returns(order);
        var catalogRepo = new Mock<ICatalogRepository>();
        catalogRepo
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken))
            .ReturnsAsync(catalogItems.AsReadOnly());
        var catalogDomainService = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo.Object, orderFactory.Object, catalogRepo.Object, catalogDomainService, logger);

        // Act
        await service.CheckoutAsync(dummyBuyerId, shipTo);

        // Assert
        orderRepo.Verify(
            r => r.AddAsync(It.Is<Order>(o => o.ShipToAddress == shipTo), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task CheckoutAsync_買い物かごにアイテムが存在する_買い物かごリポジトリのRemoveAsyncを1度だけ呼び出す()
    {
        // Arrange
        var dummyBuyerId = "dummyId";
        var dummyBasket = new Basket { BuyerId = dummyBuyerId };
        dummyBasket.AddItem(10L, 1000);
        var shipTo = CreateDefaultShipTo();
        var catalogItems = new List<CatalogItem>
         {
             new() { CatalogCategoryId = 100L, CatalogBrandId = 110L, Description = "説明1", Name = "ダミー商品1", Price = 1000m, ProductCode = "C000000001", Id = 10L },
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
            .Setup(f => f.CreateOrder(dummyBasket, catalogItems, shipTo))
            .Returns(order);
        var catalogRepo = new Mock<ICatalogRepository>();
        catalogRepo
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken))
            .ReturnsAsync(catalogItems.AsReadOnly());
        var catalogDomainService = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<ShoppingApplicationService>();
        var service = new ShoppingApplicationService(basketRepo.Object, orderRepo.Object, orderFactory.Object, catalogRepo.Object, catalogDomainService, logger);

        // Act
        await service.CheckoutAsync(dummyBuyerId, shipTo);

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
            new() { ItemOrdered = new CatalogItemOrdered(1, productName, productCode), UnitPrice = 1000m, Quantity = 1 },
        };

        return items;
    }
}
