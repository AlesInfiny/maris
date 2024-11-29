using System.Linq.Expressions;
using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.Authorization;
using Dressca.ApplicationCore.Catalog;
using Xunit.Abstractions;

namespace Dressca.UnitTests.ApplicationCore.ApplicationService;

/// <summary>
///  カタログアプリケーションサービスの単体テストです。
/// </summary>
public class CatalogApplicationServiceTest(ITestOutputHelper testOutputHelper) : TestBase(testOutputHelper)
{
    private static CancellationToken AnyToken => It.IsAny<CancellationToken>();

    private static byte[] AnyRowVersion => It.IsAny<byte[]>();

    private static CatalogItem AnyItem => It.IsAny<CatalogItem>();

    [Fact]
    public async Task GetCatalogItemsAsync_リポジトリのFindAsyncを1回呼出す()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var catalogBrandRepository = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepository = Mock.Of<ICatalogCategoryRepository>();
        var userStore = Mock.Of<IUserStore>();
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepository, catalogCategoryRepository, userStore, logger);
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
        var userStore = Mock.Of<IUserStore>();
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepository, catalogCategoryRepository, userStore, logger);

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
        var userStore = Mock.Of<IUserStore>();
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepository, catalogBrandRepositoryMock.Object, catalogCategoryRepository, userStore, logger);

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
        var userStore = Mock.Of<IUserStore>();
        var logger = this.CreateTestLogger<CatalogApplicationService>();

        var service = new CatalogApplicationService(catalogRepository, catalogBrandRepository, catalogCategoryRepositoryMock.Object, userStore, logger);

        // Act
        _ = await service.GetCategoriesAsync();

        // Assert
        catalogCategoryRepositoryMock.Verify(r => r.GetAllAsync(AnyToken), Times.Once);
    }

    [Fact]
    public async Task AddItemToCatalogAsync_リポジトリのAddAsyncを一度だけ呼び出す()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        catalogRepositoryMock
            .Setup(r => r.AddAsync(AnyItem, AnyToken)).Returns(Task.FromResult(CreateTestItem()));
        var catalogBrandRepositoryMock = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);

        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepositoryMock, catalogCategoryRepositoryMock, userStoreMock.Object, logger);

        // Assert
        await service.AddItemToCatalogAsync(
            "テストアイテム",
            "テスト用のアイテムです。",
            123456,
            "TEST001",
            1,
            1);

        // Act
        catalogRepositoryMock.Verify(r => r.AddAsync(It.IsAny<CatalogItem>(), AnyToken), Times.Once);
    }

    [Fact]
    public async Task AddItemToCatalogAsync_権限なし_PermissionDeniedExceptionが発生()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        catalogRepositoryMock
            .Setup(r => r.AddAsync(AnyItem, AnyToken)).Returns(Task.FromResult(CreateTestItem()));
        var catalogBrandRepositoryMock = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(false);

        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepositoryMock, catalogCategoryRepositoryMock, userStoreMock.Object, logger);

        // Assert
        var action = () => service.AddItemToCatalogAsync(
            "テストアイテム",
            "テスト用のアイテムです。",
            123456,
            "TEST001",
            1,
            1);

        // Act
        await Assert.ThrowsAsync<PermissionDeniedException>(action);
    }

    [Fact]
    public async Task DeleteItemFromCatalogAsync_対象のアイテムが存在_リポジトリのRemoveAsyncを1度だけ呼び出す()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var targetId = 1;
        var targetItem = CreateDefaultCatalog().Where(item => item.Id == targetId).ToList().FirstOrDefault();
        catalogRepositoryMock
            .Setup(r => r.GetAsync(targetId, AnyToken))
            .ReturnsAsync(targetItem);
        var catalogBrandRepositoryMock = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);

        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepositoryMock, catalogCategoryRepositoryMock, userStoreMock.Object, logger);

        // Act
        await service.DeleteItemFromCatalogAsync(targetId);

        // Assert
        catalogRepositoryMock.Verify(r => r.RemoveAsync(It.IsAny<CatalogItem>(), AnyToken), Times.Once);
    }

    [Fact]
    public async Task DeleteItemFromCatalogAsync_対象のアイテムが存在しない_CatalogItemNotExistingInRepositoryExceptionが発生()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var targetId = 999;
        var targetItem = CreateDefaultCatalog().Where(item => item.Id == targetId).ToList().FirstOrDefault();
        catalogRepositoryMock
            .Setup(r => r.GetAsync(targetId, AnyToken))
            .ReturnsAsync(targetItem);
        var catalogBrandRepositoryMock = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);

        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepositoryMock, catalogCategoryRepositoryMock, userStoreMock.Object, logger);

        // Act
        var action = () => service.DeleteItemFromCatalogAsync(targetId);

        // Assert
        await Assert.ThrowsAsync<CatalogItemNotExistingInRepositoryException>(action);
    }

    [Fact]
    public async Task DeleteItemFromCatalogAsync_権限なし_PermissionDeniedExceptionが発生()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var targetId = 999;
        var targetItem = CreateDefaultCatalog().Where(item => item.Id == targetId).ToList().FirstOrDefault();
        catalogRepositoryMock
            .Setup(r => r.GetAsync(targetId, AnyToken))
            .ReturnsAsync(targetItem);
        var catalogBrandRepositoryMock = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(false);

        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepositoryMock, catalogCategoryRepositoryMock, userStoreMock.Object, logger);

        // Act
        var action = () => service.DeleteItemFromCatalogAsync(targetId);

        // Assert
        await Assert.ThrowsAsync<PermissionDeniedException>(action);
    }

    [Fact]
    public async Task UpdateCatalogItemAsync_対象のアイテムが存在_リポジトリのUpdateAsyncを1度だけ呼び出す()
    {
        // Arrange
        var targetId = 1;
        var targetItem = CreateDefaultCatalog().Where(item => item.Id == targetId).ToList().FirstOrDefault();
        var targetBrandId = 1;
        var targetBrand = CreateDefaultBrands().Where(brand => brand.Id == targetBrandId).ToList().FirstOrDefault();
        var targetCategoryId = 1;
        var targetCategory = CreateDefaultCategories().Where(category => category.Id == targetCategoryId).ToList().FirstOrDefault();

        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        catalogRepositoryMock
            .Setup(r => r.DoesEntityExistAsync(targetId, AnyToken))
            .ReturnsAsync(true);
        var catalogBrandRepositoryMock = new Mock<ICatalogBrandRepository>();
        catalogBrandRepositoryMock
            .Setup(r => r.GetAsync(targetBrandId, AnyToken))
            .ReturnsAsync(targetBrand);
        var catalogCategoryRepositoryMock = new Mock<ICatalogCategoryRepository>();
        catalogCategoryRepositoryMock
            .Setup(r => r.GetAsync(targetCategoryId, AnyToken))
            .ReturnsAsync(targetCategory);
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);

        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepositoryMock.Object, catalogCategoryRepositoryMock.Object, userStoreMock.Object, logger);

        // Act
        await service.UpdateCatalogItemAsync(
            targetId,
            "テストアイテム",
            "テスト用のアイテムです。",
            123456,
            "TEST001",
            targetBrandId,
            targetCategoryId,
            AnyRowVersion);

        // Assert
        catalogRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<CatalogItem>(), AnyToken), Times.Once);
    }

    [Fact]
    public async Task UpdateCatalogItemAsync_対象のアイテムが存在しない_CatalogItemNotExistingInRepositoryExceptionが発生()
    {
        // Arrange
        var targetId = 999;
        var targetItem = CreateDefaultCatalog().Where(item => item.Id == targetId).ToList().FirstOrDefault();
        var targetBrandId = 1;
        var targetBrand = CreateDefaultBrands().Where(brand => brand.Id == targetBrandId).ToList().FirstOrDefault();
        var targetCategoryId = 1;
        var targetCategory = CreateDefaultCategories().Where(category => category.Id == targetCategoryId).ToList().FirstOrDefault();

        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        catalogRepositoryMock
            .Setup(r => r.DoesEntityExistAsync(targetId, AnyToken))
            .ReturnsAsync(false);
        var catalogBrandRepositoryMock = new Mock<ICatalogBrandRepository>();
        catalogBrandRepositoryMock
            .Setup(r => r.GetAsync(targetBrandId, AnyToken))
            .ReturnsAsync(targetBrand);
        var catalogCategoryRepositoryMock = new Mock<ICatalogCategoryRepository>();
        catalogCategoryRepositoryMock
            .Setup(r => r.GetAsync(targetCategoryId, AnyToken))
            .ReturnsAsync(targetCategory);
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepositoryMock.Object, catalogCategoryRepositoryMock.Object, userStoreMock.Object, logger);

        // Act
        var action = () => service.UpdateCatalogItemAsync(
            targetId,
            "テストアイテム",
            "テスト用のアイテムです。",
            123456,
            "TEST001",
            targetBrandId,
            targetCategoryId,
            AnyRowVersion);

        // Assert
        await Assert.ThrowsAsync<CatalogItemNotExistingInRepositoryException>(action);
    }

    [Fact]
    public async Task UpdateCatalogItemAsync_対象のブランドが存在しない_CatalogBrandNotExistingInRepositoryExceptionが発生()
    {
        // Arrange
        var targetId = 1;
        var targetItem = CreateDefaultCatalog().Where(item => item.Id == targetId).ToList().FirstOrDefault();
        var targetBrandId = 999;
        var targetBrand = CreateDefaultBrands().Where(brand => brand.Id == targetBrandId).ToList().FirstOrDefault();
        var targetCategoryId = 1;
        var targetCategory = CreateDefaultCategories().Where(category => category.Id == targetCategoryId).ToList().FirstOrDefault();

        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        catalogRepositoryMock
            .Setup(r => r.DoesEntityExistAsync(targetId, AnyToken))
            .ReturnsAsync(true);
        var catalogBrandRepositoryMock = new Mock<ICatalogBrandRepository>();
        catalogBrandRepositoryMock
            .Setup(r => r.GetAsync(targetBrandId, AnyToken))
            .ReturnsAsync(targetBrand);
        var catalogCategoryRepositoryMock = new Mock<ICatalogCategoryRepository>();
        catalogCategoryRepositoryMock
            .Setup(r => r.GetAsync(targetCategoryId, AnyToken))
            .ReturnsAsync(targetCategory);
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepositoryMock.Object, catalogCategoryRepositoryMock.Object, userStoreMock.Object, logger);

        // Act
        var action = () => service.UpdateCatalogItemAsync(
            targetId,
            "テストアイテム",
            "テスト用のアイテムです。",
            123456,
            "TEST001",
            targetBrandId,
            targetCategoryId,
            AnyRowVersion);

        // Assert
        await Assert.ThrowsAsync<CatalogBrandNotExistingInRepositoryException>(action);
    }

    [Fact]
    public async Task UpdateCatalogItemAsync_対象のカテゴリが存在しない_CatalogCategoryNotExistingInRepositoryExceptionが発生()
    {
        // Arrange
        var targetId = 1;
        var targetItem = CreateDefaultCatalog().Where(item => item.Id == targetId).ToList().FirstOrDefault();
        var targetBrandId = 1;
        var targetBrand = CreateDefaultBrands().Where(brand => brand.Id == targetBrandId).ToList().FirstOrDefault();
        var targetCategoryId = 999;
        var targetCategory = CreateDefaultCategories().Where(category => category.Id == targetCategoryId).ToList().FirstOrDefault();

        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        catalogRepositoryMock
            .Setup(r => r.DoesEntityExistAsync(targetId, AnyToken))
            .ReturnsAsync(true);
        var catalogBrandRepositoryMock = new Mock<ICatalogBrandRepository>();
        catalogBrandRepositoryMock
            .Setup(r => r.GetAsync(targetBrandId, AnyToken))
            .ReturnsAsync(targetBrand);
        var catalogCategoryRepositoryMock = new Mock<ICatalogCategoryRepository>();
        catalogCategoryRepositoryMock
            .Setup(r => r.GetAsync(targetCategoryId, AnyToken))
            .ReturnsAsync(targetCategory);
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepositoryMock.Object, catalogCategoryRepositoryMock.Object, userStoreMock.Object, logger);

        // Act
        var action = () => service.UpdateCatalogItemAsync(
            targetId,
            "テストアイテム",
            "テスト用のアイテムです。",
            123456,
            "TEST001",
            targetBrandId,
            targetCategoryId,
            AnyRowVersion);

        // Assert
        await Assert.ThrowsAsync<CatalogCategoryNotExistingInRepositoryException>(action);
    }

    [Fact]
    public async Task UpdateCatalogItemAsync_権限なし_PermissionDeniedExceptionが発生()
    {
        // Arrange
        var targetId = 1;
        var targetItem = CreateDefaultCatalog().Where(item => item.Id == targetId).ToList().FirstOrDefault();
        var targetBrandId = 1;
        var targetBrand = CreateDefaultBrands().Where(brand => brand.Id == targetBrandId).ToList().FirstOrDefault();
        var targetCategoryId = 999;
        var targetCategory = CreateDefaultCategories().Where(category => category.Id == targetCategoryId).ToList().FirstOrDefault();

        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        catalogRepositoryMock
            .Setup(r => r.GetAsync(targetId, AnyToken))
            .ReturnsAsync(targetItem);
        var catalogBrandRepositoryMock = new Mock<ICatalogBrandRepository>();
        catalogBrandRepositoryMock
            .Setup(r => r.GetAsync(targetBrandId, AnyToken))
            .ReturnsAsync(targetBrand);
        var catalogCategoryRepositoryMock = new Mock<ICatalogCategoryRepository>();
        catalogCategoryRepositoryMock
            .Setup(r => r.GetAsync(targetCategoryId, AnyToken))
            .ReturnsAsync(targetCategory);
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(false);
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepositoryMock.Object, catalogCategoryRepositoryMock.Object, userStoreMock.Object, logger);

        // Act
        var action = () => service.UpdateCatalogItemAsync(
            targetId,
            "テストアイテム",
            "テスト用のアイテムです。",
            123456,
            "TEST001",
            targetBrandId,
            targetCategoryId,
            AnyRowVersion);

        // Assert
        await Assert.ThrowsAsync<PermissionDeniedException>(action);
    }

    [Fact]
    public async Task GetCatalogItemsByAdminAsync_リポジトリのFindAsyncを一度だけ呼び出す()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var catalogBrandRepository = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepository = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepository, catalogCategoryRepository, userStoreMock.Object, logger);
        const int skip = 1;
        const int take = 10;

        // Act
        _ = await service.GetCatalogItemsByAdminAsync(skip, take, 1, 1);

        // Assert
        catalogRepositoryMock.Verify(
            r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), skip, take, AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task GetCatalogItemsByAdminAsync_リポジトリのCountAsyncを1回呼出す()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var catalogBrandRepository = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepository = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepository, catalogCategoryRepository, userStoreMock.Object, logger);

        // Act
        _ = await service.GetCatalogItemsByAdminAsync(0, 10, 1, 1);

        // Assert
        catalogRepositoryMock.Verify(
            r => r.CountAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task GetCatalogItemsByAdminAsync_権限なし_PermissionDeniedExceptionが発生()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var catalogBrandRepository = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepository = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(false);
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepository, catalogCategoryRepository, userStoreMock.Object, logger);

        // Act
        var action = () => service.GetCatalogItemsByAdminAsync(0, 10, 1, 1);

        // Assert
        await Assert.ThrowsAsync<PermissionDeniedException>(action);
    }

    [Fact]
    public async Task GetCatalogItemByAdminAsync_リポジトリのGetAsyncを一度だけ呼び出す()
    {
        // Arrange
        var targetId = 1;
        var targetItem = CreateDefaultCatalog().Where(item => item.Id == targetId).ToList().FirstOrDefault();
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        catalogRepositoryMock
            .Setup(r => r.GetAsync(targetId, AnyToken))
            .ReturnsAsync(targetItem);
        var catalogBrandRepository = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepository = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepository, catalogCategoryRepository, userStoreMock.Object, logger);

        // Act
        _ = await service.GetCatalogItemByAdminAsync(targetId);

        // Assert
        catalogRepositoryMock.Verify(r => r.GetAsync(targetId, AnyToken), Times.Once);
    }

    [Fact]
    public async Task GetCatalogItemByAdminAsync_権限なし_PermissionDeniedExceptionが発生()
    {
        // Arrange
        var targetId = 1;
        var targetItem = CreateDefaultCatalog().Where(item => item.Id == targetId).ToList().FirstOrDefault();
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        catalogRepositoryMock
            .Setup(r => r.GetAsync(targetId, AnyToken))
            .ReturnsAsync(targetItem);
        var catalogBrandRepository = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepository = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(false);
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepository, catalogCategoryRepository, userStoreMock.Object, logger);

        // Act
        var action = () => service.GetCatalogItemByAdminAsync(targetId);

        // Assert
        await Assert.ThrowsAsync<PermissionDeniedException>(action);
    }

    [Fact]
    public async Task GetCatalogItemByAdminAsync_対象のアイテムが存在しない_CatalogItemNotExistingInRepositoryExceptionが発生()
    {
        // Arrange
        var targetId = 999;
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        catalogRepositoryMock
            .Setup(r => r.GetAsync(targetId, AnyToken))
            .Returns(Task.FromResult<CatalogItem?>(null));
        var catalogBrandRepository = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepository = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepository, catalogCategoryRepository, userStoreMock.Object, logger);

        // Act
        var action = () => service.GetCatalogItemByAdminAsync(targetId);

        // Assert
        await Assert.ThrowsAsync<CatalogItemNotExistingInRepositoryException>(action);
    }

    private static CatalogItem CreateTestItem()
    {
        return new() { CatalogCategoryId = 1L, CatalogBrandId = 1L, Description = "テスト用アイテムです。", Name = "テスト用アイテム", Price = 23800m, ProductCode = "TEST001", Id = 9999L };
    }

    private static List<CatalogItem> CreateDefaultCatalog()
    {
        var catalog = new List<CatalogItem>()
        {
            new() { CatalogCategoryId = 1L, CatalogBrandId = 3L, Description = "定番の無地ロングTシャツです。", Name = "クルーネック Tシャツ - ブラック", Price = 1980m, ProductCode = "C000000001", Id = 1L },
            new() { CatalogCategoryId = 1L, CatalogBrandId = 2L, Description = "暖かいのに着膨れしない起毛デニムです。", Name = "裏起毛 スキニーデニム", Price = 4800m, ProductCode = "C000000002", Id = 2L },
            new() { CatalogCategoryId = 1L, CatalogBrandId = 1L, Description = "あたたかく肌ざわりも良いウール100%のロングコートです。", Name = "ウールコート", Price = 49800m, ProductCode = "C000000003", Id = 3L },
            new() { CatalogCategoryId = 1L, CatalogBrandId = 2L, Description = "コットン100%の柔らかい着心地で、春先から夏、秋口まで万能に使いやすいです。", Name = "無地 ボタンダウンシャツ", Price = 2800m, ProductCode = "C000000004", Id = 4L },
            new() { CatalogCategoryId = 2L, CatalogBrandId = 3L, Description = "コンパクトサイズのバッグですが収納力は抜群です", Name = "レザーハンドバッグ", Price = 18800m, ProductCode = "B000000001", Id = 5L },
            new() { CatalogCategoryId = 2L, CatalogBrandId = 2L, Description = "エイジング加工したレザーを使用しています。", Name = "ショルダーバッグ", Price = 38000m, ProductCode = "B000000002", Id = 6L },
            new() { CatalogCategoryId = 2L, CatalogBrandId = 3L, Description = "春の季節にぴったりのトートバッグです。インナーポーチまたは単体でも使用可能なポーチ付。", Name = "トートバッグ ポーチ付き", Price = 24800m, ProductCode = "B000000003", Id = 7L },
            new() { CatalogCategoryId = 2L, CatalogBrandId = 1L, Description = "さらりと気軽に纏える、キュートなミニサイズショルダー。", Name = "ショルダーバッグ", Price = 2800m, ProductCode = "B000000004", Id = 8L },
            new() { CatalogCategoryId = 2L, CatalogBrandId = 1L, Description = "エレガントな雰囲気を放つキルティングデザインです。", Name = "レザー チェーンショルダーバッグ", Price = 258000m, ProductCode = "B000000005", Id = 9L },
            new() { CatalogCategoryId = 3L, CatalogBrandId = 2L, Description = "柔らかいソールは快適な履き心地で、ランニングに最適です。", Name = "ランニングシューズ - ブルー", Price = 12800m, ProductCode = "S000000001", Id = 10L },
            new() { CatalogCategoryId = 3L, CatalogBrandId = 1L, Description = "イタリアの職人が丁寧に手作業で作り上げた一品です。", Name = "メダリオン ストレートチップ ドレスシューズ", Price = 23800m, ProductCode = "S000000002", Id = 11L },
        };
        return catalog;
    }

    private static List<CatalogBrand> CreateDefaultBrands()
    {
        var brands = new List<CatalogBrand>()
        {
            new() { Name = "高級なブランド", Id = 1L },
            new() { Name = "カジュアルなブランド", Id = 2L },
            new() { Name = "ノーブランド", Id = 3L },
        };
        return brands;
    }

    private static List<CatalogCategory> CreateDefaultCategories()
    {
        var categories = new List<CatalogCategory>()
        {
            new() { Name = "服", Id = 1L },
            new() { Name = "バッグ", Id = 2L },
            new() { Name = "シューズ", Id = 3L },
        };
        return categories;
    }
}
