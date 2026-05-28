using System.Linq.Expressions;
using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.Authorization;
using Dressca.ApplicationCore.Catalog;

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
        var catalogDomainServiceMock = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepository, catalogCategoryRepository, userStore, catalogDomainServiceMock, logger);
        var skip = 1;
        var take = 10;
        var targetBrandId = new Guid("01971a00-0000-7000-b000-000000000001");
        var targetCategoryId = new Guid("01971a00-0000-7000-c000-000000000001");
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        _ = await service.GetCatalogItemsAsync(skip, take, targetBrandId, targetCategoryId, cancellationToken);

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
        var catalogDomainServiceMock = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepository, catalogCategoryRepository, userStore, catalogDomainServiceMock, logger);
        var skip = 1;
        var take = 10;
        var targetBrandId = new Guid("01971a00-0000-7000-b000-000000000001");
        var targetCategoryId = new Guid("01971a00-0000-7000-c000-000000000001");
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        _ = await service.GetCatalogItemsAsync(skip, take, targetBrandId, targetCategoryId, cancellationToken);

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
        var catalogDomainServiceMock = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepository, catalogBrandRepositoryMock.Object, catalogCategoryRepository, userStore, catalogDomainServiceMock, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        _ = await service.GetBrandsAsync(cancellationToken);

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
        var catalogDomainServiceMock = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<CatalogApplicationService>();

        var service = new CatalogApplicationService(catalogRepository, catalogBrandRepository, catalogCategoryRepositoryMock.Object, userStore, catalogDomainServiceMock, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        _ = await service.GetCategoriesAsync(cancellationToken);

        // Assert
        catalogCategoryRepositoryMock.Verify(r => r.GetAllAsync(AnyToken), Times.Once);
    }

    [Fact]
    public async Task AddItemToCatalogAsync_リポジトリのAddAsyncを一度だけ呼び出す()
    {
        // Arrange
        var targetBrandId = new Guid("01971a00-0000-7000-b000-000000000001");
        var targetCategoryId = new Guid("01971a00-0000-7000-c000-000000000001");
        var targetName = "テストアイテム";
        var targetDescription = "テスト用のアイテムです。";
        var targetPrice = 123456;
        var targetProductCode = "TEST001";

        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        catalogRepositoryMock
            .Setup(r => r.AddAsync(AnyItem, AnyToken)).Returns(Task.FromResult(CreateTestItem()));
        var catalogBrandRepositoryMock = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);
        var catalogDomainServiceMock = new Mock<ICatalogDomainService>();
        catalogDomainServiceMock.Setup(s => s.BrandExistsAsync(targetBrandId, AnyToken)).ReturnsAsync(true);
        catalogDomainServiceMock.Setup(s => s.CategoryExistsAsync(targetCategoryId, AnyToken)).ReturnsAsync(true);

        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepositoryMock, catalogCategoryRepositoryMock, userStoreMock.Object, catalogDomainServiceMock.Object, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Assert
        await service.AddItemToCatalogAsync(
            targetName,
            targetDescription,
            targetPrice,
            targetProductCode,
            targetBrandId,
            targetCategoryId,
            cancellationToken);

        // Act
        catalogRepositoryMock.Verify(r => r.AddAsync(It.IsAny<CatalogItem>(), AnyToken), Times.Once);
    }

    [Fact]
    public async Task AddItemToCatalogAsync_追加したアイテムの情報が返却される()
    {
        // Arrange
        var targetBrandId = new Guid("01971a00-0000-7000-b000-000000000001");
        var targetCategoryId = new Guid("01971a00-0000-7000-c000-000000000001");
        var targetName = "テストアイテム";
        var targetDescription = "テスト用のアイテムです。";
        var targetPrice = 123456;
        var targetProductCode = "TEST001";
        var targetItem = new CatalogItem
        {
            Name = targetName,
            Description = targetDescription,
            Price = targetPrice,
            ProductCode = targetProductCode,
            CatalogBrandId = targetBrandId,
            CatalogCategoryId = targetCategoryId,
        };

        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        catalogRepositoryMock
            .Setup(r => r.AddAsync(AnyItem, AnyToken)).Returns(Task.FromResult(targetItem));
        var catalogBrandRepositoryMock = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);
        var catalogDomainServiceMock = new Mock<ICatalogDomainService>();
        catalogDomainServiceMock.Setup(s => s.BrandExistsAsync(targetBrandId, AnyToken)).ReturnsAsync(true);
        catalogDomainServiceMock.Setup(s => s.CategoryExistsAsync(targetCategoryId, AnyToken)).ReturnsAsync(true);

        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepositoryMock, catalogCategoryRepositoryMock, userStoreMock.Object, catalogDomainServiceMock.Object, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Assert
        var actual = await service.AddItemToCatalogAsync(
            targetName,
            targetDescription,
            targetPrice,
            targetProductCode,
            targetBrandId,
            targetCategoryId,
            cancellationToken);

        // Act
        Assert.Same(targetItem, actual);
    }

    [Fact]
    public async Task AddItemToCatalogAsync_権限なし_PermissionDeniedExceptionが発生()
    {
        // Arrange
        var targetBrandId = new Guid("01971a00-0000-7000-b000-000000000001");
        var targetCategoryId = new Guid("01971a00-0000-7000-c000-000000000001");
        var targetName = "テストアイテム";
        var targetDescription = "テスト用のアイテムです。";
        var targetPrice = 123456;
        var targetProductCode = "TEST001";

        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        catalogRepositoryMock
            .Setup(r => r.AddAsync(AnyItem, AnyToken)).Returns(Task.FromResult(CreateTestItem()));
        var catalogBrandRepositoryMock = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        var catalogDomainServiceMock = Mock.Of<ICatalogDomainService>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(false);

        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepositoryMock, catalogCategoryRepositoryMock, userStoreMock.Object, catalogDomainServiceMock, logger);

        // Assert
        var action = () => service.AddItemToCatalogAsync(
            targetName,
            targetDescription,
            targetPrice,
            targetProductCode,
            targetBrandId,
            targetCategoryId);

        // Act
        await Assert.ThrowsAsync<PermissionDeniedException>(action);
    }

    [Fact]
    public async Task AddItemToCatalogAsync_存在しないブランドを指定_CatalogBrandNotExistingInRepositoryExceptionが発生()
    {
        // Arrange
        var targetBrandId = Guid.CreateVersion7();
        var targetCategoryId = new Guid("01971a00-0000-7000-c000-000000000001");
        var targetName = "テストアイテム";
        var targetDescription = "テスト用のアイテムです。";
        var targetPrice = 123456;
        var targetProductCode = "TEST001";

        var catalogRepositoryMock = Mock.Of<ICatalogRepository>();
        var catalogBrandRepositoryMock = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);
        var catalogDomainServiceMock = new Mock<ICatalogDomainService>();
        catalogDomainServiceMock.Setup(s => s.BrandExistsAsync(targetBrandId, AnyToken)).ReturnsAsync(false);
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock, catalogBrandRepositoryMock, catalogCategoryRepositoryMock, userStoreMock.Object, catalogDomainServiceMock.Object, logger);

        // Assert
        var action = () => service.AddItemToCatalogAsync(
            targetName,
            targetDescription,
            targetPrice,
            targetProductCode,
            targetBrandId,
            targetCategoryId);

        // Act
        await Assert.ThrowsAsync<CatalogBrandNotExistingInRepositoryException>(action);
    }

    [Fact]
    public async Task AddItemToCatalogAsync_存在しないカテゴリを指定_CatalogCategoryNotExistingInRepositoryExceptionが発生()
    {
        // Arrange
        var targetBrandId = new Guid("01971a00-0000-7000-b000-000000000001");
        var targetCategoryId = Guid.CreateVersion7();
        var targetName = "テストアイテム";
        var targetDescription = "テスト用のアイテムです。";
        var targetPrice = 123456;
        var targetProductCode = "TEST001";

        var catalogRepositoryMock = Mock.Of<ICatalogRepository>();
        var catalogBrandRepositoryMock = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);
        var catalogDomainServiceMock = new Mock<ICatalogDomainService>();
        catalogDomainServiceMock.Setup(s => s.BrandExistsAsync(targetBrandId, AnyToken)).ReturnsAsync(true);
        catalogDomainServiceMock.Setup(s => s.CategoryExistsAsync(targetCategoryId, AnyToken)).ReturnsAsync(false);
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock, catalogBrandRepositoryMock, catalogCategoryRepositoryMock, userStoreMock.Object, catalogDomainServiceMock.Object, logger);

        // Assert
        var action = () => service.AddItemToCatalogAsync(
            targetName,
            targetDescription,
            targetPrice,
            targetProductCode,
            targetBrandId,
            targetCategoryId);

        // Act
        await Assert.ThrowsAsync<CatalogCategoryNotExistingInRepositoryException>(action);
    }

    [Fact]
    public async Task DeleteItemFromCatalogAsync_対象のアイテムが存在_リポジトリのRemoveAsyncを1度だけ呼び出す()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var targetId = new Guid("01971a00-0000-7000-d000-000000000001");
        byte[] targetRowVersion = [255];
        var targetItem = CreateDefaultCatalog().Where(item => item.Id == targetId).ToList().FirstOrDefault();
        catalogRepositoryMock
            .Setup(r => r.GetAsync(targetId, AnyToken))
            .ReturnsAsync(targetItem);
        catalogRepositoryMock
            .Setup(r => r.RemoveAsync(targetId, targetRowVersion, AnyToken))
            .ReturnsAsync(1);
        var catalogBrandRepositoryMock = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        var catalogDomainServiceMock = new Mock<ICatalogDomainService>();
        catalogDomainServiceMock.Setup(s => s.ItemExistsAsync(targetId, AnyToken)).ReturnsAsync(true);
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepositoryMock, catalogCategoryRepositoryMock, userStoreMock.Object, catalogDomainServiceMock.Object, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        await service.DeleteItemFromCatalogAsync(targetId, targetRowVersion, cancellationToken);

        // Assert
        catalogRepositoryMock.Verify(r => r.RemoveAsync(targetId, targetRowVersion, AnyToken), Times.Once);
    }

    [Fact]
    public async Task DeleteItemFromCatalogAsync_対象のアイテムが存在しない_CatalogItemNotExistingInRepositoryExceptionが発生()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var targetId = Guid.CreateVersion7();
        var targetItem = CreateDefaultCatalog().Where(item => item.Id == targetId).ToList().FirstOrDefault();
        catalogRepositoryMock
            .Setup(r => r.GetAsync(targetId, AnyToken))
            .ReturnsAsync(targetItem);
        byte[] targetRowVersion = [255];
        var catalogBrandRepositoryMock = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = Mock.Of<ICatalogCategoryRepository>();
        var catalogDomainServiceMock = Mock.Of<ICatalogDomainService>();
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);

        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepositoryMock, catalogCategoryRepositoryMock, userStoreMock.Object, catalogDomainServiceMock, logger);

        // Act
        var action = () => service.DeleteItemFromCatalogAsync(targetId, targetRowVersion);

        // Assert
        await Assert.ThrowsAsync<CatalogItemNotExistingInRepositoryException>(action);
    }

    [Fact]
    public async Task DeleteItemFromCatalogAsync_権限なし_PermissionDeniedExceptionが発生()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var targetId = Guid.CreateVersion7();
        byte[] targetRowVersion = [255];
        var targetItem = CreateDefaultCatalog().Where(item => item.Id == targetId).ToList().FirstOrDefault();
        catalogRepositoryMock
            .Setup(r => r.GetAsync(targetId, AnyToken))
            .ReturnsAsync(targetItem);
        var catalogBrandRepositoryMock = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        var catalogDomainServiceMock = Mock.Of<ICatalogDomainService>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(false);

        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepositoryMock, catalogCategoryRepositoryMock, userStoreMock.Object, catalogDomainServiceMock, logger);

        // Act
        var action = () => service.DeleteItemFromCatalogAsync(targetId, targetRowVersion);

        // Assert
        await Assert.ThrowsAsync<PermissionDeniedException>(action);
    }

    [Fact]
    public async Task UpdateCatalogItemAsync_対象のアイテムが存在_リポジトリのUpdateAsyncを1度だけ呼び出す()
    {
        // Arrange
        var targetId = new Guid("01971a00-0000-7000-d000-000000000001");
        var targetName = "テストアイテム";
        var targetDescription = "テスト用のアイテムです。";
        var targetPrice = 123456;
        var targetProductCode = "TEST001";
        var targetItem = CreateDefaultCatalog().Where(item => item.Id == targetId).ToList().FirstOrDefault();
        var targetBrandId = new Guid("01971a00-0000-7000-b000-000000000001");
        var targetBrand = CreateDefaultBrands().Where(brand => brand.Id == targetBrandId).ToList().FirstOrDefault();
        var targetCategoryId = new Guid("01971a00-0000-7000-c000-000000000001");
        var targetCategory = CreateDefaultCategories().Where(category => category.Id == targetCategoryId).ToList().FirstOrDefault();

        var catalogRepositoryMock = new Mock<ICatalogRepository>();
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
        var catalogDomainServiceMock = new Mock<ICatalogDomainService>();
        catalogDomainServiceMock.Setup(s => s.ItemExistsAsync(targetId, AnyToken)).ReturnsAsync(true);
        catalogDomainServiceMock.Setup(s => s.BrandExistsAsync(targetBrandId, AnyToken)).ReturnsAsync(true);
        catalogDomainServiceMock.Setup(s => s.CategoryExistsAsync(targetCategoryId, AnyToken)).ReturnsAsync(true);
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepositoryMock.Object, catalogCategoryRepositoryMock.Object, userStoreMock.Object, catalogDomainServiceMock.Object, logger);
        var cancellationToken = TestContext.Current.CancellationToken;
        var isDeleted = false;

        // Act
        await service.UpdateCatalogItemAsync(
            targetId,
            targetName,
            targetDescription,
            targetPrice,
            targetProductCode,
            targetBrandId,
            targetCategoryId,
            AnyRowVersion,
            isDeleted,
            cancellationToken);

        // Assert
        catalogRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<CatalogItem>(), AnyToken), Times.Once);
    }

    [Fact]
    public async Task UpdateCatalogItemAsync_対象のアイテムが存在しない_CatalogItemNotExistingInRepositoryExceptionが発生()
    {
        // Arrange
        var targetId = Guid.CreateVersion7();
        var targetName = "テストアイテム";
        var targetDescription = "テスト用のアイテムです。";
        var targetPrice = 123456;
        var targetProductCode = "TEST001";
        var targetItem = CreateDefaultCatalog().Where(item => item.Id == targetId).ToList().FirstOrDefault();
        var targetBrandId = new Guid("01971a00-0000-7000-b000-000000000001");
        var targetBrand = CreateDefaultBrands().Where(brand => brand.Id == targetBrandId).ToList().FirstOrDefault();
        var targetCategoryId = new Guid("01971a00-0000-7000-c000-000000000001");
        var targetCategory = CreateDefaultCategories().Where(category => category.Id == targetCategoryId).ToList().FirstOrDefault();

        var catalogRepositoryMock = new Mock<ICatalogRepository>();
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
        var catalogDomainServiceMock = new Mock<ICatalogDomainService>();
        catalogDomainServiceMock.Setup(s => s.ItemExistsAsync(targetId, AnyToken)).ReturnsAsync(false);
        catalogDomainServiceMock.Setup(s => s.BrandExistsAsync(targetBrandId, AnyToken)).ReturnsAsync(true);
        catalogDomainServiceMock.Setup(s => s.CategoryExistsAsync(targetCategoryId, AnyToken)).ReturnsAsync(true);
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepositoryMock.Object, catalogCategoryRepositoryMock.Object, userStoreMock.Object, catalogDomainServiceMock.Object, logger);
        var isDeleted = false;

        // Act
        var action = () => service.UpdateCatalogItemAsync(
            targetId,
            targetName,
            targetDescription,
            targetPrice,
            targetProductCode,
            targetBrandId,
            targetCategoryId,
            AnyRowVersion,
            isDeleted);

        // Assert
        await Assert.ThrowsAsync<CatalogItemNotExistingInRepositoryException>(action);
    }

    [Fact]
    public async Task UpdateCatalogItemAsync_対象のブランドが存在しない_CatalogBrandNotExistingInRepositoryExceptionが発生()
    {
        // Arrange
        var targetId = new Guid("01971a00-0000-7000-d000-000000000001");
        var targetName = "テストアイテム";
        var targetDescription = "テスト用のアイテムです。";
        var targetPrice = 123456;
        var targetProductCode = "TEST001";
        var targetItem = CreateDefaultCatalog().Where(item => item.Id == targetId).ToList().FirstOrDefault();
        var targetBrandId = Guid.CreateVersion7();
        var targetBrand = CreateDefaultBrands().Where(brand => brand.Id == targetBrandId).ToList().FirstOrDefault();
        var targetCategoryId = new Guid("01971a00-0000-7000-c000-000000000001");
        var targetCategory = CreateDefaultCategories().Where(category => category.Id == targetCategoryId).ToList().FirstOrDefault();

        var catalogRepositoryMock = new Mock<ICatalogRepository>();
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
        var catalogDomainServiceMock = new Mock<ICatalogDomainService>();
        catalogDomainServiceMock.Setup(s => s.ItemExistsAsync(targetId, AnyToken)).ReturnsAsync(true);
        catalogDomainServiceMock.Setup(s => s.BrandExistsAsync(targetBrandId, AnyToken)).ReturnsAsync(false);
        catalogDomainServiceMock.Setup(s => s.CategoryExistsAsync(targetCategoryId, AnyToken)).ReturnsAsync(true);
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepositoryMock.Object, catalogCategoryRepositoryMock.Object, userStoreMock.Object, catalogDomainServiceMock.Object, logger);
        var isDeleted = false;

        // Act
        var action = () => service.UpdateCatalogItemAsync(
            targetId,
            targetName,
            targetDescription,
            targetPrice,
            targetProductCode,
            targetBrandId,
            targetCategoryId,
            AnyRowVersion,
            isDeleted);

        // Assert
        await Assert.ThrowsAsync<CatalogBrandNotExistingInRepositoryException>(action);
    }

    [Fact]
    public async Task UpdateCatalogItemAsync_対象のカテゴリが存在しない_CatalogCategoryNotExistingInRepositoryExceptionが発生()
    {
        // Arrange
        var targetId = new Guid("01971a00-0000-7000-d000-000000000001");
        var targetName = "テストアイテム";
        var targetDescription = "テスト用のアイテムです。";
        var targetPrice = 123456;
        var targetProductCode = "TEST001";
        var targetItem = CreateDefaultCatalog().Where(item => item.Id == targetId).ToList().FirstOrDefault();
        var targetBrandId = new Guid("01971a00-0000-7000-b000-000000000001");
        var targetBrand = CreateDefaultBrands().Where(brand => brand.Id == targetBrandId).ToList().FirstOrDefault();
        var targetCategoryId = Guid.CreateVersion7();
        var targetCategory = CreateDefaultCategories().Where(category => category.Id == targetCategoryId).ToList().FirstOrDefault();

        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var catalogBrandRepositoryMock = new Mock<ICatalogBrandRepository>();
        catalogBrandRepositoryMock
            .Setup(r => r.GetAsync(targetBrandId, AnyToken))
            .ReturnsAsync(targetBrand);
        var catalogCategoryRepositoryMock = new Mock<ICatalogCategoryRepository>();
        catalogCategoryRepositoryMock
            .Setup(r => r.GetAsync(targetCategoryId, AnyToken))
            .ReturnsAsync(targetCategory);
        var userStoreMock = new Mock<IUserStore>();
        var catalogDomainServiceMock = new Mock<ICatalogDomainService>();
        catalogDomainServiceMock.Setup(s => s.ItemExistsAsync(targetId, AnyToken)).ReturnsAsync(true);
        catalogDomainServiceMock.Setup(s => s.BrandExistsAsync(targetBrandId, AnyToken)).ReturnsAsync(true);
        catalogDomainServiceMock.Setup(s => s.CategoryExistsAsync(targetCategoryId, AnyToken)).ReturnsAsync(false);
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepositoryMock.Object, catalogCategoryRepositoryMock.Object, userStoreMock.Object, catalogDomainServiceMock.Object, logger);
        var isDeleted = false;

        // Act
        var action = () => service.UpdateCatalogItemAsync(
            targetId,
            targetName,
            targetDescription,
            targetPrice,
            targetProductCode,
            targetBrandId,
            targetCategoryId,
            AnyRowVersion,
            isDeleted);

        // Assert
        await Assert.ThrowsAsync<CatalogCategoryNotExistingInRepositoryException>(action);
    }

    [Fact]
    public async Task UpdateCatalogItemAsync_権限なし_PermissionDeniedExceptionが発生()
    {
        // Arrange
        var targetId = new Guid("01971a00-0000-7000-d000-000000000001");
        var targetName = "テストアイテム";
        var targetDescription = "テスト用のアイテムです。";
        var targetPrice = 123456;
        var targetProductCode = "TEST001";
        var targetItem = CreateDefaultCatalog().Where(item => item.Id == targetId).ToList().FirstOrDefault();
        var targetBrandId = new Guid("01971a00-0000-7000-b000-000000000001");
        var targetBrand = CreateDefaultBrands().Where(brand => brand.Id == targetBrandId).ToList().FirstOrDefault();
        var targetCategoryId = Guid.CreateVersion7();
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
        var catalogDomainServiceMock = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepositoryMock.Object, catalogCategoryRepositoryMock.Object, userStoreMock.Object, catalogDomainServiceMock, logger);
        var isDeleted = false;

        // Act
        var action = () => service.UpdateCatalogItemAsync(
            targetId,
            targetName,
            targetDescription,
            targetPrice,
            targetProductCode,
            targetBrandId,
            targetCategoryId,
            AnyRowVersion,
            isDeleted);

        // Assert
        await Assert.ThrowsAsync<PermissionDeniedException>(action);
    }

    [Fact]
    public async Task GetCatalogItemsForAdminAsync_リポジトリのFindAsyncを一度だけ呼び出す()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var catalogBrandRepository = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepository = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);
        var catalogDomainServiceMock = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepository, catalogCategoryRepository, userStoreMock.Object, catalogDomainServiceMock, logger);
        var skip = 0;
        var take = 10;
        var targetBrandId = new Guid("01971a00-0000-7000-b000-000000000001");
        var targetCategoryId = new Guid("01971a00-0000-7000-c000-000000000001");
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        _ = await service.GetCatalogItemsForAdminAsync(skip, take, targetBrandId, targetCategoryId, cancellationToken);

        // Assert
        catalogRepositoryMock.Verify(
            r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), skip, take, AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task GetCatalogItemsForAdminAsync_リポジトリのCountAsyncを1回呼出す()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var catalogBrandRepository = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepository = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);
        var catalogDomainServiceMock = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepository, catalogCategoryRepository, userStoreMock.Object, catalogDomainServiceMock, logger);
        var skip = 0;
        var take = 10;
        var targetBrandId = new Guid("01971a00-0000-7000-b000-000000000001");
        var targetCategoryId = new Guid("01971a00-0000-7000-c000-000000000001");
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        _ = await service.GetCatalogItemsForAdminAsync(skip, take, targetBrandId, targetCategoryId, cancellationToken);

        // Assert
        catalogRepositoryMock.Verify(
            r => r.CountAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken),
            Times.Once);
    }

    [Fact]
    public async Task GetCatalogItemsForAdminAsync_ページネーションされたアイテムと総アイテム数が返却される()
    {
        // Arrange
        var skip = 0;
        var take = 10;
        Guid? targetBrandId = new Guid("01971a00-0000-7000-b000-000000000001");
        Guid? targetCategoryId = new Guid("01971a00-0000-7000-c000-000000000001");
        var targetItems = CreateDefaultCatalog().Where(
            item => item.CatalogBrandId == targetBrandId && item.CatalogCategoryId == targetCategoryId).ToList();
        var targetTotalItems = targetItems.Count;

        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        catalogRepositoryMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), skip, take, AnyToken)).ReturnsAsync(targetItems);
        catalogRepositoryMock.Setup(r => r.CountAsync(It.IsAny<Expression<Func<CatalogItem, bool>>>(), AnyToken)).ReturnsAsync(targetTotalItems);
        var catalogBrandRepositoryMock = new Mock<ICatalogBrandRepository>();
        var catalogCategoryRepositoryMock = new Mock<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);
        var catalogDomainServiceMock = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepositoryMock.Object, catalogCategoryRepositoryMock.Object, userStoreMock.Object, catalogDomainServiceMock, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var (list, totalItems) = await service.GetCatalogItemsForAdminAsync(skip, take, targetBrandId, targetCategoryId, cancellationToken);

        // Assert
        Assert.Equal(targetItems, list);
        Assert.Equal(targetTotalItems, totalItems);
    }

    [Fact]
    public async Task GetCatalogItemsForAdminAsync_権限なし_PermissionDeniedExceptionが発生()
    {
        // Arrange
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        var catalogBrandRepository = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepository = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(false);
        var catalogDomainServiceMock = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepository, catalogCategoryRepository, userStoreMock.Object, catalogDomainServiceMock, logger);
        var skip = 0;
        var take = 10;
        var targetBrandId = new Guid("01971a00-0000-7000-b000-000000000001");
        var targetCategoryId = new Guid("01971a00-0000-7000-c000-000000000001");

        // Act
        var action = () => service.GetCatalogItemsForAdminAsync(skip, take, targetBrandId, targetCategoryId);

        // Assert
        await Assert.ThrowsAsync<PermissionDeniedException>(action);
    }

    [Fact]
    public async Task GetCatalogItemForAdminAsync_リポジトリのGetAsyncを一度だけ呼び出す()
    {
        // Arrange
        var targetId = new Guid("01971a00-0000-7000-d000-000000000001");
        var targetItem = CreateDefaultCatalog().Where(item => item.Id == targetId).ToList().FirstOrDefault();
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        catalogRepositoryMock
            .Setup(r => r.GetAsync(targetId, AnyToken))
            .ReturnsAsync(targetItem);
        var catalogBrandRepository = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepository = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        var catalogDomainServiceMock = Mock.Of<ICatalogDomainService>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepository, catalogCategoryRepository, userStoreMock.Object, catalogDomainServiceMock, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        _ = await service.GetCatalogItemForAdminAsync(targetId, cancellationToken);

        // Assert
        catalogRepositoryMock.Verify(r => r.GetAsync(targetId, AnyToken), Times.Once);
    }

    [Fact]
    public async Task GetCatalogItemForAdminAsync_権限なし_PermissionDeniedExceptionが発生()
    {
        // Arrange
        var targetId = new Guid("01971a00-0000-7000-d000-000000000001");
        var targetItem = CreateDefaultCatalog().Where(item => item.Id == targetId).ToList().FirstOrDefault();
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        catalogRepositoryMock
            .Setup(r => r.GetAsync(targetId, AnyToken))
            .ReturnsAsync(targetItem);
        var catalogBrandRepository = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepository = Mock.Of<ICatalogCategoryRepository>();
        var catalogDomainServiceMock = Mock.Of<ICatalogDomainService>();
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(false);
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepository, catalogCategoryRepository, userStoreMock.Object, catalogDomainServiceMock, logger);

        // Act
        var action = () => service.GetCatalogItemForAdminAsync(targetId);

        // Assert
        await Assert.ThrowsAsync<PermissionDeniedException>(action);
    }

    [Fact]
    public async Task GetCatalogItemForAdminAsync_対象のアイテムが存在しない_CatalogItemNotExistingInRepositoryExceptionが発生()
    {
        // Arrange
        var targetId = Guid.CreateVersion7();
        var catalogRepositoryMock = new Mock<ICatalogRepository>();
        catalogRepositoryMock
            .Setup(r => r.GetAsync(targetId, AnyToken))
            .Returns(Task.FromResult<CatalogItem?>(null));
        var catalogBrandRepository = Mock.Of<ICatalogBrandRepository>();
        var catalogCategoryRepository = Mock.Of<ICatalogCategoryRepository>();
        var userStoreMock = new Mock<IUserStore>();
        userStoreMock.Setup(a => a.IsInRole(It.IsAny<string>())).Returns(true);
        var catalogDomainServiceMock = Mock.Of<ICatalogDomainService>();
        var logger = this.CreateTestLogger<CatalogApplicationService>();
        var service = new CatalogApplicationService(catalogRepositoryMock.Object, catalogBrandRepository, catalogCategoryRepository, userStoreMock.Object, catalogDomainServiceMock, logger);

        // Act
        var action = () => service.GetCatalogItemForAdminAsync(targetId);

        // Assert
        await Assert.ThrowsAsync<CatalogItemNotExistingInRepositoryException>(action);
    }

    private static CatalogItem CreateTestItem()
    {
        var category1 = new Guid("01971a00-0000-7000-c000-000000000001");
        var brand1 = new Guid("01971a00-0000-7000-b000-000000000001");

        return new() { CatalogCategoryId = category1, CatalogBrandId = brand1, Description = "テスト用アイテムです。", Name = "テスト用アイテム", Price = 23800m, ProductCode = "TEST001", Id = Guid.CreateVersion7() };
    }

    private static List<CatalogItem> CreateDefaultCatalog()
    {
        var category1 = new Guid("01971a00-0000-7000-c000-000000000001");
        var category2 = new Guid("01971a00-0000-7000-c000-000000000002");
        var category3 = new Guid("01971a00-0000-7000-c000-000000000003");
        var brand1 = new Guid("01971a00-0000-7000-b000-000000000001");
        var brand2 = new Guid("01971a00-0000-7000-b000-000000000002");
        var brand3 = new Guid("01971a00-0000-7000-b000-000000000003");
        var item1 = new Guid("01971a00-0000-7000-d000-000000000001");
        var item2 = new Guid("01971a00-0000-7000-d000-000000000002");
        var item3 = new Guid("01971a00-0000-7000-d000-000000000003");
        var item4 = new Guid("01971a00-0000-7000-d000-000000000004");
        var item5 = new Guid("01971a00-0000-7000-d000-000000000005");
        var item6 = new Guid("01971a00-0000-7000-d000-000000000006");
        var item7 = new Guid("01971a00-0000-7000-d000-000000000007");
        var item8 = new Guid("01971a00-0000-7000-d000-000000000008");
        var item9 = new Guid("01971a00-0000-7000-d000-000000000009");
        var item10 = new Guid("01971a00-0000-7000-d000-00000000000a");
        var item11 = new Guid("01971a00-0000-7000-d000-00000000000b");
        var catalog = new List<CatalogItem>()
        {
            new() { CatalogCategoryId = category1, CatalogBrandId = brand3, Description = "定番の無地ロングTシャツです。", Name = "クルーネック Tシャツ - ブラック", Price = 1980m, ProductCode = "C000000001", Id = item1, RowVersion = [255] },
            new() { CatalogCategoryId = category1, CatalogBrandId = brand2, Description = "暖かいのに着膨れしない起毛デニムです。", Name = "裏起毛 スキニーデニム", Price = 4800m, ProductCode = "C000000002", Id = item2, RowVersion = [255] },
            new() { CatalogCategoryId = category1, CatalogBrandId = brand1, Description = "あたたかく肌ざわりも良いウール100%のロングコートです。", Name = "ウールコート", Price = 49800m, ProductCode = "C000000003", Id = item3, RowVersion = [255] },
            new() { CatalogCategoryId = category1, CatalogBrandId = brand2, Description = "コットン100%の柔らかい着心地で、春先から夏、秋口まで万能に使いやすいです。", Name = "無地 ボタンダウンシャツ", Price = 2800m, ProductCode = "C000000004", Id = item4, RowVersion = [255] },
            new() { CatalogCategoryId = category2, CatalogBrandId = brand3, Description = "コンパクトサイズのバッグですが収納力は抜群です", Name = "レザーハンドバッグ", Price = 18800m, ProductCode = "B000000001", Id = item5, RowVersion = [255] },
            new() { CatalogCategoryId = category2, CatalogBrandId = brand2, Description = "エイジング加工したレザーを使用しています。", Name = "ショルダーバッグ", Price = 38000m, ProductCode = "B000000002", Id = item6, RowVersion = [255] },
            new() { CatalogCategoryId = category2, CatalogBrandId = brand3, Description = "春の季節にぴったりのトートバッグです。インナーポーチまたは単体でも使用可能なポーチ付。", Name = "トートバッグ ポーチ付き", Price = 24800m, ProductCode = "B000000003", Id = item7, RowVersion = [255] },
            new() { CatalogCategoryId = category2, CatalogBrandId = brand1, Description = "さらりと気軽に纏える、キュートなミニサイズショルダー。", Name = "ショルダーバッグ", Price = 2800m, ProductCode = "B000000004", Id = item8, RowVersion = [255] },
            new() { CatalogCategoryId = category2, CatalogBrandId = brand1, Description = "エレガントな雰囲気を放つキルティングデザインです。", Name = "レザー チェーンショルダーバッグ", Price = 258000m, ProductCode = "B000000005", Id = item9, RowVersion = [255] },
            new() { CatalogCategoryId = category3, CatalogBrandId = brand2, Description = "柔らかいソールは快適な履き心地で、ランニングに最適です。", Name = "ランニングシューズ - ブルー", Price = 12800m, ProductCode = "S000000001", Id = item10, RowVersion = [255] },
            new() { CatalogCategoryId = category3, CatalogBrandId = brand1, Description = "イタリアの職人が丁寧に手作業で作り上げた一品です。", Name = "メダリオン ストレートチップ ドレスシューズ", Price = 23800m, ProductCode = "S000000002", Id = item11, RowVersion = [255] },
        };
        return catalog;
    }

    private static List<CatalogBrand> CreateDefaultBrands()
    {
        var brand1 = new Guid("01971a00-0000-7000-b000-000000000001");
        var brand2 = new Guid("01971a00-0000-7000-b000-000000000002");
        var brand3 = new Guid("01971a00-0000-7000-b000-000000000003");
        var brands = new List<CatalogBrand>()
        {
            new() { Name = "高級なブランド", Id = brand1 },
            new() { Name = "カジュアルなブランド", Id = brand2 },
            new() { Name = "ノーブランド", Id = brand3 },
        };
        return brands;
    }

    private static List<CatalogCategory> CreateDefaultCategories()
    {
        var category1 = new Guid("01971a00-0000-7000-c000-000000000001");
        var category2 = new Guid("01971a00-0000-7000-c000-000000000002");
        var category3 = new Guid("01971a00-0000-7000-c000-000000000003");
        var categories = new List<CatalogCategory>()
        {
            new() { Name = "服", Id = category1 },
            new() { Name = "バッグ", Id = category2 },
            new() { Name = "シューズ", Id = category3 },
        };
        return categories;
    }
}
