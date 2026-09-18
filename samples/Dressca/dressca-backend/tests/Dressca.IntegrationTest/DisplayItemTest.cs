using System.Net;
using System.Net.Http.Json;
using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.Catalog;
using Dressca.ApplicationCore.DisplayItems;
using Dressca.ApplicationCore.Ordering;
using Dressca.EfInfrastructure;
using Dressca.EfInfrastructure.Configurations;
using Dressca.Web.Consumer.Dto.Baskets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dressca.IntegrationTest;

[Collection("Shopping")]
public class DisplayItemTest(IntegrationTestWebApplicationFactory<Program> factory)
    : IClassFixture<IntegrationTestWebApplicationFactory<Program>>
{
    [Fact]
    public async Task 初期データの陳列品IDとカタログIDが分離されている()
    {
        using var client = factory.CreateClient();
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DresscaDbContext>();
        var cancellationToken = TestContext.Current.CancellationToken;
        var rows = await db.DisplayItems.AsNoTracking().ToListAsync(cancellationToken);
        Assert.Equal(11, rows.Count);
        Assert.All(rows, row => Assert.NotEqual(row.CatalogItemId, row.Id));
        Assert.Contains(rows, row => row.Id == DresscaSeedIds.DisplayItem1 && row.CatalogItemId == DresscaSeedIds.Item1);

        var repository = scope.ServiceProvider.GetRequiredService<IDisplayItemRepository>();
        Assert.Empty(await repository.FindAsync(item => item.Id == DresscaSeedIds.Item1, cancellationToken));
        Assert.Empty(await repository.FindAsync(item => Array.Empty<Guid>().Contains(item.Id), cancellationToken));

        using var response = await client.PostAsJsonAsync(
            "api/basket-items",
            new PostBasketItemsRequest
            {
                DisplayItemId = DresscaSeedIds.Item1,
                AddedQuantity = 1,
            },
            cancellationToken);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("displayIdNotFound", await response.Content.ReadAsStringAsync(cancellationToken));

        foreach (var path in new[] { "api/display-items", "api/display-item-brands", "api/display-item-categories" })
        {
            using var getResponse = await client.GetAsync(path, cancellationToken);
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        }
    }

    [Fact]
    public async Task 複数画像でも商品単位でページングし未陳列品を除外する()
    {
        using var client = factory.CreateClient();
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DresscaDbContext>();
        var cancellationToken = TestContext.Current.CancellationToken;
        var (firstId, secondId, brandId) = await AddItemsAsync(db, cancellationToken);
        try
        {
            var service = scope.ServiceProvider.GetRequiredService<DisplayItemApplicationService>();

            var (firstPage, total) = await service.GetDisplayItemsAsync(0, 1, brandId, DresscaSeedIds.Category1, cancellationToken);
            var (secondPage, secondTotal) = await service.GetDisplayItemsAsync(1, 1, brandId, DresscaSeedIds.Category1, cancellationToken);
            Assert.Equal(2, total);
            Assert.Equal(total, secondTotal);
            Assert.Single(firstPage);
            Assert.Single(secondPage);
            Assert.NotEqual(firstPage[0].Id, secondPage[0].Id);
            var items = firstPage.Concat(secondPage).ToArray();
            Assert.Equal(2, Assert.Single(items, item => item.Id == firstId).Assets.Count);
            Assert.Empty(Assert.Single(items, item => item.Id == secondId).Assets);
            Assert.All(items.SelectMany(item => item.Assets), asset => Assert.Equal(firstId, asset.DisplayItemId));
            Assert.Empty((await service.GetDisplayItemsAsync(0, 20, brandId, DresscaSeedIds.Category2, cancellationToken)).ItemsOnPage);
        }
        finally
        {
            db.ChangeTracker.Clear();
            await db.CatalogBrands.Where(brand => brand.Id == brandId).ExecuteDeleteAsync(CancellationToken.None);
        }
    }

    [Fact]
    public async Task カタログ更新は陳列品に反映され削除後も注文情報を保持する()
    {
        using var client = factory.CreateClient();
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DresscaDbContext>();
        var cancellationToken = TestContext.Current.CancellationToken;
        var (displayId, _, brandId) = await AddItemsAsync(db, cancellationToken);
        var catalogId = await db.DisplayItems.Where(item => item.Id == displayId).Select(item => item.CatalogItemId).SingleAsync(cancellationToken);
        db.ChangeTracker.Clear();
        var shopping = scope.ServiceProvider.GetRequiredService<ShoppingApplicationService>();
        var buyerId = Guid.CreateVersion7().ToString();
        try
        {
            var shipTo = new ShipTo("購入者", new Address("100-0001", "東京都", "千代田区", "1-1"));
            await shopping.AddItemToBasketAsync(buyerId, displayId, 2, cancellationToken);

            // カタログの更新後も、かごに追加した時点の単価を注文に使用する。
            await db.CatalogItems.Where(item => item.Id == catalogId).ExecuteUpdateAsync(
                setters => setters.SetProperty(item => item.Name, "更新後の商品名").SetProperty(item => item.Price, 2000m), cancellationToken);
            var repository = scope.ServiceProvider.GetRequiredService<IDisplayItemRepository>();
            var updated = Assert.Single(await repository.FindAsync(item => item.Id == displayId, cancellationToken));
            Assert.Equal("更新後の商品名", updated.Name);
            Assert.Equal(2000m, updated.Price);
            var order = await shopping.CheckoutAsync(buyerId, shipTo, cancellationToken);
            var orderItem = Assert.Single(order.OrderItems);
            Assert.Equal(displayId, orderItem.ItemOrdered.DisplayItemId);
            Assert.Equal("更新後の商品名", orderItem.ItemOrdered.ProductName);
            Assert.Equal(1000m, orderItem.UnitPrice);
            Assert.Equal(2, orderItem.Assets.Count);

            await shopping.AddItemToBasketAsync(buyerId, displayId, 1, cancellationToken);
            db.ChangeTracker.Clear();
            var catalogRepository = scope.ServiceProvider.GetRequiredService<ICatalogRepository>();
            var catalog = (await catalogRepository.GetAsync(catalogId, cancellationToken))!;
            var rowVersion = catalog.RowVersion.ToArray();
            db.ChangeTracker.Clear();
            await catalogRepository.RemoveAsync(catalogId, rowVersion, cancellationToken);
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => catalogRepository.RemoveAsync(catalogId, rowVersion, cancellationToken));

            var (basket, itemsInBasket, deletedIds) = await shopping.GetBasketItemsAsync(buyerId, cancellationToken);
            Assert.Single(basket.Items);
            Assert.True(Assert.Single(itemsInBasket).IsDeleted);
            Assert.Contains(displayId, deletedIds);
            var displayService = scope.ServiceProvider.GetRequiredService<DisplayItemApplicationService>();
            Assert.DoesNotContain((await displayService.GetDisplayItemsAsync(0, 20, brandId, null, cancellationToken)).ItemsOnPage, item => item.Id == displayId);
            await Assert.ThrowsAsync<DisplayItemNotExistingInRepositoryException>(() => shopping.AddItemToBasketAsync(buyerId, displayId, 1, cancellationToken));
            await Assert.ThrowsAsync<DisplayItemNotExistingInRepositoryException>(() => shopping.SetBasketItemsQuantitiesAsync(buyerId, new() { [displayId] = 2 }, cancellationToken));
            await Assert.ThrowsAsync<DisplayItemNotExistingInRepositoryException>(() => shopping.CheckoutAsync(buyerId, shipTo, cancellationToken));
            await shopping.RemoveItemFromBasketAsync(buyerId, displayId, cancellationToken);
            Assert.Empty((await shopping.GetBasketItemsAsync(buyerId, cancellationToken)).BasketResult.Items);

            db.ChangeTracker.Clear();
            var persisted = await scope.ServiceProvider.GetRequiredService<OrderApplicationService>().GetOrderAsync(order.Id, buyerId, cancellationToken);
            var persistedItem = Assert.Single(persisted.OrderItems);
            Assert.Equal("更新後の商品名", persistedItem.ItemOrdered.ProductName);
            Assert.Equal(1000m, persistedItem.UnitPrice);
            Assert.Equal(2, persistedItem.Assets.Count);
        }
        finally
        {
            db.ChangeTracker.Clear();
            await db.Baskets.Where(basket => basket.BuyerId == buyerId).ExecuteDeleteAsync(CancellationToken.None);
            await db.Orders.Where(order => order.BuyerId == buyerId).ExecuteDeleteAsync(CancellationToken.None);
            await db.CatalogBrands.Where(brand => brand.Id == brandId).ExecuteDeleteAsync(CancellationToken.None);
        }
    }

    private static async Task<(Guid FirstId, Guid SecondId, Guid BrandId)> AddItemsAsync(DresscaDbContext db, CancellationToken cancellationToken)
    {
        var brandId = Guid.CreateVersion7();
        db.CatalogBrands.Add(new CatalogBrand { Id = brandId, Name = "検証用ブランド" });
        var catalogs = Enumerable.Range(0, 3).Select(index => new CatalogItem
        {
            Id = Guid.CreateVersion7(), Name = $"商品{index}", Description = "説明", ProductCode = $"TEST{index}",
            Price = 1000m, CatalogBrandId = brandId, CatalogCategoryId = DresscaSeedIds.Category1,
        }).ToArray();
        db.CatalogItems.AddRange(catalogs);
        var firstId = Guid.CreateVersion7();
        var secondId = Guid.CreateVersion7();
        db.DisplayItems.AddRange(
            new DisplayItemEntity { Id = firstId, CatalogItemId = catalogs[0].Id },
            new DisplayItemEntity { Id = secondId, CatalogItemId = catalogs[1].Id });
        db.CatalogItemAssets.AddRange(
            new CatalogItemAsset { Id = Guid.CreateVersion7(), CatalogItemId = catalogs[0].Id, AssetCode = "image1" },
            new CatalogItemAsset { Id = Guid.CreateVersion7(), CatalogItemId = catalogs[0].Id, AssetCode = "image2" });
        await db.SaveChangesAsync(cancellationToken);
        return (FirstId: firstId, SecondId: secondId, BrandId: brandId);
    }
}
