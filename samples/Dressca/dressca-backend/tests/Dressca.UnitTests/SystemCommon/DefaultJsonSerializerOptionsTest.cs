using Dressca.SystemCommon.Text.Json;

namespace Dressca.UnitTests.SystemCommon;

public class DefaultJsonSerializerOptionsTest
{
    [Fact]
    public void GetInstance_インスタンスが取得できる()
    {
        // Arrange & Act
        var options = DefaultJsonSerializerOptions.GetInstance();

        // Assert
        Assert.NotNull(options);
    }

    [Fact]
    public void GetInstance_複数回呼び出しても同一のインスタンスを取得できる()
    {
        // Arrange & Act
        var options1 = DefaultJsonSerializerOptions.GetInstance();
        var options2 = DefaultJsonSerializerOptions.GetInstance();

        // Assert
        Assert.Equal(options1, options2);
    }
}
