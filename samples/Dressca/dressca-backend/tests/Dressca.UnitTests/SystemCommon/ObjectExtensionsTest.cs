using Dressca.SystemCommon;

namespace Dressca.UnitTests.SystemCommon;

public class ObjectExtensionsTest
{
    [Fact]
    public void 参照型の値がnullの場合例外()
    {
        // Arrange
        object? obj = null;

        // Act
        var action = () => ObjectExtensions.ThrowIfNull(obj);

        // Assert
        Assert.Throws<ArgumentNullException>("obj", action);
    }

    [Fact]
    public void Nullableな値型の値がnullの場合例外()
    {
        // Arrange
        int? intValue = null;

        // Act
        var action = () => ObjectExtensions.ThrowIfNull(intValue);

        // Assert
        Assert.Throws<ArgumentNullException>("intValue", action);
    }
}
