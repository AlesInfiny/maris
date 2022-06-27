using Dressca.ApplicationCore.Ordering;

namespace Dressca.UnitTests.ApplicationCore.Ordering;

public class AddressTest
{
    [Fact]
    public void Constructor_郵便番号がnullの場合例外()
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
    public void Constructor_都道府県がnullの場合例外()
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
    public void Constructor_市区町村がnullの場合例外()
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
    public void Constructor_字がnullの場合例外()
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
