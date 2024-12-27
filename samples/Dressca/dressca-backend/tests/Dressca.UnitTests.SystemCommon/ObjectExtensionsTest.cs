namespace Dressca.UnitTests.SystemCommon;

public class ObjectExtensionsTest
{
    [Fact]
    public void ThrowIfNull_参照型の値がnull_ArgumentNullExceptionが発生する()
    {
        // Arrange
        object? obj = null;

        // Act
        var action = () => ObjectExtensions.ThrowIfNull(obj);

        // Assert
        Assert.Throws<ArgumentNullException>("obj", action);
    }

    [Fact]
    public void ThrowIfNull_Nullableな値型の値がnull_ArgumentNullExceptionが発生する()
    {
        // Arrange
        int? intValue = null;

        // Act
        var action = () => ObjectExtensions.ThrowIfNull(intValue);

        // Assert
        Assert.Throws<ArgumentNullException>("intValue", action);
    }
}
