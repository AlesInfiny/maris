using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Logging;
using CommandLine;
using Maris.ConsoleApp.Core;
using Microsoft.Extensions.Logging;
using static Maris.ConsoleApp.UnitTests.Core.SyncCommandTest;

namespace Maris.ConsoleApp.UnitTests.Core;

public class CommandExecutorTest
{
    [Fact]
    public void Constructor_factoryがnullの場合は例外()
    {
        // Arrange
        ICommandFactory? factory = null;
        ILogger<CommandExecutor>? logger = null;

        // Act
        var action = () => new CommandExecutor(factory!, logger!);

        // Assert
        Assert.Throws<ArgumentNullException>("factory", action);
    }

    [Fact]
    public void Constructor_ロガーがnullの場合は例外()
    {
        // Arrange
        var factoryMock = new Mock<ICommandFactory>();
        ILogger<CommandExecutor>? logger = null;

        // Act
        var action = () => new CommandExecutor(factoryMock.Object, logger!);

        // Assert
        Assert.Throws<ArgumentNullException>("logger", action);
    }

    [Fact]
    public void Constructor_factoryを使ってコマンドが生成される()
    {
        // Arrange
        var factoryMock = new Mock<ICommandFactory>();
        factoryMock
            .Setup(factory => factory.CreateCommand())
            .Returns(new CommandImpl("dummy-command"));
        var loggerMock = new Mock<ILogger<CommandExecutor>>();

        // Act
        _ = new CommandExecutor(factoryMock.Object, loggerMock.Object);

        // Assert
        factoryMock.Verify(factory => factory.CreateCommand(), Times.Once);
    }

    [Fact]
    public void Constructor_ファクトリーがコマンドを作成できなかった場合は例外()
    {
        // Arrange
        var factoryMock = new Mock<ICommandFactory>();
        factoryMock
            .Setup(factory => factory.CreateCommand())
            .Returns((CommandBase?)null!);
        var loggerMock = new Mock<ILogger<CommandExecutor>>();

        // Act
        var action = () => new CommandExecutor(factoryMock.Object, loggerMock.Object);

        // Assert
        var exception = Assert.Throws<ArgumentException>("factory", action);
        Assert.StartsWith($"{factoryMock.Object.GetType()} クラスの CreateCommand メソッドを呼び出しましたが null を取得しました。", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("dummy-command")]
    public void CommandName_ファクトリーで生成したコマンドオブジェクトの名前を取得できる(string? commandName)
    {
        // Arrange
        var factoryMock = new Mock<ICommandFactory>();
        var command = new CommandImpl(commandName);
        factoryMock
            .Setup(factory => factory.CreateCommand())
            .Returns(command);
        var loggerMock = new Mock<ILogger<CommandExecutor>>();
        var executor = new CommandExecutor(factoryMock.Object, loggerMock.Object);

        // Act
        var actualCommandName = executor.CommandName;

        // Assert
        Assert.Equal(commandName, actualCommandName);
    }

    [Fact]
    public async Task ExecuteCommandAsync_パラメーターの入力検証に失敗した場合は例外()
    {
        // Arrange
        var commandAttribute = new CommandAttribute("sync-command", typeof(SyncCommandImpl));
        var parameter = new CommandParameter { StringParam = "123456" };
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var factoryMock = new Mock<ICommandFactory>();
        var command = new SyncCommandImpl();
        factoryMock
            .Setup(factory => factory.CreateCommand())
            .Returns(command);
        command.Initialize(context);
        var loggerMock = new Mock<ILogger<CommandExecutor>>();
        var executor = new CommandExecutor(factoryMock.Object, loggerMock.Object);
        CancellationToken cancellationToken = new CancellationToken(false);

        // Act
        var action = () => executor.ExecuteCommandAsync(cancellationToken);

        // Assert
        var exception = await Assert.ThrowsAsync<InvalidParameterException>(action);
        Assert.Collection(
            exception.ValidationResults,
            result => Assert.Equal("StringParam は 5 文字以下に設定してください。", result.ErrorMessage));
    }

    [Fact]
    public async Task ExecuteCommandAsync_パラメーターのカスタム入力検証に失敗した場合は例外()
    {
        // Arrange
        var parameter = new ValidatableParameter();
        var context = new ConsoleAppContext(parameter);
        var factoryMock = new Mock<ICommandFactory>();
        var command = new ValidatableCommand();
        factoryMock
            .Setup(factory => factory.CreateCommand())
            .Returns(command);
        command.Initialize(context);
        var loggerMock = new Mock<ILogger<CommandExecutor>>();
        var executor = new CommandExecutor(factoryMock.Object, loggerMock.Object);
        CancellationToken cancellationToken = new CancellationToken(false);

        // Act
        var action = () => executor.ExecuteCommandAsync(cancellationToken);

        // Assert
        var exception = await Assert.ThrowsAsync<InvalidParameterException>(action);
        Assert.Collection(
            exception.ValidationResults,
            result => Assert.Equal("Validate メソッド内で検証", result.ErrorMessage));
    }

    [Fact]
    public async Task ExecuteCommandAsync_コマンド内のカスタム入力値検証に失敗した場合は例外()
    {
        // Arrange
        var commandAttribute = new CommandAttribute("sync-command", typeof(SyncCommandImpl));
        var input = "9999";
        var parameter = new CommandParameter { StringParam = input };
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var factoryMock = new Mock<ICommandFactory>();
        var errorMessage = "9999 は明示的にエラー";
        var command = new SyncCommandImpl((param) =>
        {
            if (param.StringParam == input)
            {
                throw new ArgumentException(errorMessage);
            }
        });
        factoryMock
            .Setup(factory => factory.CreateCommand())
            .Returns(command);
        command.Initialize(context);
        var loggerMock = new Mock<ILogger<CommandExecutor>>();
        var executor = new CommandExecutor(factoryMock.Object, loggerMock.Object);
        CancellationToken cancellationToken = new CancellationToken(false);

        // Act
        var action = () => executor.ExecuteCommandAsync(cancellationToken);

        // Assert
        var exception = await Assert.ThrowsAsync<InvalidParameterException>(action);
        Assert.Equal("コマンドのパラメーターに入力エラーがあります。", exception.Message);
        Assert.Empty(exception.ValidationResults);
        var innerException = Assert.IsType<ArgumentException>(exception.InnerException);
        Assert.Equal(errorMessage, innerException.Message);
    }

    [Fact]
    public async Task ExecuteCommandAsync_SyncCommandの派生コマンドを実行可能()
    {
        // Arrange
        var commandAttribute = new CommandAttribute("sync-command", typeof(SyncCommandImpl));
        var parameter = new CommandParameter { StringParam = "12345" };
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var factoryMock = new Mock<ICommandFactory>();
        var command = new SyncCommandImpl();
        factoryMock
            .Setup(factory => factory.CreateCommand())
            .Returns(command);
        command.Initialize(context);
        var loggerMock = new Mock<ILogger<CommandExecutor>>();
        var executor = new CommandExecutor(factoryMock.Object, loggerMock.Object);
        CancellationToken cancellationToken = new CancellationToken(false);

        // Act
        var exitCode = await executor.ExecuteCommandAsync(cancellationToken);

        // Assert
        Assert.Equal(999, exitCode);
    }

    [Fact]
    public async Task ExecuteCommandAsync_SyncCommandの派生コマンドにパラメーターが引き渡される()
    {
        // Arrange
        var commandAttribute = new CommandAttribute("sync-command", typeof(SyncCommand<object>));
        var parameter = new object();
        var exitCode = 901;
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var factoryMock = new Mock<ICommandFactory>();
        var commandMock = new Mock<SyncCommand<object>>();
        commandMock
            .Setup(command => command.Execute(It.IsAny<object>()))
            .Returns(new TestCommandResult(exitCode));
        var command = commandMock.Object;
        factoryMock
            .Setup(factory => factory.CreateCommand())
            .Returns(command);
        command.Initialize(context);
        var loggerMock = new Mock<ILogger<CommandExecutor>>();
        var executor = new CommandExecutor(factoryMock.Object, loggerMock.Object);
        CancellationToken cancellationToken = new CancellationToken(false);

        // Act
        var actualExitCode = await executor.ExecuteCommandAsync(cancellationToken);

        // Assert
        Assert.Equal(exitCode, actualExitCode);
        commandMock.Verify(command => command.Execute(parameter), Times.Once);
    }

    [Fact]
    public async Task ExecuteCommandAsync_AsyncCommandの派生コマンドを実行可能()
    {
        // Arrange
        var commandAttribute = new CommandAttribute("async-command", typeof(AsyncCommandImpl));
        var parameter = new CommandParameter { StringParam = "12345" };
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var factoryMock = new Mock<ICommandFactory>();
        var command = new AsyncCommandImpl();
        factoryMock
            .Setup(factory => factory.CreateCommand())
            .Returns(command);
        command.Initialize(context);
        var loggerMock = new Mock<ILogger<CommandExecutor>>();
        var executor = new CommandExecutor(factoryMock.Object, loggerMock.Object);
        CancellationToken cancellationToken = new CancellationToken(false);

        // Act
        var exitCode = await executor.ExecuteCommandAsync(cancellationToken);

        // Assert
        Assert.Equal(888, exitCode);
    }

    [Fact]
    public async Task ExecuteCommandAsync_AsyncCommandの派生コマンドにパラメーターとキャンセルトークンが引き渡される()
    {
        // Arrange
        var commandAttribute = new CommandAttribute("async-command", typeof(AsyncCommand<object>));
        var parameter = new object();
        var exitCode = 801;
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var factoryMock = new Mock<ICommandFactory>();
        var commandMock = new Mock<AsyncCommand<object>>();
        commandMock
            .Setup(command => command.ExecuteAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<ICommandResult>(new TestCommandResult(exitCode)));
        var command = commandMock.Object;
        factoryMock
            .Setup(factory => factory.CreateCommand())
            .Returns(command);
        command.Initialize(context);
        var loggerMock = new Mock<ILogger<CommandExecutor>>();
        var executor = new CommandExecutor(factoryMock.Object, loggerMock.Object);
        CancellationToken cancellationToken = new CancellationToken(false);

        // Act
        var actualExitCode = await executor.ExecuteCommandAsync(cancellationToken);

        // Assert
        Assert.Equal(exitCode, actualExitCode);
        commandMock.Verify(command => command.ExecuteAsync(parameter, cancellationToken), Times.Once);
    }

    public class CommandImpl : CommandBase
    {
        private readonly string? commandName;

        public CommandImpl()
            : this("dummy-command")
        {
        }

        internal CommandImpl(string? commandName)
        {
            this.commandName = commandName;
        }

        internal override string? CommandName => this.commandName;

        internal override void ValidateParameter()
        {
        }
    }

    private class CommandParameter
    {
        [Option("string-param")]
        [StringLength(maximumLength: 5, ErrorMessage = "{0} は {1} 文字以下に設定してください。")]
        public string StringParam { get; set; } = string.Empty;
    }

    private class SyncCommandImpl : SyncCommand<CommandParameter>
    {
        private readonly Action<CommandParameter>? validateParameter;

        internal SyncCommandImpl(Action<CommandParameter>? validateParameter = null)
            => this.validateParameter = validateParameter;

        protected internal override ICommandResult Execute(CommandParameter parameter)
            => new TestCommandResult(999);

        protected internal override void ValidateParameter(CommandParameter parameter)
        {
            if (this.validateParameter is not null)
            {
                this.validateParameter(parameter);
            }
        }
    }

    [Command("validatable-command", typeof(ValidatableCommand))]
    private class ValidatableParameter : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return new ValidationResult("Validate メソッド内で検証", new string[] { "dummy-member-name" });
        }
    }

    private class ValidatableCommand : SyncCommand<ValidatableParameter>
    {
        protected internal override ICommandResult Execute(ValidatableParameter parameter)
            => new TestCommandResult(0);
    }

    private class AsyncCommandImpl : AsyncCommand<CommandParameter>
    {
        protected internal override Task<ICommandResult> ExecuteAsync(CommandParameter parameter, CancellationToken cancellationToken)
            => Task.FromResult<ICommandResult>(new TestCommandResult(888));
    }

    private class TestCommandResult : ICommandResult
    {
        internal TestCommandResult(int exitCode) => this.ExitCode = exitCode;

        public int ExitCode { get; }
    }
}
