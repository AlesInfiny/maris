using Dressca.ApplicationCore.Ordering;

namespace Dressca.UnitTests.ApplicationCore.Ordering;

public class AddressTest
{
    [Fact]
    public void Constructor_郵便番号がnull_ArgumentNullExceptionが発生する()
    {
        // Arrange
        string? postalCode = null;
        string todofuken = "東京都";
        string shikuchoson = "千代田区";
        string azanaAndOthers = "永田町1-10-1";

        // Act
        var action = () => new Address(postalCode!, todofuken, shikuchoson, azanaAndOthers);

        // Assert
        Assert.Throws<ArgumentNullException>("value", action);
    }

    [Fact]
    public void Constructor_都道府県がnull_ArgumentNullExceptionが発生する()
    {
        // Arrange
        string postalCode = "100-8924";
        string? todofuken = null;
        string shikuchoson = "千代田区";
        string azanaAndOthers = "永田町1-10-1";

        // Act
        var action = () => new Address(postalCode, todofuken!, shikuchoson, azanaAndOthers);

        // Assert
        Assert.Throws<ArgumentNullException>("value", action);
    }

    [Fact]
    public void Constructor_市区町村がnull_ArgumentNullExceptionが発生する()
    {
        // Arrange
        string postalCode = "100-8924";
        string todofuken = "東京都";
        string? shikuchoson = null;
        string azanaAndOthers = "永田町1-10-1";

        // Act
        var action = () => new Address(postalCode, todofuken, shikuchoson!, azanaAndOthers);

        // Assert
        Assert.Throws<ArgumentNullException>("value", action);
    }

    [Fact]
    public void Constructor_字がnull_ArgumentNullExceptionが発生する()
    {
        // Arrange
        string postalCode = "100-8924";
        string todofuken = "東京都";
        string shikuchoson = "千代田区";
        string? azanaAndOthers = null;

        // Act
        var action = () => new Address(postalCode, todofuken, shikuchoson, azanaAndOthers!);

        // Assert
        Assert.Throws<ArgumentNullException>("value", action);
    }
}
