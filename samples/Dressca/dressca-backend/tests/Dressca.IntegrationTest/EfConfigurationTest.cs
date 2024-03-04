using Dressca.ApplicationCore.Ordering;
using Xunit;

namespace Dressca.IntegrationTest;

[Collection("TransactionalTests")]
public class EfConfigurationTest(TransactionalTestDatabaseFixture fixture) : IDisposable
{
    public TransactionalTestDatabaseFixture Fixture { get; private set; } = fixture;

    [Fact]
    public async void ShipTo_Address_値オブジェクトはコピーをINSERTできる()
    {
        using var dbContext = this.Fixture.CreateContext();

        // Arrange
        var buyerId = Guid.NewGuid().ToString("D");
        var shipTo = CreateDefaultShipTo();
        var items = CreateDefaultOrderItems();
        var order = new Order(buyerId, shipTo, items);
        dbContext.Add(order);
        await dbContext.SaveChangesAsync();
        var copiedOrder = new Order(order.BuyerId, order.ShipToAddress, order.OrderItems.ToList());

        // Act
        dbContext.Add(copiedOrder);
        var result = await dbContext.SaveChangesAsync();
        var orderList = dbContext.Orders.Where(o => o.BuyerId == buyerId).ToList();

        // Assert
        foreach (Order testOrder in orderList)
        {
            Assert.Equal(shipTo, testOrder.ShipToAddress);
        }
    }

    public void Dispose()
            => this.Fixture.Cleanup();

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
        const string productName = "ダミー商品1";
        const string productCode = "C000000001";

        var items = new List<OrderItem>()
        {
            new(new CatalogItemOrdered(1, productName, productCode), 1000m, 1),
        };

        return items;
    }
}
