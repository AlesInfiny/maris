using System.Linq.Expressions;
using Dressca.ApplicationCore.Catalog;
using Maris.Logging.Testing.Xunit;
using Xunit.Abstractions;

namespace Dressca.UnitTests.ApplicationCore.Catalog;

public class CatalogDomainServiceTest
{
    private readonly TestLoggerManager loggerManager;

    public CatalogDomainServiceTest(ITestOutputHelper testOutputHelper)
        => this.loggerManager = new TestLoggerManager(testOutputHelper);

    private static CancellationToken AnyToken => It.IsAny<CancellationToken>();

    [Fact]
    public async Task ExistsAllAsync_カタログアイテムIdがすべて存在する場合()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var catalogItems = new List<CatalogItem>
        {
            CreateCatalogItem(1L),
            CreateCatalogItem(2L),
        };
        catalogRepositoryMock
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken))
            .ReturnsAsync(catalogItems);

        var logger = this.loggerManager.CreateLogger<CatalogDomainService>();
        var domainService = new CatalogDomainService(catalogRepositoryMock.Object, logger);

        // Act
        var (existsAll, items) = await domainService.ExistsAllAsync(new[] { 1L, 2L });

        // Assert
        Assert.True(existsAll);
        Assert.Collection(
            items,
            item => Assert.Equal(1L, item.Id),
            item => Assert.Equal(2L, item.Id));
    }

    [Fact]
    public async Task ExistsAllAsync_カタログアイテムIdが一部だけ存在する場合()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var catalogItems = new List<CatalogItem>
        {
            CreateCatalogItem(2L),
        };
        catalogRepositoryMock
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken))
            .ReturnsAsync(catalogItems);

        var logger = this.loggerManager.CreateLogger<CatalogDomainService>();
        var domainService = new CatalogDomainService(catalogRepositoryMock.Object, logger);

        // Act
        var (existsAll, items) = await domainService.ExistsAllAsync(new[] { 1L, 2L });

        // Assert
        Assert.False(existsAll);
        Assert.Collection(
            items,
            item => Assert.Equal(2L, item.Id));
    }

    [Fact]
    public async Task ExistsAllAsync_カタログアイテムIdが1件も存在しない場合()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var catalogItems = new List<CatalogItem>();
        catalogRepositoryMock
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken))
            .ReturnsAsync(catalogItems);

        var logger = this.loggerManager.CreateLogger<CatalogDomainService>();
        var domainService = new CatalogDomainService(catalogRepositoryMock.Object, logger);

        // Act
        var (existsAll, items) = await domainService.ExistsAllAsync(new[] { 1L });

        // Assert
        Assert.False(existsAll);
        Assert.Empty(items);
    }

    private static CatalogItem CreateCatalogItem(long id)
    {
        var random = new Random();
        long defaultCatalogCategoryId = random.NextInt64(1000L);
        long defaultCatalogBrandId = random.NextInt64(1000L);
        const string defaultDescription = "Description.";
        const string defaultName = "Name";
        const decimal defaultPrice = 100m;
        const string defaultProductCode = "C000000001";
        return new CatalogItem(defaultCatalogCategoryId, defaultCatalogBrandId, defaultDescription, defaultName, defaultPrice, defaultProductCode) { Id = id };
    }
}
