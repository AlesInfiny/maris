﻿using System.ComponentModel.DataAnnotations;
using Maris.ConsoleApp.Core;

namespace Maris.ConsoleApp.UnitTests.Core;

public class InvalidParameterExceptionTest
{
    [Fact]
    public void Message_メッセージの既定値_コマンドのパラメーターに入力エラーがあります()
    {
        // Arrange
        var ex = new InvalidParameterException();

        // Act
        var message = ex.Message;

        // Assert
        Assert.Equal("コマンドのパラメーターに入力エラーがあります。", message);
    }

    [Fact]
    public void Message_検証結果が1件登録されている_メンバー名とエラーメッセージが含まれている()
    {
        // Arrange
        var errorMessage = "error message";
        var memberNames = new string[] { "param1", "param2" };
        var validationResults = new List<ValidationResult>
        {
            new(errorMessage, memberNames),
        };
        var ex = new InvalidParameterException(validationResults);

        // Act
        var message = ex.Message;

        // Assert
        Assert.Equal($"コマンドのパラメーターに入力エラーがあります。パラメーターの入力エラー情報詳細 : [{{\"MemberNames\":[\"{memberNames[0]}\",\"{memberNames[1]}\"],\"ErrorMessage\":\"{errorMessage}\"}}] 。", message);
    }

    [Fact]
    public void Message_検証結果が2件登録されている場合_メンバー名とエラーメッセージが含まれている()
    {
        // Arrange
        var errorMessage1 = "error message1";
        var errorMessage2 = "error message2";
        var memberNames1 = new string[] { "param1" };
        var memberNames2 = new string[] { "param2", "param3" };
        var validationResults = new List<ValidationResult>
        {
            new(errorMessage1, memberNames1),
            new(errorMessage2, memberNames2),
        };
        var ex = new InvalidParameterException(validationResults);

        // Act
        var message = ex.Message;

        // Assert
        Assert.Equal($"コマンドのパラメーターに入力エラーがあります。パラメーターの入力エラー情報詳細 : [{{\"MemberNames\":[\"{memberNames1[0]}\"],\"ErrorMessage\":\"{errorMessage1}\"}},{{\"MemberNames\":[\"{memberNames2[0]}\",\"{memberNames2[1]}\"],\"ErrorMessage\":\"{errorMessage2}\"}}] 。", message);
    }

    [Fact]
    public void ValidationResults_検証結果のリストを指定しない_空のリスト()
    {
        // Arrange
        var ex = new InvalidParameterException();

        // Act
        var validationResults = ex.ValidationResults;

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void ValidationResults_検証結果のリストを指定_指定したリストを取得できる()
    {
        // Arrange
        string errorMessage1 = "error message1";
        string errorMessage2 = "error message2";
        var results = new List<ValidationResult>
        {
            new(errorMessage1),
            new(errorMessage2),
        };
        var ex = new InvalidParameterException(validationResults: results);

        // Act
        var validationResults = ex.ValidationResults;

        // Assert
        Assert.Collection(
            validationResults,
            result => Assert.Equal(errorMessage1, result.ErrorMessage),
            result => Assert.Equal(errorMessage2, result.ErrorMessage));
    }
}
