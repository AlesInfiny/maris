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

    [Fact]
    public void SetName_空文字_ArgumentExceptionが発生()
    {
        // Arrange
        var item = CreateTestItem();
        var name = string.Empty;

        // Act
        var action = () => item.SetName(name);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void SetDescription_空文字_ArgumentExceptionが発生()
    {
        // Arrange
        var item = CreateTestItem();
        var description = string.Empty;

        // Act
        var action = () => item.SetDescription(description);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void SetPrice_負の数_ArgumentOutOfRangeExceptionが発生()
    {
        // Arrange
        var item = CreateTestItem();
        var price = -1000;

        // Act
        var action = () => item.SetPrice(price);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    [Fact]
    public void SetProductCode_半角英数字以外_ArgumentExceptionが発生()
    {
        // Arrange
        var item = CreateTestItem();
        var productCode = "商品コード";

        // Act
        var action = () => item.SetProductCode(productCode);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void SetCatalogBrandId_負の数_ArgumentOutOfRangeExceptionが発生()
    {
        // Arrange
        var item = CreateTestItem();
        var catalogBrandId = -99;

        // Act
        var action = () => item.SetCatalogBrandId(catalogBrandId);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    [Fact]
    public void SetCatalogCategoryId_負の数_ArgumentOutOfRangeExceptionが発生()
    {
        // Arrange
        var item = CreateTestItem();
        var catalogCategoryId = -99;

        // Act
        var action = () => item.SetCatalogCategoryId(catalogCategoryId);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    private static CatalogItem CreateTestItem()
    {
        return new() { CatalogCategoryId = 1L, CatalogBrandId = 1L, Description = "テスト用アイテムです。", Name = "テスト用アイテム", Price = 23800m, ProductCode = "TEST001", Id = 9999L };
    }
}
