using System.Linq.Expressions;
using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.Catalog;
using Dressca.ApplicationCore.Ordering;
using Maris.Diagnostics.Testing.Xunit;
using Xunit.Abstractions;

namespace Dressca.UnitTests.ApplicationCore.Ordering;

public class OrderApplicationServiceTest
{
    private readonly TestLoggerManager loggerManager;

    public OrderApplicationServiceTest(ITestOutputHelper testOutputHelper)
        => this.loggerManager = new TestLoggerManager(testOutputHelper);

    private static CancellationToken AnyToken => It.IsAny<CancellationToken>();

    [Fact]
    public async Task CreateOrderAsync_注文作成処理は注文リポジトリのAddを1回呼出す()
    {
        // Arrange
        const long basketId = 1L;
        var buyerId = Guid.NewGuid().ToString("D");
        var basket = new Basket(buyerId);
        basket.AddItem(10L, 1000m);
        var shipTo = CreateDefaultShipTo();
        var catalogItems = new List<CatalogItem>
        {
            new(100L, 110L, "説明1", "ダミー商品1", 1000m, "C000000001") { Id = 10L },
        };
        var basketRepositoryMock = new Mock<IBasketRepository>();
        basketRepositoryMock
            .Setup(r => r.GetWithBasketItemsAsync(basketId, AnyToken))
            .ReturnsAsync(basket);
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        catalogRepositoryMock
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken))
            .ReturnsAsync(catalogItems.AsReadOnly());
        var orderRepositoryMock = new Mock<IOrderRepository>();
        orderRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Order>(), AnyToken))
            .ReturnsAsync(new Order(buyerId, shipTo, CreateDefaultOrderItems()));
        var logger = this.loggerManager.CreateLogger<OrderApplicationService>();
        var service = new OrderApplicationService(orderRepositoryMock.Object, basketRepositoryMock.Object, catalogRepositoryMock.Object, logger);

        // Act
        _ = await service.CreateOrderAsync(basketId, shipTo);

        // Assert
        orderRepositoryMock.Verify(
            r => r.AddAsync(It.Is<Order>(o => o.BuyerId == buyerId), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task CreateOrderAsync_注文作成処理で指定した買い物かごが存在しない場合は業務例外が発生する()
    {
        // Arrange
        const long basketId = 999L;
        var basketRepositoryMock = new Mock<IBasketRepository>();
        basketRepositoryMock
            .Setup(r => r.GetWithBasketItemsAsync(basketId, AnyToken))
            .ReturnsAsync((Basket?)null);
        var catalogRepository = Mock.Of<ICatalogRepository>();
        var orderRepository = Mock.Of<IOrderRepository>();
        var logger = this.loggerManager.CreateLogger<OrderApplicationService>();
        var service = new OrderApplicationService(orderRepository, basketRepositoryMock.Object, catalogRepository, logger);
        var shipTo = CreateDefaultShipTo();

        // Act
        var action = () => service.CreateOrderAsync(basketId, shipTo);

        // Assert
        await Assert.ThrowsAsync<BasketNotFoundException>(action);
    }

    [Fact]
    public async Task CreateOrderAsync_注文作成処理で指定した買い物かごが空の場合は業務例外が発生する()
    {
        // Arrange
        const long basketId = 3L;
        string buyerId = Guid.NewGuid().ToString("D");
        var basket = new Basket(buyerId);
        var basketRepositoryMock = new Mock<IBasketRepository>();
        basketRepositoryMock
            .Setup(r => r.GetWithBasketItemsAsync(basketId, AnyToken))
            .ReturnsAsync(basket);
        var catalogRepository = Mock.Of<ICatalogRepository>();
        var orderRepository = Mock.Of<IOrderRepository>();
        var logger = this.loggerManager.CreateLogger<OrderApplicationService>();
        var service = new OrderApplicationService(orderRepository, basketRepositoryMock.Object, catalogRepository, logger);
        var shipTo = CreateDefaultShipTo();

        // Act
        var action = () => service.CreateOrderAsync(basketId, shipTo);

        // Assert
        await Assert.ThrowsAsync<EmptyBasketOnCheckoutException>(action);
    }

    [Fact]
    public async Task GetOrderAsync_注文リポジトリから取得した情報と指定した購入者IDが合致する場合注文情報を取得できる()
    {
        // Arrange
        var orderId = 10L;
        var buyerId = Guid.NewGuid().ToString("D");
        var shipToAddress = CreateDefaultShipTo();
        var orderItems = CreateDefaultOrderItems();
        var order = new Order(buyerId, shipToAddress, orderItems);
        var orderRepositoryMock = new Mock<IOrderRepository>();
        orderRepositoryMock
            .Setup(r => r.FindAsync(orderId, AnyToken))
            .ReturnsAsync(order);
        var basketRepository = Mock.Of<IBasketRepository>();
        var catalogRepository = Mock.Of<ICatalogRepository>();
        var logger = this.loggerManager.CreateLogger<OrderApplicationService>();
        var service = new OrderApplicationService(orderRepositoryMock.Object, basketRepository, catalogRepository, logger);

        // Act
        var actual = await service.GetOrderAsync(orderId, buyerId);

        // Assert
        Assert.Same(order, actual);
    }

    [Fact]
    public async Task GetOrderAsync_注文リポジトリから取得した情報と指定した購入者IDが異なる場合例外になる()
    {
        // Arrange
        var orderId = 10L;
        var buyerId = Guid.NewGuid().ToString("D");
        var shipToAddress = CreateDefaultShipTo();
        var orderItems = CreateDefaultOrderItems();
        var order = new Order(buyerId, shipToAddress, orderItems);
        var orderRepositoryMock = new Mock<IOrderRepository>();
        orderRepositoryMock
            .Setup(r => r.FindAsync(orderId, AnyToken))
            .ReturnsAsync(order);
        var basketRepository = Mock.Of<IBasketRepository>();
        var catalogRepository = Mock.Of<ICatalogRepository>();
        var logger = this.loggerManager.CreateLogger<OrderApplicationService>();
        var service = new OrderApplicationService(orderRepositoryMock.Object, basketRepository, catalogRepository, logger);

        // Act
        var action = () => service.GetOrderAsync(orderId, "dummy");

        // Assert
        await Assert.ThrowsAsync<OrderNotFoundException>(action);
    }

    [Fact]
    public async Task GetOrderAsync_注文リポジトリから注文情報を取得できない場合例外になる()
    {
        // Arrange
        var orderId = 10L;
        var buyerId = Guid.NewGuid().ToString("D");
        var orderRepositoryMock = new Mock<IOrderRepository>();
        orderRepositoryMock
            .Setup(r => r.FindAsync(orderId, AnyToken))
            .ReturnsAsync((Order?)null);
        var basketRepository = Mock.Of<IBasketRepository>();
        var catalogRepository = Mock.Of<ICatalogRepository>();
        var logger = this.loggerManager.CreateLogger<OrderApplicationService>();
        var service = new OrderApplicationService(orderRepositoryMock.Object, basketRepository, catalogRepository, logger);

        // Act
        var action = () => service.GetOrderAsync(orderId, buyerId);

        // Assert
        await Assert.ThrowsAsync<OrderNotFoundException>(action);
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
            new(new CatalogItemOrdered(1, productName, productCode), 1000m, 1),
        };

        return items;
    }
}
