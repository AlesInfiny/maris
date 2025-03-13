using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.Catalog;
using Dressca.ApplicationCore.Ordering;

namespace Dressca.UnitTests.ApplicationCore.ApplicationService;

public class OrderApplicationServiceTest(ITestOutputHelper testOutputHelper) : TestBase(testOutputHelper)
{
    private static CancellationToken AnyToken => It.IsAny<CancellationToken>();

    [Fact]
    public async Task GetOrderAsync_注文リポジトリから取得した情報と指定した購入者IDが合致する_注文情報を取得できる()
    {
        // Arrange
        var orderId = 10L;
        var buyerId = Guid.NewGuid().ToString("D");
        var shipToAddress = CreateDefaultShipTo();
        var orderItems = CreateDefaultOrderItems();
        var order = new Order(orderItems) { BuyerId = buyerId, ShipToAddress = shipToAddress };
        var orderRepositoryMock = new Mock<IOrderRepository>();
        orderRepositoryMock
            .Setup(r => r.FindAsync(orderId, AnyToken))
            .ReturnsAsync(order);
        var basketRepository = Mock.Of<IBasketRepository>();
        var catalogRepository = Mock.Of<ICatalogRepository>();
        var logger = this.CreateTestLogger<OrderApplicationService>();
        var service = new OrderApplicationService(orderRepositoryMock.Object, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var actual = await service.GetOrderAsync(orderId, buyerId, cancellationToken);

        // Assert
        Assert.Same(order, actual);
    }

    [Fact]
    public async Task GetOrderAsync_注文リポジトリのFindAsyncを1度だけ呼び出す()
    {
        // Arrange
        var orderId = 10L;
        var buyerId = Guid.NewGuid().ToString("D");
        var shipToAddress = CreateDefaultShipTo();
        var orderItems = CreateDefaultOrderItems();
        var order = new Order(orderItems) { BuyerId = buyerId, ShipToAddress = shipToAddress };
        var orderRepositoryMock = new Mock<IOrderRepository>();
        orderRepositoryMock
            .Setup(r => r.FindAsync(orderId, AnyToken))
            .ReturnsAsync(order);
        var basketRepository = Mock.Of<IBasketRepository>();
        var catalogRepository = Mock.Of<ICatalogRepository>();
        var logger = this.CreateTestLogger<OrderApplicationService>();
        var service = new OrderApplicationService(orderRepositoryMock.Object, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var actual = await service.GetOrderAsync(orderId, buyerId, cancellationToken);

        // Assert
        orderRepositoryMock.Verify(r => r.FindAsync(orderId, AnyToken), Times.Once);
    }

    [Fact]
    public async Task GetOrderAsync_注文リポジトリから取得した情報と指定した購入者IDが異なる_OrderNotFoundExceptionが発生する()
    {
        // Arrange
        var orderId = 10L;
        var buyerId = Guid.NewGuid().ToString("D");
        var shipToAddress = CreateDefaultShipTo();
        var orderItems = CreateDefaultOrderItems();
        var order = new Order(orderItems) { BuyerId = buyerId, ShipToAddress = shipToAddress };
        var orderRepositoryMock = new Mock<IOrderRepository>();
        orderRepositoryMock
            .Setup(r => r.FindAsync(orderId, AnyToken))
            .ReturnsAsync(order);
        var basketRepository = Mock.Of<IBasketRepository>();
        var catalogRepository = Mock.Of<ICatalogRepository>();
        var logger = this.CreateTestLogger<OrderApplicationService>();
        var service = new OrderApplicationService(orderRepositoryMock.Object, logger);

        // Act
        var action = () => service.GetOrderAsync(orderId, "dummy");

        // Assert
        await Assert.ThrowsAsync<OrderNotFoundException>(action);
    }

    [Fact]
    public async Task GetOrderAsync_注文リポジトリから注文情報を取得できない_OrderNotFoundExceptionが発生する()
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
        var logger = this.CreateTestLogger<OrderApplicationService>();
        var service = new OrderApplicationService(orderRepositoryMock.Object, logger);

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
            new() { ItemOrdered = new CatalogItemOrdered(1, productName, productCode), UnitPrice = 1000m, Quantity = 1 },
        };

        return items;
    }
}
