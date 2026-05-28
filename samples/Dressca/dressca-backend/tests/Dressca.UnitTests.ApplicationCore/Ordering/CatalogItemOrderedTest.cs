using Dressca.ApplicationCore.Ordering;

namespace Dressca.UnitTests.ApplicationCore.Ordering;

public class CatalogItemOrderedTest
{
    [Fact]
    public void Constructor_カタログアイテムIdが空Guid_ArgumentExceptionが発生する()
    {
        // Arrange
        var catalogItemId = Guid.Empty;
        string productName = "製品1";
        string productCode = "A000000001";

        // Act
        var action = () => new CatalogItemOrdered(catalogItemId, productName, productCode);

        // Assert
        var ex = Assert.Throws<ArgumentException>("value", action);
        Assert.StartsWith("カタログアイテム ID は 0 以下にできません。", ex.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_製品名がnullまたは空の文字列_ArgumentExceptionが発生する(string? productName)
    {
        // Arrange
        var catalogItemId = new Guid("01971a00-0000-7000-d000-000000000001");
        string productCode = "A000000001";

        // Act
        var action = () => new CatalogItemOrdered(catalogItemId, productName!, productCode);

        // Assert
        var ex = Assert.Throws<ArgumentException>("value", action);
        Assert.StartsWith("null または空の文字列を設定できません。", ex.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_製品コードがnullまたは空の文字列_ArgumentExceptionが発生する(string? productCode)
    {
        // Arrange
        var catalogItemId = new Guid("01971a00-0000-7000-d000-000000000001");
        string productname = "製品1";

        // Act
        var action = () => new CatalogItemOrdered(catalogItemId, productname, productCode!);

        // Assert
        var ex = Assert.Throws<ArgumentException>("value", action);
        Assert.StartsWith("null または空の文字列を設定できません。", ex.Message);
    }
}
