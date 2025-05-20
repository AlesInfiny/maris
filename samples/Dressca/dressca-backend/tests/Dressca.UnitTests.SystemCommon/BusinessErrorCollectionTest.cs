using Dressca.SystemCommon;

namespace Dressca.UnitTests.SystemCommon;

public class BusinessErrorCollectionTest
{
    [Fact]
    public void AddOrMerge_nullを追加する_ArgumentNullExceptionが発生する()
    {
        // Arrange
        var errors = new BusinessErrorCollection();
        BusinessError? newBusinessError = null;

        // Act
        var action = () => errors.AddOrMerge(newBusinessError!);

        // Assert
        Assert.Throws<ArgumentNullException>("newBusinessError", action);
    }

    [Fact]
    public void AddOrMerge_コレクションに追加していないエラーコードのデータは追加される()
    {
        // Arrange
        var errors = new BusinessErrorCollection();
        string errorCode = "ERR_CODE";
        ErrorMessage errorMessage = new ErrorMessage("ERROR_MESSAGE");
        BusinessError newBusinessError = new BusinessError(errorCode, errorMessage);

        // Act
        errors.AddOrMerge(newBusinessError);

        // Assert
        var error = Assert.Single(errors, error => error.ErrorCode == errorCode);
        Assert.Single(error.ErrorMessages, e => e.Message == errorMessage.Message);
    }

    [Fact]
    public void AddOrMerge_コレクションに追加済みのエラーコードのデータはマージされる()
    {
        // Arrange
        var errors = new BusinessErrorCollection();
        string errorCode = "ERR_CODE";
        ErrorMessage errorMessage1 = new ErrorMessage("ERROR_MESSAGE1");
        ErrorMessage errorMessage2 = new ErrorMessage("ERROR_MESSAGE2");
        BusinessError businessError1 = new BusinessError(errorCode, errorMessage1);
        errors.AddOrMerge(businessError1);
        BusinessError businessError2 = new BusinessError(errorCode, errorMessage2);

        // Act
        errors.AddOrMerge(businessError2);

        // Assert
        var error = Assert.Single(errors, error => error.ErrorCode == errorCode);
        Assert.Collection(
            error.ErrorMessages,
            message => Assert.Equal(errorMessage1.Message, message.Message),
            message => Assert.Equal(errorMessage2.Message, message.Message));
    }

    [Fact]
    public void ToString_業務エラーのエラーコードとエラーメッセージがすべてJSON形式に変換される()
    {
        // Arrange
        var errors = new BusinessErrorCollection();
        string errorCode1 = "ERR_CODE1";
        ErrorMessage errorMessage1_1 = new ErrorMessage("ERROR_MESSAGE1-1");
        ErrorMessage errorMessage1_2 = new ErrorMessage("ERROR_MESSAGE1-2");
        BusinessError businessError1 = new BusinessError(errorCode1, errorMessage1_1, errorMessage1_2);
        errors.AddOrMerge(businessError1);
        string errorCode2 = "ERR_CODE2";
        ErrorMessage errorMessage2_1 = new ErrorMessage("ERROR_MESSAGE2-1");
        ErrorMessage errorMessage2_2 = new ErrorMessage("ERROR_MESSAGE2-2");
        BusinessError businessError2 = new BusinessError(errorCode2, errorMessage2_1, errorMessage2_2);
        errors.AddOrMerge(businessError2);

        // Act
        var str = errors.ToString();

        // Assert
        Assert.Equal("[{\"ERR_CODE1\":[\"ERROR_MESSAGE1-1\",\"ERROR_MESSAGE1-2\"]},{\"ERR_CODE2\":[\"ERROR_MESSAGE2-1\",\"ERROR_MESSAGE2-2\"]}]", str);
    }
}
