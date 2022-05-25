using Dressca.SystemCommon;

namespace Dressca.UnitTests.SystemCommon;

public class BusinessExceptionTest
{
    [Fact]
    public void ToString_業務エラーのリストが文字列化される()
    {
        // Arrange
        string errorCode = "ERR_CODE";
        string errorMessage = "ERROR_MESSAGE";
        BusinessError businessError = new BusinessError(errorCode, errorMessage);
        var businessEx = new BusinessException(businessError);

        // Act
        var str = businessEx.ToString();

        // Assert
        Assert.Contains("[{\"ERR_CODE\":[\"ERROR_MESSAGE\"]}]", str);
    }

    [Fact]
    public void AddOrMergeError_nullを追加しようとすると例外()
    {
        // Arrange
        BusinessError? businessError = null;
        var businessEx = new BusinessException();

        // Act
        var action = () => businessEx.AddOrMergeError(businessError!);

        // Assert
        Assert.Throws<ArgumentNullException>("businessError", action);
    }
}
