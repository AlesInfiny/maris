using System.ComponentModel.DataAnnotations;
using CommandLine;
using Maris.ConsoleApp.Core;

namespace Maris.ConsoleApp.UnitTests.Core;

public class CommandBaseTest
{
    [Fact]
    public void CommandName_初期値はnull()
    {
        // Arrange
        var command = new CommandImpl();

        // Act
        var commandName = command.CommandName;

        // Assert
        Assert.Null(commandName);
    }

    [Fact]
    public void CommandName_初期化時に指定したコマンド名を取得できる()
    {
        // Arrange
        var command = new CommandImpl();
        string commandName = "dummy-command";
        var commandAttribute = new CommandAttribute(commandName, typeof(CommandImpl));
        var context = new ConsoleAppContext(commandAttribute, new object());
        command.Initialize(context);

        // Act
        var actualCommandName = command.CommandName;

        // Assert
        Assert.Equal(commandName, actualCommandName);
    }

    [Fact]
    public void Context_初期値のまま取得する_InvalidOperationExceptionが発生する()
    {
        // Arrange
        var command = new CommandImpl();

        // Act
        var action = () => command.Context;

        // Assert
        var ex = Assert.Throws<InvalidOperationException>(action);
        Assert.Equal("Context が初期化されていません。", ex.Message);
    }

    [Fact]
    public void Context_初期化時に指定したコンテキストを取得できる()
    {
        // Arrange
        var command = new CommandImpl();
        string commandName = "dummy-command";
        var commandAttribute = new CommandAttribute(commandName, typeof(CommandImpl));
        var context = new ConsoleAppContext(commandAttribute, new object());
        command.Initialize(context);

        // Act
        var actualContext = command.Context;

        // Assert
        Assert.Same(context, actualContext);
    }

    [Fact]
    public void Initialize_nullで初期化する_ArgumentNullExceptionが発生する()
    {
        // Arrange
        var command = new CommandImpl();
        ConsoleAppContext? context = null;

        // Act
        var action = () => command.Initialize(context!);

        // Assert
        Assert.Throws<ArgumentNullException>("context", action);
    }

    [Fact]
    public void ValidateAllParameter_入力パラメータのクラスにプロパティが定義されていない_例外が発生しない()
    {
        // Arrange
        var command = new CommandImpl();
        string commandName = "dummy-command";
        var commandAttribute = new CommandAttribute(commandName, typeof(CommandImpl));
        var parameter = new object();
        var context = new ConsoleAppContext(commandAttribute, parameter);
        command.Initialize(context);

        // Act and Assert(例外が発生しないこと)
        command.ValidateAllParameter();
    }

    [Fact]
    public void ValidateAllParameter_入力パラメータのクラスにプロパティがあるが検証属性が定義されていない_例外が発生しない()
    {
        // Arrange
        var command = new CommandImpl();
        string commandName = "dummy-command";
        var commandAttribute = new CommandAttribute(commandName, typeof(CommandImpl));
        var parameter = new NoDataAnnotationParameter();
        var context = new ConsoleAppContext(commandAttribute, parameter);
        command.Initialize(context);

        // Act and Assert(例外が発生しないこと)
        command.ValidateAllParameter();
    }

    [Theory]
    [InlineData("", 0)]
    [InlineData("1234567890", 5)]
    public void ValidateAllParameter_入力パラメータのクラスに検証属性を定義したプロパティがあり検証に成功する値が設定されている_例外が発生しない(string param1, int param2)
    {
        // Arrange
        var command = new CommandImpl();
        string commandName = "dummy-command";
        var commandAttribute = new CommandAttribute(commandName, typeof(CommandImpl));
        var parameter = new TestParameter
        {
            Param1 = param1,
            Param2 = param2,
        };
        var context = new ConsoleAppContext(commandAttribute, parameter);
        command.Initialize(context);

        // Act and Assert(例外が発生しないこと)
        command.ValidateAllParameter();
    }

    [Fact]
    public void ValidateAllParameter_入力パラメータのクラスに検証属性を定義したプロパティがあり一部検証に失敗する値が設定されている_検証失敗メッセージを伴うInvalidParameterExceptionが発生する()
    {
        // Arrange
        var command = new CommandImpl();
        string commandName = "dummy-command";
        var commandAttribute = new CommandAttribute(commandName, typeof(CommandImpl));
        string param1 = "1234567890"; // 検証成功するデータ。
        int param2 = -1; // 検証失敗するデータ。

        var parameter = new TestParameter
        {
            Param1 = param1,
            Param2 = param2,
        };
        var context = new ConsoleAppContext(commandAttribute, parameter);
        command.Initialize(context);

        // Act
        var action = () => command.ValidateAllParameter();

        // Assert
        var ex = Assert.Throws<InvalidParameterException>(action);
        Assert.Single(ex.ValidationResults, result => result.ErrorMessage == "Param2 は 0 から 5 の間で設定してください。");
    }

    [Fact]
    public void ValidateAllParameter_入力パラメータのクラスに検証属性を定義したプロパティがあり複数検証に失敗する値が設定されている_検証失敗メッセージを伴うInvalidParameterExceptionが発生する()
    {
        // Arrange
        var command = new CommandImpl();
        string commandName = "dummy-command";
        var commandAttribute = new CommandAttribute(commandName, typeof(CommandImpl));
        string param1 = "12345678901"; // 検証失敗するデータ。
        int param2 = 6; // 検証失敗するデータ。

        var parameter = new TestParameter
        {
            Param1 = param1,
            Param2 = param2,
        };
        var context = new ConsoleAppContext(commandAttribute, parameter);
        command.Initialize(context);

        // Act
        var action = () => command.ValidateAllParameter();

        // Assert
        var ex = Assert.Throws<InvalidParameterException>(action);
        Assert.Collection(
            ex.ValidationResults,
            result => Assert.Equal("Param1 は 10 文字以下に設定してください。", result.ErrorMessage),
            result => Assert.Equal("Param2 は 0 から 5 の間で設定してください。", result.ErrorMessage));
    }

    [Fact]
    public void ValidateAllParameter_コマンドクラスのロジック内でパラメーターの入力値検証エラーがあった_InvalidParameterExceptionが発生する()
    {
        // Arrange
        var command = new ValidationErrorCommand();
        string commandName = "validation-error-command";
        var commandAttribute = new CommandAttribute(commandName, typeof(ValidationErrorCommand));
        var context = new ConsoleAppContext(commandAttribute, new object());
        command.Initialize(context);

        // Act
        var action = () => command.ValidateAllParameter();

        // Assert
        var ex = Assert.Throws<InvalidParameterException>(action);
        Assert.Equal("コマンドのパラメーターに入力エラーがあります。", ex.Message);
        Assert.Empty(ex.ValidationResults);
        var innerException = Assert.IsType<InvalidOperationException>(ex.InnerException);
        Assert.Equal("パラメーター検証エラーの動作確認", innerException.Message);
    }

    private class CommandImpl : CommandBase, ISyncCommand
    {
        public ICommandResult Execute() => CommandResult.Success;

        internal override void ValidateParameter()
        {
        }
    }

    private class ValidationErrorCommand : CommandBase, ISyncCommand
    {
        public ICommandResult Execute() => CommandResult.Success;

        internal override void ValidateParameter()
        {
            throw new InvalidOperationException("パラメーター検証エラーの動作確認");
        }
    }

    private class CommandResult : ICommandResult
    {
        public int ExitCode => 0;

        internal static ICommandResult Success => new CommandResult();
    }

    private class NoDataAnnotationParameter
    {
        [Option("param1")]
        public string Param1 { get; set; } = string.Empty;
    }

    private class TestParameter
    {
        [Option("param1")]
        [StringLength(maximumLength: 10, ErrorMessage = "{0} は {1} 文字以下に設定してください。")]
        public string Param1 { get; set; } = string.Empty;

        [Option("param2")]
        [Range(minimum: 0, maximum: 5, ErrorMessage = "{0} は {1} から {2} の間で設定してください。")]
        public int Param2 { get; set; } = 0;
    }
}
