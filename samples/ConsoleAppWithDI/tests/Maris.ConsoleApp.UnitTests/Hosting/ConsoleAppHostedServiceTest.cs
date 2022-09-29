using Maris.ConsoleApp.Core;
using Maris.ConsoleApp.Hosting;
using Maris.Testing.Xunit.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Maris.ConsoleApp.UnitTests.Hosting;

public class ConsoleAppHostedServiceTest
{
    private readonly XunitLoggerFactory loggerFactory;

    public ConsoleAppHostedServiceTest(ITestOutputHelper testOutputHelper)
        => this.loggerFactory = XunitLoggerFactory.Create(testOutputHelper);

    public static IEnumerable<object[]> GetContextsAndCommands()
    {
        // パターン1：正常終了するコマンド
        var commandAttribute1 = new CommandAttribute("dummy-command", typeof(SyncCommandImpl));
        var parameter1 = new CommandParameter();
        var context1 = new ConsoleAppContext(commandAttribute1, parameter1);
        var command1 = new SyncCommandImpl(999);

        // パターン2：検証エラーになるコマンド
        var commandAttribute2 = new CommandAttribute("validation-error-command", typeof(ValidationErrorCommand));
        var parameter2 = new CommandParameter();
        var context2 = new ConsoleAppContext(commandAttribute2, parameter2);
        var command2 = new ValidationErrorCommand();

        // パターン3：コマンドの実行エラーになるコマンド
        var commandAttribute3 = new CommandAttribute("error-command", typeof(TestCommand));
        var parameter3 = new CommandParameter();
        var context3 = new ConsoleAppContext(commandAttribute3, parameter3);
        var command3 = new TestCommand();

        return new List<object[]>
        {
            new object[] { context1, command1 },
            new object[] { context2, command2 },
            new object[] { context3, command3 },
        };
    }

    [Fact]
    public void Constructor_lifetimeがnullの場合は例外()
    {
        // Arrange
        IHostApplicationLifetime? lifetime = null;
        ConsoleAppSettings settings = new ConsoleAppSettings();
        var factoryMock = CreateCommandFactoryMock();
        ICommandFactory factory = factoryMock.Object;
        ILogger<CommandExecutor> comandExecutorLogger = this.loggerFactory.CreateLogger<CommandExecutor>();
        CommandExecutor executor = new CommandExecutor(factory, comandExecutorLogger);
        ILogger<ConsoleAppHostedService> logger = this.loggerFactory.CreateLogger<ConsoleAppHostedService>();

        // Act
        var action = () => new ConsoleAppHostedService(lifetime!, settings, executor, logger);

        // Assert
        Assert.Throws<ArgumentNullException>("lifetime", action);
    }

    [Fact]
    public void Constructor_settingsがnullの場合は例外()
    {
        // Arrange
        IHostApplicationLifetime lifetime = Mock.Of<IHostApplicationLifetime>();
        ConsoleAppSettings? settings = null;
        var factoryMock = CreateCommandFactoryMock();
        ICommandFactory factory = factoryMock.Object;
        ILogger<CommandExecutor> comandExecutorLogger = this.loggerFactory.CreateLogger<CommandExecutor>();
        CommandExecutor executor = new CommandExecutor(factory, comandExecutorLogger);
        ILogger<ConsoleAppHostedService> logger = this.loggerFactory.CreateLogger<ConsoleAppHostedService>();

        // Act
        var action = () => new ConsoleAppHostedService(lifetime, settings!, executor, logger);

        // Assert
        Assert.Throws<ArgumentNullException>("settings", action);
    }

    [Fact]
    public void Constructor_executorがnullの場合は例外()
    {
        // Arrange
        IHostApplicationLifetime lifetime = Mock.Of<IHostApplicationLifetime>();
        ConsoleAppSettings settings = new ConsoleAppSettings();
        var factoryMock = CreateCommandFactoryMock();
        ICommandFactory factory = factoryMock.Object;
        ILogger<CommandExecutor> comandExecutorLogger = this.loggerFactory.CreateLogger<CommandExecutor>();
        CommandExecutor? executor = null;
        ILogger<ConsoleAppHostedService> logger = this.loggerFactory.CreateLogger<ConsoleAppHostedService>();

        // Act
        var action = () => new ConsoleAppHostedService(lifetime, settings, executor!, logger);

        // Assert
        Assert.Throws<ArgumentNullException>("executor", action);
    }

    [Fact]
    public void Constructor_loggerがnullの場合は例外()
    {
        // Arrange
        IHostApplicationLifetime lifetime = Mock.Of<IHostApplicationLifetime>();
        ConsoleAppSettings settings = new ConsoleAppSettings();
        var factoryMock = CreateCommandFactoryMock();
        ICommandFactory factory = factoryMock.Object;
        ILogger<CommandExecutor> comandExecutorLogger = this.loggerFactory.CreateLogger<CommandExecutor>();
        CommandExecutor executor = new CommandExecutor(factory, comandExecutorLogger);
        ILogger<ConsoleAppHostedService>? logger = null;

        // Act
        var action = () => new ConsoleAppHostedService(lifetime, settings, executor, logger!);

        // Assert
        Assert.Throws<ArgumentNullException>("logger", action);
    }

    [Fact]
    public async Task StartAsync_コマンドが正常に完了するとコマンドから返却した終了コードが設定される()
    {
        // Arrange
        IHostApplicationLifetime lifetime = Mock.Of<IHostApplicationLifetime>();
        ConsoleAppSettings settings = new ConsoleAppSettings();
        var commandAttribute = new CommandAttribute("dummy-command", typeof(SyncCommandImpl));
        var parameter = new CommandParameter();
        var context = new ConsoleAppContext(commandAttribute, parameter);
        int exitCode = 999;
        var command = new SyncCommandImpl(exitCode);
        command.Initialize(context);
        var factoryMock = CreateCommandFactoryMock(command);
        ICommandFactory factory = factoryMock.Object;
        ILogger<CommandExecutor> comandExecutorLogger = this.loggerFactory.CreateLogger<CommandExecutor>();
        CommandExecutor executor = new CommandExecutor(factory, comandExecutorLogger);
        ILogger<ConsoleAppHostedService> logger = this.loggerFactory.CreateLogger<ConsoleAppHostedService>();
        var service = new ConsoleAppHostedService(lifetime, settings, executor, logger);
        int actualExitCode = 0;
        service.SetExitCode = (exitCode) => actualExitCode = exitCode;
        var cancellationToken = new CancellationToken(false);

        // Act
        await service.StartAsync(cancellationToken);

        // Assert
        Assert.Equal(exitCode, actualExitCode);
    }

    [Fact]
    public async Task StartAsync_コマンドの入力値エラーがあると設定の入力値検証エラーの終了コードが設定される()
    {
        // Arrange
        IHostApplicationLifetime lifetime = Mock.Of<IHostApplicationLifetime>();
        int exitCode = 990;
        ConsoleAppSettings settings = new ConsoleAppSettings { DefaultValidationErrorExitCode = exitCode };
        var commandAttribute = new CommandAttribute("validation-error-command", typeof(ValidationErrorCommand));
        var parameter = new CommandParameter();
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var command = new ValidationErrorCommand();
        command.Initialize(context);
        var factoryMock = CreateCommandFactoryMock(command);
        ICommandFactory factory = factoryMock.Object;
        ILogger<CommandExecutor> comandExecutorLogger = this.loggerFactory.CreateLogger<CommandExecutor>();
        CommandExecutor executor = new CommandExecutor(factory, comandExecutorLogger);
        ILogger<ConsoleAppHostedService> logger = this.loggerFactory.CreateLogger<ConsoleAppHostedService>();
        var service = new ConsoleAppHostedService(lifetime, settings, executor, logger);
        int actualExitCode = 0;
        service.SetExitCode = (exitCode) => actualExitCode = exitCode;
        var cancellationToken = new CancellationToken(false);

        // Act
        await service.StartAsync(cancellationToken);

        // Assert
        Assert.Equal(exitCode, actualExitCode);
    }

    [Fact]
    public async Task StartAsync_コマンドの実行時に例外が発生すると設定のエラーの既定の終了コードが設定される()
    {
        // Arrange
        IHostApplicationLifetime lifetime = Mock.Of<IHostApplicationLifetime>();
        int exitCode = 991;
        ConsoleAppSettings settings = new ConsoleAppSettings { DefaultErrorExitCode = exitCode };
        var commandAttribute = new CommandAttribute("error-command", typeof(TestCommand));
        var parameter = new CommandParameter();
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var command = new TestCommand();
        command.Initialize(context);
        var factoryMock = CreateCommandFactoryMock(command);
        ICommandFactory factory = factoryMock.Object;
        ILogger<CommandExecutor> comandExecutorLogger = this.loggerFactory.CreateLogger<CommandExecutor>();
        CommandExecutor executor = new CommandExecutor(factory, comandExecutorLogger);
        ILogger<ConsoleAppHostedService> logger = this.loggerFactory.CreateLogger<ConsoleAppHostedService>();
        var service = new ConsoleAppHostedService(lifetime, settings, executor, logger);
        int actualExitCode = 0;
        service.SetExitCode = (exitCode) => actualExitCode = exitCode;
        var cancellationToken = new CancellationToken(false);

        // Act
        await service.StartAsync(cancellationToken);

        // Assert
        Assert.Equal(exitCode, actualExitCode);
    }

    [Theory]
    [MemberData(nameof(GetContextsAndCommands))]
    public async Task StartAsync_コマンドが完了するとIHostApplicationLifetimeのStopApplicationが呼び出される(ConsoleAppContext context, CommandBase command)
    {
        // Arrange
        var lifetimeMock = new Mock<IHostApplicationLifetime>();
        IHostApplicationLifetime lifetime = lifetimeMock.Object;
        ConsoleAppSettings settings = new ConsoleAppSettings();
        command.Initialize(context);
        var factoryMock = CreateCommandFactoryMock(command);
        ICommandFactory factory = factoryMock.Object;
        ILogger<CommandExecutor> comandExecutorLogger = this.loggerFactory.CreateLogger<CommandExecutor>();
        CommandExecutor executor = new CommandExecutor(factory, comandExecutorLogger);
        ILogger<ConsoleAppHostedService> logger = this.loggerFactory.CreateLogger<ConsoleAppHostedService>();
        var service = new ConsoleAppHostedService(lifetime, settings, executor, logger);
        int actualExitCode = 0;
        service.SetExitCode = (exitCode) => actualExitCode = exitCode;
        var cancellationToken = new CancellationToken(false);

        // Act
        await service.StartAsync(cancellationToken);

        // Assert
        lifetimeMock.Verify(lifetime => lifetime.StopApplication(), Times.Once);
    }

    [Fact]
    public async Task StopAsync_例外が発生しない()
    {
        // Arrange
        IHostApplicationLifetime lifetime = Mock.Of<IHostApplicationLifetime>();
        ConsoleAppSettings settings = new ConsoleAppSettings();
        var factoryMock = CreateCommandFactoryMock();
        ICommandFactory factory = factoryMock.Object;
        ILogger<CommandExecutor> comandExecutorLogger = this.loggerFactory.CreateLogger<CommandExecutor>();
        CommandExecutor executor = new CommandExecutor(factory, comandExecutorLogger);
        ILogger<ConsoleAppHostedService> logger = this.loggerFactory.CreateLogger<ConsoleAppHostedService>();
        var service = new ConsoleAppHostedService(lifetime, settings, executor, logger);
        var cancellationToken = new CancellationToken(false);

        // Act & Assert 例外が発生しないこと
        await service.StopAsync(cancellationToken);
    }

    private static Mock<ICommandFactory> CreateCommandFactoryMock(CommandBase? creatingCommand = null)
    {
        var command = creatingCommand ?? new TestCommand();
        var mock = new Mock<ICommandFactory>();
        mock.Setup(factory => factory.CreateCommand())
            .Returns(command);
        return mock;
    }

    private class CommandParameter
    {
    }

    private class TestCommand : SyncCommand<CommandParameter>
    {
        protected internal override ICommandResult Execute(CommandParameter parameter)
            => throw new NotImplementedException();
    }

    private class SyncCommandImpl : SyncCommand<CommandParameter>
    {
        private readonly int exitCode;

        internal SyncCommandImpl(int exitCode) => this.exitCode = exitCode;

        protected internal override ICommandResult Execute(CommandParameter parameter)
            => new CommandResult(this.exitCode);
    }

    private class ValidationErrorCommand : SyncCommand<CommandParameter>
    {
        protected internal override ICommandResult Execute(CommandParameter parameter)
            => throw new NotImplementedException();

        protected internal override void ValidateParameter(CommandParameter parameter)
            => throw new ArgumentException("検証エラー", nameof(parameter));
    }

    private class CommandResult : ICommandResult
    {
        internal CommandResult(int exitCode) => this.ExitCode = exitCode;

        public int ExitCode { get; set; }

        public override string ToString() => this.ExitCode.ToString();
    }
}
