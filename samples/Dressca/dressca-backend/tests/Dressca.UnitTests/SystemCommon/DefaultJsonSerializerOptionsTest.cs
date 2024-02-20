using Dressca.SystemCommon;

namespace Dressca.UnitTests.SystemCommon;

public class DefaultJsonSerializerOptionsTest
{
    [Fact]
    public void Options_インスタンスが取得できる()
    {
        // Arrange & Act
        var options = DefaultJsonSerializerOptions.Options;

        // Assert
        Assert.NotNull(options);
    }

    [Fact]
    public void Options_複数回取得しても同一のインスタンスとなる()
    {
        // Arrange & Act
        var options1 = DefaultJsonSerializerOptions.Options;
        var options2 = DefaultJsonSerializerOptions.Options;

        // Assert
        Assert.Equal(options1, options2);
    }
}
