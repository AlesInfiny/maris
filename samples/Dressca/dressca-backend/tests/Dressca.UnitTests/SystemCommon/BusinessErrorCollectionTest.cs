using Dressca.SystemCommon;

namespace Dressca.UnitTests.SystemCommon;

public class BusinessErrorCollectionTest
{
    [Fact]
    public void AddOrMerge_nullを追加しようとすると例外()
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
}
