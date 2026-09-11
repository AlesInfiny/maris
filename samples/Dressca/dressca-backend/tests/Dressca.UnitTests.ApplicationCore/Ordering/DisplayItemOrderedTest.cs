using Dressca.ApplicationCore.Ordering;

namespace Dressca.UnitTests.ApplicationCore.Ordering;

public class DisplayItemOrderedTest
{
    [Fact]
    public void Constructor_陳列品Idが空Guid_ArgumentExceptionが発生する()
    {
        // Arrange
        var displayItemId = Guid.Empty;
        string productName = "製品1";
        string productCode = "A000000001";

        // Act
        var action = () => new DisplayItemOrdered(displayItemId, productName, productCode);

        // Assert
        var ex = Assert.Throws<ArgumentException>("value", action);
        Assert.StartsWith("陳列品 ID に空の Guid は設定できません。", ex.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_製品名がnullまたは空の文字列_ArgumentExceptionが発生する(string? productName)
    {
        // Arrange
        var displayItemId = Guid.CreateVersion7();
        string productCode = "A000000001";

        // Act
        var action = () => new DisplayItemOrdered(displayItemId, productName!, productCode);

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
        var displayItemId = Guid.CreateVersion7();
        string productname = "製品1";

        // Act
        var action = () => new DisplayItemOrdered(displayItemId, productname, productCode!);

        // Assert
        var ex = Assert.Throws<ArgumentException>("value", action);
        Assert.StartsWith("null または空の文字列を設定できません。", ex.Message);
    }
}
