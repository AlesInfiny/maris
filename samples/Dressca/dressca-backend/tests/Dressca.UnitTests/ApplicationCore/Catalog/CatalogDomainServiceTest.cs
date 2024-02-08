﻿using System.Linq.Expressions;
using Dressca.ApplicationCore.Catalog;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Dressca.UnitTests.ApplicationCore.Catalog;

public class CatalogDomainServiceTest(ITestOutputHelper testOutputHelper) : TestBase(testOutputHelper)
{
    private static CancellationToken AnyToken => It.IsAny<CancellationToken>();

    [Fact]
    public async Task ExistsAllAsync_カタログアイテムIdがすべて存在する_existsAllはfalse_itemsは見つかったカタログアイテムのリスト()
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

        var logger = this.CreateTestLogger<CatalogDomainService>();
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
    public async Task ExistsAllAsync_カタログアイテムIdが一部だけ存在する_existsAllはfalse_itemsは見つかったカタログアイテムのリスト()
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

        var logger = this.CreateTestLogger<CatalogDomainService>();
        var domainService = new CatalogDomainService(catalogRepositoryMock.Object, logger);

        // Act
        var (existsAll, items) = await domainService.ExistsAllAsync(new[] { 1L, 2L });

        // Assert
        Assert.False(existsAll);
        Assert.Single(items, item => item.Id == 2L);
    }

    [Fact]
    public async Task ExistsAllAsync_カタログアイテムIdが一部だけ存在する_情報ログが1件出る()
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

        var logger = this.CreateTestLogger<CatalogDomainService>();
        var domainService = new CatalogDomainService(catalogRepositoryMock.Object, logger);

        // Act
        _ = await domainService.ExistsAllAsync(new[] { 1L, 2L });

        // Assert
        Assert.Equal(1, this.LogCollector.Count);
        var record = this.LogCollector.LatestRecord;
        Assert.Equal("指定されたカタログアイテム ID: [1] のカタログアイテムがリポジトリに存在しません。", record.Message);
        Assert.Equal(LogLevel.Information, record.Level);
        Assert.Equal(new EventId(0), record.Id);
    }

    [Fact]
    public async Task ExistsAllAsync_カタログアイテムIdが1件も存在しない_existsAllはfalse_itemsは空()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var catalogItems = new List<CatalogItem>();
        catalogRepositoryMock
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken))
            .ReturnsAsync(catalogItems);

        var logger = this.CreateTestLogger<CatalogDomainService>();
        var domainService = new CatalogDomainService(catalogRepositoryMock.Object, logger);

        // Act
        var (existsAll, items) = await domainService.ExistsAllAsync(new[] { 1L });

        // Assert
        Assert.False(existsAll);
        Assert.Empty(items);
    }

    [Fact]
    public async Task ExistsAllAsync_カタログアイテムIdが1件も存在しない_情報ログが1件出る()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var catalogItems = new List<CatalogItem>();
        catalogRepositoryMock
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken))
            .ReturnsAsync(catalogItems);

        var logger = this.CreateTestLogger<CatalogDomainService>();
        var domainService = new CatalogDomainService(catalogRepositoryMock.Object, logger);

        // Act
        _ = await domainService.ExistsAllAsync(new[] { 1L });

        // Assert
        Assert.Equal(1, this.LogCollector.Count);
        var record = this.LogCollector.LatestRecord;
        Assert.Equal("指定されたカタログアイテム ID: [1] のカタログアイテムがリポジトリに存在しません。", record.Message);
        Assert.Equal(LogLevel.Information, record.Level);
        Assert.Equal(new EventId(0), record.Id);
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
