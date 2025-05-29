using Dressca.SystemCommon;

namespace Dressca.UnitTests.SystemCommon;

public class BusinessErrorTest
{
    [Fact]
    public void Constructor_引数なしコンストラクターを使用するとエラーコードは空文字になる()
    {
        // Arrange & Act
        var error = new BusinessError();

        // Assert
        Assert.Equal(string.Empty, error.ExceptionId);
    }

    [Fact]
    public void Constructor_引数なしコンストラクターを使用するとエラーメッセージリストは空リストになる()
    {
        // Arrange & Act
        var error = new BusinessError();

        // Assert
        Assert.Empty(error.ErrorMessages);
    }

    [Fact]
    public void Constructor_引数ありコンストラクターを使用するとエラーコードは指定した値になる()
    {
        // Arrange
        string? exceptionId = "ERR_CODE";
        ErrorMessage[] errorMessages = [new ErrorMessage("ERR_MESSAGE1"), new ErrorMessage("ERR_MESSAGE2")];

        // Act
        var error = new BusinessError(exceptionId, errorMessages);

        // Assert
        Assert.Equal(exceptionId, error.ExceptionId);
    }

    [Fact]
    public void Constructor_引数ありコンストラクターを使用するとエラーメッセージリストは指定した値になる()
    {
        // Arrange
        string? exceptionId = "ERR_CODE";
        ErrorMessage[] errorMessages = [new ErrorMessage("ERR_MESSAGE1"), new ErrorMessage("ERR_MESSAGE2")];

        // Act
        var error = new BusinessError(exceptionId, errorMessages);

        // Assert
        Assert.Collection(
            error.ErrorMessages.Select(e => e.Message),
            errorMessage => Assert.Equal(errorMessages[0].Message, errorMessage),
            errorMessage => Assert.Equal(errorMessages[1].Message, errorMessage));
    }

    [Fact]
    public void AddErrorMessage_エラーメッセージを追加できる()
    {
        // Arrange
        var error = new BusinessError("ERR_CODE", new ErrorMessage("ERR_MESSAGE1"));
        ErrorMessage errorMessage = new ErrorMessage("ERR_MESSAGE2");

        // Act
        error.AddErrorMessage(errorMessage);

        // Assert
        Assert.Collection(
            error.ErrorMessages,
            e => Assert.Equal("ERR_MESSAGE1", e.Message),
            e => Assert.Equal("ERR_MESSAGE2", e.Message));
    }

    [Fact]
    public void ToString_エラーコードが未設定_キーが空文字のJSON形式に変換される()
    {
        // Arrange
        var error = new BusinessError();

        // Act
        var str = error.ToString();

        // Assert
        Assert.Equal("{\"\":[]}", str);
    }

    [Fact]
    public void ToString_エラーコードがキーでエラーメッセージのリストが値のJSON形式に変換される()
    {
        // Arrange
        var error = new BusinessError("ERR_CODE", new ErrorMessage("エラー1"), new ErrorMessage("ERR_MESSAGE2"));

        // Act
        var str = error.ToString();

        // Assert
        Assert.Equal("{\"ERR_CODE\":[\"エラー1\",\"ERR_MESSAGE2\"]}", str);
    }
}
