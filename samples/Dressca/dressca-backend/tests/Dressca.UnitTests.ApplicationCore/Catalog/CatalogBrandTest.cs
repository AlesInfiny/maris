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
        var brand = new CatalogBrand() { Name = brandName };

        // Assert
        Assert.NotNull(brand);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ブランド名がnullまたは空白文字_ArgumentExceptionが発生する(string? brandName)
    {
        // Arrange & Act
        var action = () => new CatalogBrand() { Name = brandName! };

        // Assert
        var ex = Assert.Throws<ArgumentException>("value", action);
        Assert.StartsWith("null または空の文字列を設定できません。", ex.Message);
    }
}
