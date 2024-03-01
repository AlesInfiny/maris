﻿using Dressca.SystemCommon;

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
        string errorCode1 = "ERR_CODE1";
        string errorCode2 = "ERR_CODE2";
        string errorMessage1 = "ERROR_MESSAGE1";
        string errorMessage2 = "ERROR_MESSAGE2";
        string errorMessage3 = "ERROR_MESSAGE3";
        BusinessError businessError1 = new BusinessError(errorCode1, errorMessage1, errorMessage2);
        BusinessError businessError2 = new BusinessError(errorCode2, errorMessage3);
        var businessEx = new BusinessException(businessError1, businessError2);

        // Act
        var errors = businessEx.GetErrors();

        // Assert
        Assert.Collection(
            errors,
            error =>
            {
                Assert.Equal(errorCode1, error.ErrorCode);
                Assert.Equal(errorMessage1, error.ErrorMessage);
            },
            error =>
            {
                Assert.Equal(errorCode1, error.ErrorCode);
                Assert.Equal(errorMessage2, error.ErrorMessage);
            },
            error =>
            {
                Assert.Equal(errorCode2, error.ErrorCode);
                Assert.Equal(errorMessage3, error.ErrorMessage);
            });
    }
}
