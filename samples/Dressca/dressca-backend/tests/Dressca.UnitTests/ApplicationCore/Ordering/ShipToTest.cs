using Dressca.ApplicationCore.Ordering;

namespace Dressca.UnitTests.ApplicationCore.Ordering;

public class ShipToTest
{
    [Fact]
    public void 宛名がnullの場合例外()
    {
        // Arrange
        string? fullName = null;
        var address = new Address("100-8924", "東京都", "千代田区", "永田町1-10-1");

        // Act
        var action = () => new ShipTo(fullName!, address);

        // Assert
        Assert.Throws<ArgumentNullException>("value", action);
    }

    [Fact]
    public void 住所がnullの場合例外()
    {
        // Arrange
        string fullName = "国会　太郎";
        Address? address = null;

        // Act
        var action = () => new ShipTo(fullName, address!);

        // Assert
        Assert.Throws<ArgumentNullException>("value", action);
    }
}
