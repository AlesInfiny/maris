using System.Linq.Expressions;
using Dressca.ApplicationCore.DisplayItems;

namespace Dressca.UnitTests.ApplicationCore.DisplayItems;

public class DisplayItemDomainServiceTest(ITestOutputHelper testOutputHelper) : TestBase(testOutputHelper)
{
    [Fact]
    public async Task ExistsAllAsync_削除済みと存在しないIDを除外する()
    {
        var available = CreateItem(false);
        var deleted = CreateItem(true);
        DisplayItem[] items = [available, deleted];
        var repository = new Mock<IDisplayItemRepository>();
        repository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<DisplayItem, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expression<Func<DisplayItem, bool>> predicate, CancellationToken _) => items.Where(predicate.Compile()).ToArray());
        var service = new DisplayItemDomainService(repository.Object, this.CreateTestLogger<DisplayItemDomainService>());
        var cancellationToken = TestContext.Current.CancellationToken;

        var result = await service.ExistsAllAsync([available.Id, deleted.Id, Guid.CreateVersion7()], cancellationToken);
        Assert.False(result.ExistsAll);
        Assert.Equal(available.Id, Assert.Single(result.DisplayItems).Id);
        Assert.True((await service.ExistsAllAsync([available.Id, available.Id], cancellationToken)).ExistsAll);
        var empty = await service.ExistsAllAsync([], cancellationToken);
        Assert.True(empty.ExistsAll);
        Assert.Empty(empty.DisplayItems);
    }

    private static DisplayItem CreateItem(bool deleted) => new()
    {
        Id = Guid.CreateVersion7(),
        Name = "商品", Description = "説明", ProductCode = "TEST1", Price = 1000m,
        DisplayItemBrandId = Guid.CreateVersion7(), DisplayItemCategoryId = Guid.CreateVersion7(), IsDeleted = deleted,
    };
}
