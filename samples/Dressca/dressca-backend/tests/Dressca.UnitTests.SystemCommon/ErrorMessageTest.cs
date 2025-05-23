using Dressca.SystemCommon;

namespace Dressca.UnitTests.SystemCommon;

public class ErrorMessageTest
{
    [Fact]
    public void Constructor_errorMessageValuesが指定の値に設定される()
    {
        // Arrange & Act
        var message = "エラーが発生しました。ID：{0}";
        var errorMessageValue1 = 1;
        var errorMessageValue2 = 2;
        ErrorMessage errorMessage = new ErrorMessage(message, errorMessageValue1, errorMessageValue2);

        // Assert
        Assert.Collection(
            errorMessage.ErrorMessageValues,
            v => Assert.Equal(v, errorMessageValue1),
            v => Assert.Equal(v, errorMessageValue2));
    }

    [Fact]
    public void Constructor_プレースホルダーの値を補完したエラーメッセージが設定される()
    {
        // Arrange & Act
        var message = "エラーが発生しました。ID：{0}";
        var errorMessageValues = "1";
        ErrorMessage errorMessage = new ErrorMessage(message, errorMessageValues);

        // Assert
        Assert.Equal(string.Format(message, errorMessageValues), errorMessage.Message);
    }

    [Fact]
    public void Constructor_errorMessageをnullにすると空文字がMessageに設定される()
    {
        // Arrange & Act
        ErrorMessage errorMessage = new ErrorMessage(null!);

        // Assert
        Assert.Empty(errorMessage.Message);
    }

    [Fact]
    public void ToString_プレースホルダーの値を補完したエラーメッセージを取得できる()
    {
        // Arrange & Act
        var message = "エラーが発生しました。ID：{0}";
        var errorMessageValues = "1";
        ErrorMessage errorMessage = new ErrorMessage(message, errorMessageValues);

        // Assert
        Assert.Equal(string.Format(message, errorMessageValues), errorMessage.ToString());
    }
}
