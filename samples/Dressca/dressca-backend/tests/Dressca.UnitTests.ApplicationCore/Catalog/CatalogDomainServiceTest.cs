using System.Linq.Expressions;
using Dressca.ApplicationCore.Catalog;
using Microsoft.Extensions.Logging;

namespace Dressca.UnitTests.ApplicationCore.Catalog;

public class CatalogDomainServiceTest(ITestOutputHelper testOutputHelper) : TestBase(testOutputHelper)
{
    private static CancellationToken AnyToken => It.IsAny<CancellationToken>();

    [Fact]
    public async Task ExistsAllAsync_カタログアイテムIdがすべて存在する_existsAllはfalse_itemsは見つかったカタログアイテムのリスト()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var catalogBrandRepositoryMock = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = Mock.Of<ICatalogCategoryRepository>();
        var catalogItems = new List<CatalogItem>
        {
            CreateCatalogItem(1L),
            CreateCatalogItem(2L),
        };
        catalogRepositoryMock
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken))
            .ReturnsAsync(catalogItems);

        var logger = this.CreateTestLogger<CatalogDomainService>();
        var domainService = new CatalogDomainService(catalogRepositoryMock.Object, catalogBrandRepositoryMock, catalogCategoryRepositoryMock, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var (existsAll, items) = await domainService.ExistsAllAsync([1L, 2L], cancellationToken);

        // Assert
        Assert.True(existsAll);
        Assert.Collection(
            items,
            item => Assert.Equal(1L, item.Id),
            item => Assert.Equal(2L, item.Id));
    }

    [Fact]
    public async Task ExistsAllAsync_カタログアイテムIdが一部だけ存在する_existsAllはfalse_itemsは見つかったカタログアイテムのリスト()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var catalogBrandRepositoryMock = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = Mock.Of<ICatalogCategoryRepository>();
        var catalogItems = new List<CatalogItem>
        {
            CreateCatalogItem(2L),
        };
        catalogRepositoryMock
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken))
            .ReturnsAsync(catalogItems);

        var logger = this.CreateTestLogger<CatalogDomainService>();
        var domainService = new CatalogDomainService(catalogRepositoryMock.Object, catalogBrandRepositoryMock, catalogCategoryRepositoryMock, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var (existsAll, items) = await domainService.ExistsAllAsync([1L, 2L], cancellationToken);

        // Assert
        Assert.False(existsAll);
        Assert.Single(items, item => item.Id == 2L);
    }

    [Fact]
    public async Task ExistsAllAsync_カタログアイテムIdが一部だけ存在する_情報ログが1件出る()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var catalogBrandRepositoryMock = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = Mock.Of<ICatalogCategoryRepository>();
        var catalogItems = new List<CatalogItem>
        {
            CreateCatalogItem(2L),
        };
        catalogRepositoryMock
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken))
            .ReturnsAsync(catalogItems);

        var logger = this.CreateTestLogger<CatalogDomainService>();
        var domainService = new CatalogDomainService(catalogRepositoryMock.Object, catalogBrandRepositoryMock, catalogCategoryRepositoryMock, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        _ = await domainService.ExistsAllAsync([1L, 2L], cancellationToken);

        // Assert
        Assert.Equal(1, this.LogCollector.Count);
        var record = this.LogCollector.LatestRecord;
        Assert.Equal("指定されたカタログアイテム ID: [1] のカタログアイテムがリポジトリに存在しません。", record.Message);
        Assert.Equal(LogLevel.Information, record.Level);
        Assert.Equal(1001, record.Id);
    }

    [Fact]
    public async Task ExistsAllAsync_カタログアイテムIdが1件も存在しない_existsAllはfalse_itemsは空()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var catalogBrandRepositoryMock = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = Mock.Of<ICatalogCategoryRepository>();
        var catalogItems = new List<CatalogItem>();
        catalogRepositoryMock
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken))
            .ReturnsAsync(catalogItems);

        var logger = this.CreateTestLogger<CatalogDomainService>();
        var domainService = new CatalogDomainService(catalogRepositoryMock.Object, catalogBrandRepositoryMock, catalogCategoryRepositoryMock, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var (existsAll, items) = await domainService.ExistsAllAsync([1L], cancellationToken);

        // Assert
        Assert.False(existsAll);
        Assert.Empty(items);
    }

    [Fact]
    public async Task ExistsAllAsync_カタログアイテムIdが1件も存在しない_情報ログが1件出る()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var catalogBrandRepositoryMock = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = Mock.Of<ICatalogCategoryRepository>();
        var catalogItems = new List<CatalogItem>();
        catalogRepositoryMock
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken))
            .ReturnsAsync(catalogItems);

        var logger = this.CreateTestLogger<CatalogDomainService>();
        var domainService = new CatalogDomainService(catalogRepositoryMock.Object, catalogBrandRepositoryMock, catalogCategoryRepositoryMock, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        _ = await domainService.ExistsAllAsync([1L], cancellationToken);

        // Assert
        Assert.Equal(1, this.LogCollector.Count);
        var record = this.LogCollector.LatestRecord;
        Assert.Equal("指定されたカタログアイテム ID: [1] のカタログアイテムがリポジトリに存在しません。", record.Message);
        Assert.Equal(LogLevel.Information, record.Level);
        Assert.Equal(1001, record.Id);
    }

    [Fact]
    public async Task ItemExistsAsync_対象のアイテムが存在する_true()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var catalogBrandRepositoryMock = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = Mock.Of<ICatalogCategoryRepository>();
        var logger = this.CreateTestLogger<CatalogDomainService>();
        var domainService = new CatalogDomainService(catalogRepositoryMock.Object, catalogBrandRepositoryMock, catalogCategoryRepositoryMock, logger);
        var targetItemId = 123L;

        catalogRepositoryMock
            .Setup(r => r.AnyAsync(targetItemId, AnyToken))
            .ReturnsAsync(true);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var result = await domainService.ItemExistsAsync(targetItemId, cancellationToken);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ItemExistsAsync_対象のアイテムが存在しない_false()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var catalogBrandRepositoryMock = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = Mock.Of<ICatalogCategoryRepository>();
        var logger = this.CreateTestLogger<CatalogDomainService>();
        var domainService = new CatalogDomainService(catalogRepositoryMock.Object, catalogBrandRepositoryMock, catalogCategoryRepositoryMock, logger);
        var targetItemId = 123L;

        catalogRepositoryMock
            .Setup(r => r.AnyAsync(targetItemId, AnyToken))
            .ReturnsAsync(false);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var result = await domainService.ItemExistsAsync(targetItemId, cancellationToken);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task BrandExistsAsync_対象のブランドが存在する_true()
    {
        // Arrange
        var catalogRepositoryMock = Mock.Of<ICatalogRepository>();
        var catalogBrandRepositoryMock = new Mock<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = Mock.Of<ICatalogCategoryRepository>();
        var logger = this.CreateTestLogger<CatalogDomainService>();
        var domainService = new CatalogDomainService(catalogRepositoryMock, catalogBrandRepositoryMock.Object, catalogCategoryRepositoryMock, logger);
        var targetBrandId = 123L;

        catalogBrandRepositoryMock
            .Setup(r => r.AnyAsync(targetBrandId, AnyToken))
            .ReturnsAsync(true);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var result = await domainService.BrandExistsAsync(targetBrandId, cancellationToken);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task BrandExistsAsync_対象のブランドが存在しない_false()
    {
        // Arrange
        var catalogRepositoryMock = Mock.Of<ICatalogRepository>();
        var catalogBrandRepositoryMock = new Mock<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = Mock.Of<ICatalogCategoryRepository>();
        var logger = this.CreateTestLogger<CatalogDomainService>();
        var domainService = new CatalogDomainService(catalogRepositoryMock, catalogBrandRepositoryMock.Object, catalogCategoryRepositoryMock, logger);
        var targetBrandId = 123L;

        catalogBrandRepositoryMock
            .Setup(r => r.AnyAsync(targetBrandId, AnyToken))
            .ReturnsAsync(false);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var result = await domainService.BrandExistsAsync(targetBrandId, cancellationToken);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CategoryExistsAsync_対象のカテゴリが存在する_true()
    {
        // Arrange
        var catalogRepositoryMock = Mock.Of<ICatalogRepository>();
        var catalogBrandRepositoryMock = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = new Mock<ICatalogCategoryRepository>();
        var logger = this.CreateTestLogger<CatalogDomainService>();
        var domainService = new CatalogDomainService(catalogRepositoryMock, catalogBrandRepositoryMock, catalogCategoryRepositoryMock.Object, logger);
        var targetCategoryId = 123L;

        catalogCategoryRepositoryMock
            .Setup(r => r.AnyAsync(targetCategoryId, AnyToken))
            .ReturnsAsync(true);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var result = await domainService.CategoryExistsAsync(targetCategoryId, cancellationToken);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CategoryExistsAsync_対象のカテゴリが存在しない_false()
    {
        // Arrange
        var catalogRepositoryMock = Mock.Of<ICatalogRepository>();
        var catalogBrandRepositoryMock = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = new Mock<ICatalogCategoryRepository>();
        var logger = this.CreateTestLogger<CatalogDomainService>();
        var domainService = new CatalogDomainService(catalogRepositoryMock, catalogBrandRepositoryMock, catalogCategoryRepositoryMock.Object, logger);
        var targetCategoryId = 123L;

        catalogCategoryRepositoryMock
            .Setup(r => r.AnyAsync(targetCategoryId, AnyToken))
            .ReturnsAsync(false);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var result = await domainService.CategoryExistsAsync(targetCategoryId, cancellationToken);

        // Assert
        Assert.False(result);
    }

    private static CatalogItem CreateCatalogItem(long id)
    {
        var random = new Random();
        long defaultCatalogCategoryId = random.NextInt64(1L, 1000L);
        long defaultCatalogBrandId = random.NextInt64(1L, 1000L);
        const string defaultDescription = "Description.";
        const string defaultName = "Name";
        const decimal defaultPrice = 100m;
        const string defaultProductCode = "C000000001";
        return new CatalogItem
        {
            CatalogCategoryId = defaultCatalogCategoryId,
            CatalogBrandId = defaultCatalogBrandId,
            Description = defaultDescription,
            Name = defaultName,
            Price = defaultPrice,
            ProductCode = defaultProductCode,
            Id = id,
            IsDeleted = false,
        };
    }
}
