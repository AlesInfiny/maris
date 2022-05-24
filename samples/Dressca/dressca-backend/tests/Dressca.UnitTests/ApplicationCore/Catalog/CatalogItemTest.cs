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
        var catalogItem = new CatalogItem(1L, 2L, "説明", "商品名", 100m, "C000000001");

        // Assert
        Assert.NotNull(catalogItem);
    }
}
