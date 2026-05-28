using Dressca.ApplicationCore.Catalog;

namespace Dressca.UnitTests.ApplicationCore.Catalog;

public class CatalogItemTest
{
    [Fact]
    public void Constructor_正しくインスタンス化できる()
    {
        // Arrange
        // Nothing to do.
        var category1 = new Guid("01971a00-0000-7000-c000-000000000001");
        var brand2 = new Guid("01971a00-0000-7000-b000-000000000002");

        // Act
        // コンストラクタのテストなので CreateDefaultCatalogItem は使わない
        var catalogItem = new CatalogItem { CatalogCategoryId = category1, CatalogBrandId = brand2, Description = "説明", Name = "商品名", Price = 100m, ProductCode = "C000000001" };

        // Assert
        Assert.NotNull(catalogItem);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("　")]
    public void Constructor_Name_nullまたは空白文字_ArgumentExceptionが発生(string? name)
    {
        // Arrange & Act
        var category1 = new Guid("01971a00-0000-7000-c000-000000000001");
        var brand1 = new Guid("01971a00-0000-7000-b000-000000000001");
        var action = () => new CatalogItem
        {
            CatalogCategoryId = category1,
            CatalogBrandId = brand1,
            Description = "テスト用アイテムです。",
            Name = name!,
            Price = 23800m,
            ProductCode = "TEST001",
            Id = Guid.CreateVersion7(),
        };

        // Assert
        var exception = Assert.Throws<ArgumentException>("value", action);
        Assert.StartsWith("null または空の文字列を設定できません。", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("　")]
    public void Constructor_Description_nullまたは空白文字_ArgumentExceptionが発生(string? description)
    {
        // Arrange & Act
        var category1 = new Guid("01971a00-0000-7000-c000-000000000001");
        var brand1 = new Guid("01971a00-0000-7000-b000-000000000001");
        var action = () => new CatalogItem
        {
            CatalogCategoryId = category1,
            CatalogBrandId = brand1,
            Description = description!,
            Name = "テスト用アイテム",
            Price = 23800m,
            ProductCode = "TEST001",
            Id = Guid.CreateVersion7(),
        };

        // Assert
        var exception = Assert.Throws<ArgumentException>("value", action);
        Assert.StartsWith("null または空の文字列を設定できません。", exception.Message);
    }

    [Fact]
    public void Constructor_Price_負の数_ArgumentOutOfRangeExceptionが発生()
    {
        // Arrange
        var price = -100m;
        var category1 = new Guid("01971a00-0000-7000-c000-000000000001");
        var brand1 = new Guid("01971a00-0000-7000-b000-000000000001");

        // Act
        var action = () => new CatalogItem
        {
            CatalogCategoryId = category1,
            CatalogBrandId = brand1,
            Description = "テスト用アイテムです。",
            Name = "テスト用アイテム",
            Price = price,
            ProductCode = "TEST001",
            Id = Guid.CreateVersion7(),
        };

        // Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>("value", action);
        Assert.StartsWith("単価は負の値に設定できません。", exception.Message);
    }

    [Fact]
    public void Constructor_ProductCode_半角英数字以外_ArgumentExceptionが発生()
    {
        // Arrange
        var productCode = "商品コード";
        var category1 = new Guid("01971a00-0000-7000-c000-000000000001");
        var brand1 = new Guid("01971a00-0000-7000-b000-000000000001");

        // Act
        var action = () => new CatalogItem
        {
            CatalogCategoryId = category1,
            CatalogBrandId = brand1,
            Description = "テスト用アイテムです。",
            Name = "テスト用アイテム",
            Price = 23800m,
            ProductCode = productCode,
            Id = Guid.CreateVersion7(),
        };

        // Assert
        var exception = Assert.Throws<ArgumentException>("value", action);
        Assert.StartsWith("半角英数字を設定してください。", exception.Message);
    }

    [Fact]
    public void Constructor_CatalogCategoryId_空Guid_ArgumentExceptionが発生()
    {
        // Arrange
        var catalogCategoryId = Guid.Empty;
        var brand1 = new Guid("01971a00-0000-7000-b000-000000000001");

        // Act
        var action = () => new CatalogItem
        {
            CatalogCategoryId = catalogCategoryId,
            CatalogBrandId = brand1,
            Description = "テスト用アイテムです。",
            Name = "テスト用アイテム",
            Price = 23800m,
            ProductCode = "TEST001",
            Id = Guid.CreateVersion7(),
        };

        // Assert
        var exception = Assert.Throws<ArgumentException>("value", action);
        Assert.StartsWith("カタログカテゴリ ID は 0 以下にできません。", exception.Message);
    }

    [Fact]
    public void Constructor_CatalogBrandId_空Guid_ArgumentExceptionが発生()
    {
        // Arrange
        var catalogBrandId = Guid.Empty;
        var category1 = new Guid("01971a00-0000-7000-c000-000000000001");

        // Act
        var action = () => new CatalogItem
        {
            CatalogCategoryId = category1,
            CatalogBrandId = catalogBrandId,
            Description = "テスト用アイテムです。",
            Name = "テスト用アイテム",
            Price = 23800m,
            ProductCode = "TEST001",
            Id = Guid.CreateVersion7(),
        };

        // Assert
        var exception = Assert.Throws<ArgumentException>("value", action);
        Assert.StartsWith("カタログブランド ID は 0 以下にできません。", exception.Message);
    }
}
