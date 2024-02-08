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
        string errorMessage = "ERROR_MESSAGE";
        BusinessError newBusinessError = new BusinessError(errorCode, errorMessage);

        // Act
        errors.AddOrMerge(newBusinessError);

        // Assert
        Assert.Collection(
            errors,
            error =>
            {
                Assert.Equal(errorCode, error.ErrorCode);
                Assert.Collection(
                    error.ErrorMessages,
                    message => Assert.Equal(errorMessage, message));
            });
    }

    [Fact]
    public void AddOrMerge_コレクションに追加済みのエラーコードのデータはマージされる()
    {
        // Arrange
        var errors = new BusinessErrorCollection();
        string errorCode = "ERR_CODE";
        string errorMessage1 = "ERROR_MESSAGE1";
        string errorMessage2 = "ERROR_MESSAGE2";
        BusinessError businessError1 = new BusinessError(errorCode, errorMessage1);
        errors.AddOrMerge(businessError1);
        BusinessError businessError2 = new BusinessError(errorCode, errorMessage2);

        // Act
        errors.AddOrMerge(businessError2);

        // Assert
        Assert.Collection(
            errors,
            error =>
            {
                Assert.Equal(errorCode, error.ErrorCode);
                Assert.Collection(
                    error.ErrorMessages,
                    message => Assert.Equal(errorMessage1, message),
                    message => Assert.Equal(errorMessage2, message));
            });
    }

    [Fact]
    public void ToString_業務エラーのエラーコードとエラーメッセージがすべてJSON形式に変換される()
    {
        // Arrange
        var errors = new BusinessErrorCollection();
        string errorCode1 = "ERR_CODE1";
        string errorMessage1_1 = "ERROR_MESSAGE1-1";
        string errorMessage1_2 = "ERROR_MESSAGE1-2";
        BusinessError businessError1 = new BusinessError(errorCode1, errorMessage1_1, errorMessage1_2);
        errors.AddOrMerge(businessError1);
        string errorCode2 = "ERR_CODE2";
        string errorMessage2_1 = "ERROR_MESSAGE2-1";
        string errorMessage2_2 = "ERROR_MESSAGE2-2";
        BusinessError businessError2 = new BusinessError(errorCode2, errorMessage2_1, errorMessage2_2);
        errors.AddOrMerge(businessError2);

        // Act
        var str = errors.ToString();

        // Assert
        Assert.Equal("[{\"ERR_CODE1\":[\"ERROR_MESSAGE1-1\",\"ERROR_MESSAGE1-2\"]},{\"ERR_CODE2\":[\"ERROR_MESSAGE2-1\",\"ERROR_MESSAGE2-2\"]}]", str);
    }
}
