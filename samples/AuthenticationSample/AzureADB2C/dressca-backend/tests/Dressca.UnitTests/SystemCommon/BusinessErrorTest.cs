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
        Assert.Equal(string.Empty, error.ErrorCode);
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
        string? errorCode = "ERR_CODE";
        string[] errorMessages = ["ERR_MESSAGE1", "ERR_MESSAGE2"];

        // Act
        var error = new BusinessError(errorCode, errorMessages);

        // Assert
        Assert.Equal(errorCode, error.ErrorCode);
    }

    [Fact]
    public void Constructor_引数ありコンストラクターを使用するとエラーメッセージリストは指定した値になる()
    {
        // Arrange
        string? errorCode = "ERR_CODE";
        string[] errorMessages = ["ERR_MESSAGE1", "ERR_MESSAGE2"];

        // Act
        var error = new BusinessError(errorCode, errorMessages);

        // Assert
        Assert.Collection(
            error.ErrorMessages,
            errorMessage => Assert.Equal(errorMessages[0], errorMessage),
            errorMessage => Assert.Equal(errorMessages[1], errorMessage));
    }

    [Fact]
    public void AddErrorMessage_nullを追加すると空文字が追加される()
    {
        // Arrange
        var error = new BusinessError();
        string? errorMessage = null;

        // Act
        error.AddErrorMessage(errorMessage);

        // Assert
        Assert.Single(error.ErrorMessages, errorMessage => errorMessage == string.Empty);
    }

    [Fact]
    public void AddErrorMessage_エラーメッセージを追加できる()
    {
        // Arrange
        var error = new BusinessError("ERR_CODE", ["ERR_MESSAGE1"]);
        string? errorMessage = "ERR_MESSAGE2";

        // Act
        error.AddErrorMessage(errorMessage);

        // Assert
        Assert.Collection(
            error.ErrorMessages,
            errorMessage => Assert.Equal("ERR_MESSAGE1", errorMessage),
            errorMessage => Assert.Equal("ERR_MESSAGE2", errorMessage));
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
        var error = new BusinessError("ERR_CODE", "エラー1", "ERR_MESSAGE2");

        // Act
        var str = error.ToString();

        // Assert
        Assert.Equal("{\"ERR_CODE\":[\"エラー1\",\"ERR_MESSAGE2\"]}", str);
    }
}
