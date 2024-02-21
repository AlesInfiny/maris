using System.Text.Encodings.Web;
using System.Text.Unicode;
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

    [Fact]
    public void GetInstance_Encoderの設定が指定通り()
    {
        // Arrange & Act
        var options = DefaultJsonSerializerOptions.GetInstance();

        // Assert
        // UnicodeRanges.All の範囲がエンコード可能か確認
        Assert.True(options.Encoder.WillEncode(0x0000));
        Assert.True(options.Encoder.WillEncode(0xFFFF));
        Assert.True(options.Encoder.WillEncode(0x3040));
    }
}
