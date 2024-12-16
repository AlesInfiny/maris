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

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("　")]
    public void Constructor_Name_nullまたは空白文字_ArgumentExceptionが発生(string? name)
    {
        // Arrange & Act
        var action = () => new CatalogItem
        {
            CatalogCategoryId = 1L,
            CatalogBrandId = 1L,
            Description = "テスト用アイテムです。",
            Name = name!,
            Price = 23800m,
            ProductCode = "TEST001",
            Id = 9999L,
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
        var action = () => new CatalogItem
        {
            CatalogCategoryId = 1L,
            CatalogBrandId = 1L,
            Description = description!,
            Name = "テスト用アイテム",
            Price = 23800m,
            ProductCode = "TEST001",
            Id = 9999L,
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

        // Act
        var action = () => new CatalogItem
        {
            CatalogCategoryId = 1L,
            CatalogBrandId = 1L,
            Description = "テスト用アイテムです。",
            Name = "テスト用アイテム",
            Price = price,
            ProductCode = "TEST001",
            Id = 9999L,
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

        // Act
        var action = () => new CatalogItem
        {
            CatalogCategoryId = 1L,
            CatalogBrandId = 1L,
            Description = "テスト用アイテムです。",
            Name = "テスト用アイテム",
            Price = 23800m,
            ProductCode = productCode,
            Id = 9999L,
        };

        // Assert
        var exception = Assert.Throws<ArgumentException>("value", action);
        Assert.StartsWith("半角英数字を設定してください。", exception.Message);
    }

    [Fact]
    public void Constructor_CatalogCategoryId_負の数_ArgumentOutOfRangeExceptionが発生()
    {
        // Arrange
        var catalogCategoryId = -99L;

        // Act
        var action = () => new CatalogItem
        {
            CatalogCategoryId = catalogCategoryId,
            CatalogBrandId = 1L,
            Description = "テスト用アイテムです。",
            Name = "テスト用アイテム",
            Price = 23800m,
            ProductCode = "TEST001",
            Id = 9999L,
        };

        // Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>("value", action);
        Assert.StartsWith("カタログカテゴリ ID は 0 以下にできません。", exception.Message);
    }

    [Fact]
    public void Constructor_CatalogBrandId_負の数_ArgumentOutOfRangeExceptionが発生()
    {
        // Arrange
        var catalogBrandId = -99L;

        // Act
        var action = () => new CatalogItem
        {
            CatalogCategoryId = 1L,
            CatalogBrandId = catalogBrandId,
            Description = "テスト用アイテムです。",
            Name = "テスト用アイテム",
            Price = 23800m,
            ProductCode = "TEST001",
            Id = 9999L,
        };

        // Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>("value", action);
        Assert.StartsWith("カタログブランド ID は 0 以下にできません。", exception.Message);
    }
}
