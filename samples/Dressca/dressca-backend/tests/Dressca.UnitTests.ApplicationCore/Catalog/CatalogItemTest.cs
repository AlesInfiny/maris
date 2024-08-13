using Dressca.ApplicationCore.Catalog;

namespace Dressca.UnitTests.ApplicationCore.Catalog;

public class CatalogItemTest
{
    [Fact]
    public void Constructor_正しくインスタンス化できる()
    {
        // Arrange
        // Nothing to do.

        // Act
        // コンストラクタのテストなので CreateDefaultCatalogItem は使わない
        var catalogItem = new CatalogItem { CatalogCategoryId = 1L, CatalogBrandId = 2L, Description = "説明", Name = "商品名", Price = 100m, ProductCode = "C000000001" };

        // Assert
        Assert.NotNull(catalogItem);
    }
}
