using Dressca.ApplicationCore.Catalog;

namespace Dressca.UnitTests.ApplicationCore.Catalog;

public class CatalogItemTest
{
    [Fact]
    public void 正しくインスタンス化できる()
    {
        // Arrange
        // Nothing to do.

        // Act
        // コンストラクタのテストなので CreateDefaultCatalogItem は使わない
        var catalogItem = new CatalogItem(1L, 2L, "説明", "商品名", 100m, "C000000001");

        // Assert
        Assert.NotNull(catalogItem);
    }

    [Theory]
    [InlineData("ダミー商品", "テスト.", 0)]
    [InlineData("ダミー商品", "テスト.", 2)]
    public void カタログの詳細情報を正しく更新できる(string name, string description, decimal price)
    {
        // Arrange
        var catalogItem = CreateDefaultCatalogItem();

        // Act
        catalogItem.UpdateDetails(name, description, price);

        // Assert
        Assert.Equal(name, catalogItem.Name);
        Assert.Equal(description, catalogItem.Description);
        Assert.Equal(price, catalogItem.Price);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void カタログの詳細情報を更新する際に商品名は必須(string? name)
    {
        // Arrange
        var catalogItem = CreateDefaultCatalogItem();
        const string NEW_DESCRIPTION = "テスト.";

        // Act
        var action = () => catalogItem.UpdateDetails(name!, NEW_DESCRIPTION, 2m);

        // Assert
        var ex = Assert.Throws<ArgumentException>("value", action);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void カタログの詳細情報を更新する際に説明は必須(string? description)
    {
        // Arrange
        var catalogItem = CreateDefaultCatalogItem();
        const string NEW_NAME = "ダミー商品";

        // Act
        var action = () => catalogItem.UpdateDetails(NEW_NAME, description!, 2m);

        // Assert
        var ex = Assert.Throws<ArgumentException>("value", action);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void カタログの詳細情報を更新する際に単価は負数にできない(decimal price)
    {
        // Arrange
        var catalogItem = CreateDefaultCatalogItem();
        const string NEW_NAME = "ダミー商品";
        const string NEW_DESCRIPTION = "テスト.";

        // Act
        var action = () => catalogItem.UpdateDetails(NEW_NAME, NEW_DESCRIPTION, price);

        // Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>("value", action);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    public void カタログブランドを正しく更新できる(long catalogBrandId)
    {
        // Arrange
        var catalogItem = CreateDefaultCatalogItem();

        // Act
        catalogItem.UpdateBrand(catalogBrandId);

        // Assert
        Assert.Equal(catalogBrandId, catalogItem.CatalogBrandId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void カタログブランドを更新する際にカタログブランドIdを0以下にできない(long catalogBrandId)
    {
        // Arrange
        var catalogItem = CreateDefaultCatalogItem();

        // Act
        var action = () => catalogItem.UpdateBrand(catalogBrandId);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    public void カタログカテゴリを正しく更新できる(long catalogCategoryId)
    {
        // Arrange
        var catalogItem = CreateDefaultCatalogItem();

        // Act
        catalogItem.UpdateCategory(catalogCategoryId);

        // Assert
        Assert.Equal(catalogCategoryId, catalogItem.CatalogCategoryId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void カタログカテゴリを更新する際にカタログカテゴリIdを0以下にできない(long catalogCategoryId)
    {
        // Arrange
        var catalogItem = CreateDefaultCatalogItem();

        // Act
        var action = () => catalogItem.UpdateCategory(catalogCategoryId);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void 商品コードをnullまたは空文字で更新できない(string? productCode)
    {
        // Arrange
        var catalogItem = CreateDefaultCatalogItem();

        // Act
        var action = () => catalogItem.UpdateProductCode(productCode!);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    private static CatalogItem CreateDefaultCatalogItem()
    {
        const long defaultCatalogCategoryId = 1L;
        const long defaultCatalogBrandId = 1L;
        const string defaultDescription = "Description.";
        const string defaultName = "Name";
        const decimal defaultPrice = 1m;
        const string defaultProductCode = "C000000001";

        return new CatalogItem(defaultCatalogCategoryId, defaultCatalogBrandId, defaultDescription, defaultName, defaultPrice, defaultProductCode);
    }
}
