using Dressca.ApplicationCore.Catalog;

namespace Dressca.UnitTests.ApplicationCore.Catalog;

public class CatalogCategoryTest
{
    [Fact]
    public void Constructor_正しくインスタンス化できる()
    {
        // Arrange
        const string brandName = "dressca";

        // Act
        var category = new CatalogCategory(brandName);

        // Assert
        Assert.NotNull(category);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_カテゴリ名がnullまたは空白文字_ArgumentExceptionが発生する(string? categoryName)
    {
        // Arrange & Act
        var action = () => new CatalogCategory(categoryName!);

        // Assert
        var ex = Assert.Throws<ArgumentException>("name", action);
        Assert.StartsWith("null または空の文字列を設定できません。", ex.Message);
    }
}
