using Dressca.ApplicationCore.Catalog;

namespace Dressca.UnitTests.ApplicationCore.Catalog;

public class CatalogBrandTest
{
    [Fact]
    public void Constructor_正しくインスタンス化できる()
    {
        // Arrange
        const string brandName = "dressca";

        // Act
        var brand = new CatalogBrand(brandName);

        // Assert
        Assert.NotNull(brand);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ブランド名は必須(string? brandName)
    {
        // Arrange & Act
        var action = () => new CatalogBrand(brandName!);

        // Assert
        var ex = Assert.Throws<ArgumentException>("name", action);
        Assert.StartsWith("null または空の文字列を設定できません。", ex.Message);
    }
}
