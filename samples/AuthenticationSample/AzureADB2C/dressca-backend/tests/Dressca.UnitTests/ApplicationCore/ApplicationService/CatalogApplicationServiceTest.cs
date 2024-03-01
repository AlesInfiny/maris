using System.Linq.Expressions;
using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.Catalog;
using Xunit.Abstractions;

namespace Dressca.UnitTests.ApplicationCore.ApplicationService;

/// <summary>
///  カタログアプリケーションサービスの単体テストです。
/// </summary>
public class CatalogApplicationServiceTest(ITestOutputHelper testOutputHelper) : TestBase(testOutputHelper)
{
    private static CancellationToken AnyToken => It.IsAny<CancellationToken>();

    [Fact]
    public async Task GetCatalogItemsAsync_リポジトリのFindAsyncを1回呼出す()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var catalogBrandRepository = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepository = Mock.Of<ICatalogCategoryRepository>();
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepository, catalogCategoryRepository, logger);
        const int skip = 1;
        const int take = 10;

        // Act
        _ = await service.GetCatalogItemsAsync(skip, take, 1, 1);

        // Assert
        catalogRepositoryMock.Verify(
            r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), skip, take, AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_リポジトリのCountAsyncを1回呼出す()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var catalogBrandRepository = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepository = Mock.Of<ICatalogCategoryRepository>();
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepository, catalogCategoryRepository, logger);

        // Act
        _ = await service.GetCatalogItemsAsync(0, 10, 1, 1);

        // Assert
        catalogRepositoryMock.Verify(
            r => r.CountAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task GetBrandsAsync_ブランドリポジトリのGetAllAsyncを1回呼出す()
    {
        // Arrange
        var catalogRepository = Mock.Of<ICatalogRepository>();
        var catalogBrandRepositoryMock = new Mock<ICatalogBrandRepository>();
        var catalogCategoryRepository = Mock.Of<ICatalogCategoryRepository>();
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepository, catalogBrandRepositoryMock.Object, catalogCategoryRepository, logger);

        // Act
        _ = await service.GetBrandsAsync();

        // Assert
        catalogBrandRepositoryMock.Verify(r => r.GetAllAsync(AnyToken), Times.Once);
    }

    [Fact]
    public async Task GetCategoriesAsync_カテゴリリポジトリのGetAllAsyncを1回呼出す()
    {
        // Arrange
        var catalogRepository = Mock.Of<ICatalogRepository>();
        var catalogBrandRepository = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = new Mock<ICatalogCategoryRepository>();
        var logger = this.CreateTestLogger<CatalogApplicationService>();

        var service = new CatalogApplicationService(catalogRepository, catalogBrandRepository, catalogCategoryRepositoryMock.Object, logger);

        // Act
        _ = await service.GetCategoriesAsync();

        // Assert
        catalogCategoryRepositoryMock.Verify(r => r.GetAllAsync(AnyToken), Times.Once);
    }
}
