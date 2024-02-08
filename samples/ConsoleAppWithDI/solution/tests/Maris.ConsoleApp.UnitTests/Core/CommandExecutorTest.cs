using System.ComponentModel.DataAnnotations;
using CommandLine;
using Maris.ConsoleApp.Core;
using Microsoft.Extensions.Logging;

namespace Maris.ConsoleApp.UnitTests.Core;

public class CommandExecutorTest
{
    [Fact]
    public void Constructor_managerがnull_ArgumentNullExceptionが発生する()
    {
        // Arrange
        ICommandManager? manager = null;
        ILogger<CommandExecutor>? logger = null;

        // Act
        var action = () => new CommandExecutor(manager!, logger!);

        // Assert
        Assert.Throws<ArgumentNullException>("commandManager", action);
    }

    [Fact]
    public void Constructor_ロガーがnullの場合_ArgumentNullExceptionが発生する()
    {
        // Arrange
        var managerMock = new Mock<ICommandManager>();
        ILogger<CommandExecutor>? logger = null;

        // Act
        var action = () => new CommandExecutor(managerMock.Object, logger!);

        // Assert
        Assert.Throws<ArgumentNullException>("logger", action);
    }

    [Fact]
    public void Constructor_ICommandManagerのCreateCommandが1回呼び出される()
    {
        // Arrange
        var managerMock = new Mock<ICommandManager>();
        managerMock
            .Setup(manager => manager.CreateCommand())
            .Returns(new CommandImpl("dummy-command"));
        var loggerMock = new Mock<ILogger<CommandExecutor>>();

        // Act
        _ = new CommandExecutor(managerMock.Object, loggerMock.Object);

        // Assert
        managerMock.Verify(manager => manager.CreateCommand(), Times.Once);
    }

    [Fact]
    public void Constructor_ICommandManagerがコマンドを作成できない_ArgumentExceptionが発生する()
    {
        // Arrange
        var managerMock = new Mock<ICommandManager>();
        managerMock
            .Setup(manager => manager.CreateCommand())
            .Returns((CommandBase?)null!);
        var loggerMock = new Mock<ILogger<CommandExecutor>>();

        // Act
        var action = () => new CommandExecutor(managerMock.Object, loggerMock.Object);

        // Assert
        var exception = Assert.Throws<ArgumentException>("commandManager", action);
        Assert.StartsWith($"{managerMock.Object.GetType()} クラスの CreateCommand メソッドを呼び出しましたが null を取得しました。", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("dummy-command")]
    public void CommandName_ICommandManagerで生成したコマンドオブジェクトの名前を取得できる(string? commandName)
    {
        // Arrange
        var managerMock = new Mock<ICommandManager>();
        var command = new CommandImpl(commandName);
        managerMock
            .Setup(manager => manager.CreateCommand())
            .Returns(command);
        var loggerMock = new Mock<ILogger<CommandExecutor>>();
        var executor = new CommandExecutor(managerMock.Object, loggerMock.Object);

        // Act
        var actualCommandName = executor.CommandName;

        // Assert
        Assert.Equal(commandName, actualCommandName);
    }

    [Fact]
    public async Task ExecuteCommandAsync_パラメーターの入力検証に失敗した_InvalidParameterExceptionが発生する()
    {
        // Arrange
        var commandAttribute = new CommandAttribute("sync-command", typeof(SyncCommandImpl));
        var parameter = new CommandParameter { StringParam = "123456" };
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var managerMock = new Mock<ICommandManager>();
        var command = new SyncCommandImpl();
        managerMock
            .Setup(manager => manager.CreateCommand())
            .Returns(command);
        command.Initialize(context);
        var loggerMock = new Mock<ILogger<CommandExecutor>>();
        var executor = new CommandExecutor(managerMock.Object, loggerMock.Object);
        var cancellationToken = new CancellationToken(false);

        // Act
        var action = () => executor.ExecuteCommandAsync(cancellationToken);

        // Assert
        var exception = await Assert.ThrowsAsync<InvalidParameterException>(action);
        Assert.Single(exception.ValidationResults, result => result.ErrorMessage == "StringParam は 5 文字以下に設定してください。");
    }

    [Fact]
    public async Task ExecuteCommandAsync_パラメーターのカスタム入力検証に失敗した_InvalidParameterExceptionが発生する()
    {
        // Arrange
        var parameter = new ValidatableParameter();
        var context = new ConsoleAppContext(parameter);
        var managerMock = new Mock<ICommandManager>();
        var command = new ValidatableCommand();
        managerMock
            .Setup(manager => manager.CreateCommand())
            .Returns(command);
        command.Initialize(context);
        var loggerMock = new Mock<ILogger<CommandExecutor>>();
        var executor = new CommandExecutor(managerMock.Object, loggerMock.Object);
        var cancellationToken = new CancellationToken(false);

        // Act
        var action = () => executor.ExecuteCommandAsync(cancellationToken);

        // Assert
        var exception = await Assert.ThrowsAsync<InvalidParameterException>(action);
        Assert.Single(exception.ValidationResults, result => result.ErrorMessage == "Validate メソッド内で検証");
    }

    [Fact]
    public async Task ExecuteCommandAsync_コマンド内のカスタム入力値検証に失敗した_InvalidParameterExceptionが発生する()
    {
        // Arrange
        var commandAttribute = new CommandAttribute("sync-command", typeof(SyncCommandImpl));
        var input = "9999";
        var parameter = new CommandParameter { StringParam = input };
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var managerMock = new Mock<ICommandManager>();
        var errorMessage = "9999 は明示的にエラー";
        var command = new SyncCommandImpl((param) =>
        {
            if (param.StringParam == input)
            {
                throw new ArgumentException(errorMessage);
            }
        });
        managerMock
            .Setup(manager => manager.CreateCommand())
            .Returns(command);
        command.Initialize(context);
        var loggerMock = new Mock<ILogger<CommandExecutor>>();
        var executor = new CommandExecutor(managerMock.Object, loggerMock.Object);
        var cancellationToken = new CancellationToken(false);

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
    public async Task ExecuteCommandAsync_SyncCommandの派生コマンドを実行可能_コマンドの戻り値を取得できる()
    {
        // Arrange
        var commandAttribute = new CommandAttribute("sync-command", typeof(SyncCommandImpl));
        var parameter = new CommandParameter { StringParam = "12345" };
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var managerMock = new Mock<ICommandManager>();
        var command = new SyncCommandImpl();
        managerMock
            .Setup(manager => manager.CreateCommand())
            .Returns(command);
        command.Initialize(context);
        var loggerMock = new Mock<ILogger<CommandExecutor>>();
        var executor = new CommandExecutor(managerMock.Object, loggerMock.Object);
        var cancellationToken = new CancellationToken(false);

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
        var managerMock = new Mock<ICommandManager>();
        var commandMock = new Mock<SyncCommand<object>>();
        commandMock
            .Setup(command => command.Execute(It.IsAny<object>()))
            .Returns(new TestCommandResult(exitCode));
        var command = commandMock.Object;
        managerMock
            .Setup(manager => manager.CreateCommand())
            .Returns(command);
        command.Initialize(context);
        var loggerMock = new Mock<ILogger<CommandExecutor>>();
        var executor = new CommandExecutor(managerMock.Object, loggerMock.Object);
        var cancellationToken = new CancellationToken(false);

        // Act
        var actualExitCode = await executor.ExecuteCommandAsync(cancellationToken);

        // Assert
        Assert.Equal(exitCode, actualExitCode);
        commandMock.Verify(command => command.Execute(parameter), Times.Once);
    }

    [Fact]
    public async Task ExecuteCommandAsync_AsyncCommandの派生コマンドを実行可能_コマンドの戻り値を取得できる()
    {
        // Arrange
        var commandAttribute = new CommandAttribute("async-command", typeof(AsyncCommandImpl));
        var parameter = new CommandParameter { StringParam = "12345" };
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var managerMock = new Mock<ICommandManager>();
        var command = new AsyncCommandImpl();
        managerMock
            .Setup(manager => manager.CreateCommand())
            .Returns(command);
        command.Initialize(context);
        var loggerMock = new Mock<ILogger<CommandExecutor>>();
        var executor = new CommandExecutor(managerMock.Object, loggerMock.Object);
        var cancellationToken = new CancellationToken(false);

        // Act
        var exitCode = await executor.ExecuteCommandAsync(cancellationToken);

        // Assert
        Assert.Equal(888, exitCode);
    }

    [Fact]
    public async Task ExecuteCommandAsync_AsyncCommandの派生コマンドのExecuteAsyncがパラメーターとキャンセルトークンを伴い1回呼び出される()
    {
        // Arrange
        var commandAttribute = new CommandAttribute("async-command", typeof(AsyncCommand<object>));
        var parameter = new object();
        var exitCode = 801;
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var managerMock = new Mock<ICommandManager>();
        var commandMock = new Mock<AsyncCommand<object>>();
        commandMock
            .Setup(command => command.ExecuteAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<ICommandResult>(new TestCommandResult(exitCode)));
        var command = commandMock.Object;
        managerMock
            .Setup(manager => manager.CreateCommand())
            .Returns(command);
        command.Initialize(context);
        var loggerMock = new Mock<ILogger<CommandExecutor>>();
        var executor = new CommandExecutor(managerMock.Object, loggerMock.Object);
        var cancellationToken = new CancellationToken(false);

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

        internal CommandImpl(string? commandName) => this.commandName = commandName;

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
            yield return new ValidationResult("Validate メソッド内で検証", ["dummy-member-name"]);
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
