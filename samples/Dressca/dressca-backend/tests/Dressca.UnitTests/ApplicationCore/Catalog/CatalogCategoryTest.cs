using Dressca.ApplicationCore.Catalog;

namespace Dressca.UnitTests.ApplicationCore.Catalog;

public class CatalogCategoryTest
{
    [Fact]
    public void Constructor_正しくインスタンス化できる()
    {
        // Arrange
        const string categoryName = "dressca";

        // Act
        var category = new CatalogCategory { Name = categoryName };

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
        var action = () => new CatalogCategory { Name = categoryName! };

        // Assert
        var ex = Assert.Throws<ArgumentException>("value", action);
        Assert.StartsWith("null または空の文字列を設定できません。", ex.Message);
    }
}
