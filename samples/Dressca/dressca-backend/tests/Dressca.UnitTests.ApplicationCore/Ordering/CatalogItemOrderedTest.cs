using Dressca.ApplicationCore.Ordering;

namespace Dressca.UnitTests.ApplicationCore.Ordering;

public class CatalogItemOrderedTest
{
    [Fact]
    public void Constructor_カタログアイテムIdが0以下_ArgumentOutOfRangeExceptionが発生する()
    {
        // Arrange
        long catalogItemId = 0L;
        string productName = "製品1";
        string productCode = "A000000001";

        // Act
        var action = () => new CatalogItemOrdered(catalogItemId, productName, productCode);

        // Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>("value", action);
        Assert.StartsWith("カタログアイテム ID は 0 以下にできません。", ex.Message);
        Assert.Equal(catalogItemId, ex.ActualValue);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_製品名がnullまたは空の文字列_ArgumentExceptionが発生する(string? productName)
    {
        // Arrange
        long catalogItemId = 1L;
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
        long catalogItemId = 1L;
        string productname = "製品1";

        // Act
        var action = () => new CatalogItemOrdered(catalogItemId, productname, productCode!);

        // Assert
        var ex = Assert.Throws<ArgumentException>("value", action);
        Assert.StartsWith("null または空の文字列を設定できません。", ex.Message);
    }
}
