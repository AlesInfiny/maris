using System.Linq.Expressions;
using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.Catalog;
using Dressca.ApplicationCore.Ordering;
using Dressca.TestLibrary.Xunit.Logging;
using Xunit.Abstractions;

namespace Dressca.UnitTests.ApplicationCore.Ordering;

public class OrderApplicationServiceTest
{
    private readonly XunitLoggerFactory loggerFactory;

    public OrderApplicationServiceTest(ITestOutputHelper testOutputHelper)
        => this.loggerFactory = XunitLoggerFactory.Create(testOutputHelper);

    private static CancellationToken AnyToken => It.IsAny<CancellationToken>();

    [Fact]
    public async Task 注文作成処理は注文リポジトリのAddを1回呼出す()
    {
        // Arrange
        const long basketId = 1L;
        const string buyerId = "user1";
        var basket = new Basket(buyerId);
        basket.AddItem(10L, 1000m);
        var shipTo = CreateDefaultShipTo();
        var catalogItems = new List<CatalogItem>
        {
            new CatalogItem(100L, 110L, "description", "name", 1000m, "productCode") { Id = 10L },
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
            .ReturnsAsync(new Order(buyerId, shipTo, new List<OrderItem>()));
        var logger = this.loggerFactory.CreateLogger<OrderApplicationService>();
        var service = new OrderApplicationService(orderRepositoryMock.Object, basketRepositoryMock.Object, catalogRepositoryMock.Object, logger);

        // Act
        _ = await service.CreateOrderAsync(basketId, shipTo);

        // Assert
        orderRepositoryMock.Verify(
            r => r.AddAsync(It.Is<Order>(o => o.BuyerId == buyerId), AnyToken),
            Times.Once);
    }

    [Fact]
    public void 注文作成処理で指定した買い物かごが存在しない場合は業務例外が発生する()
    {
        // Arrange
        const long basketId = 999L;
        var basketRepositoryMock = new Mock<IBasketRepository>();
        basketRepositoryMock
            .Setup(r => r.GetWithBasketItemsAsync(basketId, AnyToken))
            .ReturnsAsync((Basket?)null);
        var catalogRepository = Mock.Of<ICatalogRepository>();
        var orderRepository = Mock.Of<IOrderRepository>();
        var logger = this.loggerFactory.CreateLogger<OrderApplicationService>();
        var service = new OrderApplicationService(orderRepository, basketRepositoryMock.Object, catalogRepository, logger);
        var shipTo = CreateDefaultShipTo();

        // Act
        var action = () => service.CreateOrderAsync(basketId, shipTo);

        // Assert
        Assert.ThrowsAsync<BasketNotFoundException>(action);
    }

    [Fact]
    public void 注文作成処理で指定した買い物かごが空の場合は業務例外が発生する()
    {
        // Arrange
        const long basketId = 3L;
        const string buyerId = "user1";
        var basket = new Basket(buyerId);
        var basketRepositoryMock = new Mock<IBasketRepository>();
        basketRepositoryMock
            .Setup(r => r.GetWithBasketItemsAsync(basketId, AnyToken))
            .ReturnsAsync(basket);
        var catalogRepository = Mock.Of<ICatalogRepository>();
        var orderRepository = Mock.Of<IOrderRepository>();
        var logger = this.loggerFactory.CreateLogger<OrderApplicationService>();
        var service = new OrderApplicationService(orderRepository, basketRepositoryMock.Object, catalogRepository, logger);
        var shipTo = CreateDefaultShipTo();

        // Act
        var action = () => service.CreateOrderAsync(basketId, shipTo);

        // Assert
        Assert.ThrowsAsync<EmptyBasketOnCheckoutException>(action);
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
}
