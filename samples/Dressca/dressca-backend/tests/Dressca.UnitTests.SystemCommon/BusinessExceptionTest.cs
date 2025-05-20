using Dressca.SystemCommon;

namespace Dressca.UnitTests.SystemCommon;

public class BusinessExceptionTest
{
    [Fact]
    public void ToString_業務エラーのリストが文字列化される()
    {
        // Arrange
        string exceptionId = "ERR_CODE";
        ErrorMessage errorMessage = new ErrorMessage("ERROR_MESSAGE");
        BusinessError businessError = new BusinessError(exceptionId, errorMessage);
        var businessEx = new BusinessException(businessError);

        // Act
        var str = businessEx.ToString();

        // Assert
        Assert.Contains("[{\"ERR_CODE\":[\"ERROR_MESSAGE\"]}]", str);
    }

    [Fact]
    public void AddOrMergeError_nullを追加する_ArgumentNullExceptionが発生する()
    {
        // Arrange
        BusinessError? businessError = null;
        var businessEx = new BusinessException();

        // Act
        var action = () => businessEx.AddOrMergeError(businessError!);

        // Assert
        Assert.Throws<ArgumentNullException>("businessError", action);
    }

    [Fact]
    public void GetErrors_エラーメッセージ単位で業務エラーの情報を取得できる()
    {
        // Arrange
        string exceptionId1 = "ERR_CODE1";
        string exceptionId2 = "ERR_CODE2";
        ErrorMessage errorMessage1 = new ErrorMessage("ERROR_MESSAGE1");
        ErrorMessage errorMessage2 = new ErrorMessage("ERROR_MESSAGE2");
        ErrorMessage errorMessage3 = new ErrorMessage("ERROR_MESSAGE3");
        BusinessError businessError1 = new BusinessError(exceptionId1, errorMessage1, errorMessage2);
        BusinessError businessError2 = new BusinessError(exceptionId2, errorMessage3);
        var businessEx = new BusinessException(businessError1, businessError2);

        // Act
        var errors = businessEx.GetErrors();

        // Assert
        Assert.Collection(
            errors,
            error =>
            {
                Assert.Equal(exceptionId1, error.ExceptionId);
                Assert.Equal(errorMessage1.Message, error.ErrorMessage);
            },
            error =>
            {
                Assert.Equal(exceptionId1, error.ExceptionId);
                Assert.Equal(errorMessage2.Message, error.ErrorMessage);
            },
            error =>
            {
                Assert.Equal(exceptionId2, error.ExceptionId);
                Assert.Equal(errorMessage3.Message, error.ErrorMessage);
            });
    }
}
