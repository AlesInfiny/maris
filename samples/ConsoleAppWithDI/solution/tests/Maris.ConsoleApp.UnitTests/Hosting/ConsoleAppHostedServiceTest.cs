using Maris.ConsoleApp.Core;
using Maris.ConsoleApp.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;

namespace Maris.ConsoleApp.UnitTests.Hosting;

public class ConsoleAppHostedServiceTest(ITestOutputHelper testOutputHelper) : TestBase(testOutputHelper)
{
    public static TheoryData<ConsoleAppContext, CommandBase> GetContextsAndCommands()
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

        var data = new TheoryData<ConsoleAppContext, CommandBase>
        {
            { context1, command1 },
            { context2, command2 },
            { context3, command3 },
        };
        return data;
    }

    [Fact]
    public void Constructor_lifetimeがnull_ArgumentNullExceptionが発生する()
    {
        // Arrange
        IHostApplicationLifetime? lifetime = null;
        var settings = new ConsoleAppSettings();
        var managerMock = CreateCommandManagerMock();
        var manager = managerMock.Object;
        var commandExecutorLogger = this.CreateTestLogger<CommandExecutor>();
        var executor = new CommandExecutor(manager, commandExecutorLogger);
        var logger = this.CreateTestLogger<ConsoleAppHostedService>();

        // Act
        var action = () => new ConsoleAppHostedService(lifetime!, settings, executor, logger);

        // Assert
        Assert.Throws<ArgumentNullException>("lifetime", action);
    }

    [Fact]
    public void Constructor_settingsがnull_ArgumentNullExceptionが発生する()
    {
        // Arrange
        var lifetime = Mock.Of<IHostApplicationLifetime>();
        ConsoleAppSettings? settings = null;
        var managerMock = CreateCommandManagerMock();
        var manager = managerMock.Object;
        var commandExecutorLogger = this.CreateTestLogger<CommandExecutor>();
        var executor = new CommandExecutor(manager, commandExecutorLogger);
        var logger = this.CreateTestLogger<ConsoleAppHostedService>();

        // Act
        var action = () => new ConsoleAppHostedService(lifetime, settings!, executor, logger);

        // Assert
        Assert.Throws<ArgumentNullException>("settings", action);
    }

    [Fact]
    public void Constructor_executorがnull_ArgumentNullExceptionが発生する()
    {
        // Arrange
        var lifetime = Mock.Of<IHostApplicationLifetime>();
        var settings = new ConsoleAppSettings();
        CommandExecutor? executor = null;
        var logger = this.CreateTestLogger<ConsoleAppHostedService>();

        // Act
        var action = () => new ConsoleAppHostedService(lifetime, settings, executor!, logger);

        // Assert
        Assert.Throws<ArgumentNullException>("executor", action);
    }

    [Fact]
    public void Constructor_loggerがnull_ArgumentNullExceptionが発生する()
    {
        // Arrange
        var lifetime = Mock.Of<IHostApplicationLifetime>();
        var settings = new ConsoleAppSettings();
        var managerMock = CreateCommandManagerMock();
        var manager = managerMock.Object;
        var commandExecutorLogger = this.CreateTestLogger<CommandExecutor>();
        var executor = new CommandExecutor(manager, commandExecutorLogger);
        ILogger<ConsoleAppHostedService>? logger = null;

        // Act
        var action = () => new ConsoleAppHostedService(lifetime, settings, executor, logger!);

        // Assert
        Assert.Throws<ArgumentNullException>("logger", action);
    }

    [Fact]
    public async Task StartAsync_コマンドが正常に完了する_コマンドから返却した終了コードが設定される()
    {
        // Arrange
        var lifetime = Mock.Of<IHostApplicationLifetime>();
        var settings = new ConsoleAppSettings();
        var commandAttribute = new CommandAttribute("dummy-command", typeof(SyncCommandImpl));
        var parameter = new CommandParameter();
        var context = new ConsoleAppContext(commandAttribute, parameter);
        int exitCode = 999;
        var command = new SyncCommandImpl(exitCode);
        command.Initialize(context);
        var managerMock = CreateCommandManagerMock(command);
        var manager = managerMock.Object;
        var commandExecutorLogger = this.CreateTestLogger<CommandExecutor>();
        var executor = new CommandExecutor(manager, commandExecutorLogger);
        var logger = this.CreateTestLogger<ConsoleAppHostedService>();
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
    public async Task StartAsync_コマンドの入力値エラーがある_入力値検証エラーの終了コードが設定される()
    {
        // Arrange
        var lifetime = Mock.Of<IHostApplicationLifetime>();
        int exitCode = 990;
        var settings = new ConsoleAppSettings { DefaultValidationErrorExitCode = exitCode };
        var commandAttribute = new CommandAttribute("validation-error-command", typeof(ValidationErrorCommand));
        var parameter = new CommandParameter();
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var command = new ValidationErrorCommand();
        command.Initialize(context);
        var managerMock = CreateCommandManagerMock(command);
        var manager = managerMock.Object;
        var commandExecutorLogger = this.CreateTestLogger<CommandExecutor>();
        var executor = new CommandExecutor(manager, commandExecutorLogger);
        var logger = this.CreateTestLogger<ConsoleAppHostedService>();
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
    public async Task StartAsync_コマンドの実行時に例外が発生する_既定のエラー終了コードが設定される()
    {
        // Arrange
        var lifetime = Mock.Of<IHostApplicationLifetime>();
        int exitCode = 991;
        var settings = new ConsoleAppSettings { DefaultErrorExitCode = exitCode };
        var commandAttribute = new CommandAttribute("error-command", typeof(TestCommand));
        var parameter = new CommandParameter();
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var command = new TestCommand();
        command.Initialize(context);
        var managerMock = CreateCommandManagerMock(command);
        var manager = managerMock.Object;
        var commandExecutorLogger = this.CreateTestLogger<CommandExecutor>();
        var executor = new CommandExecutor(manager, commandExecutorLogger);
        var logger = this.CreateTestLogger<ConsoleAppHostedService>();
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
    public async Task StartAsync_コマンドが完了_IHostApplicationLifetimeのStopApplicationが1回呼び出される(ConsoleAppContext context, CommandBase command)
    {
        // Arrange
        var lifetimeMock = new Mock<IHostApplicationLifetime>();
        var lifetime = lifetimeMock.Object;
        var settings = new ConsoleAppSettings();
        command.Initialize(context);
        var managerMock = CreateCommandManagerMock(command);
        var manager = managerMock.Object;
        var commandExecutorLogger = this.CreateTestLogger<CommandExecutor>();
        var executor = new CommandExecutor(manager, commandExecutorLogger);
        var logger = this.CreateTestLogger<ConsoleAppHostedService>();
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
    public async Task StartAsync_コマンドの開始について情報レベルでログに記録される()
    {
        // Arrange
        var lifetime = Mock.Of<IHostApplicationLifetime>();
        var settings = new ConsoleAppSettings();
        var commandAttribute = new CommandAttribute("dummy-command", typeof(SyncCommandImpl));
        var parameter = new CommandParameter();
        var context = new ConsoleAppContext(commandAttribute, parameter);
        int exitCode = 0;
        var command = new SyncCommandImpl(exitCode);
        command.Initialize(context);
        var managerMock = CreateCommandManagerMock(command);
        var manager = managerMock.Object;
        var commandExecutorLogger = this.CreateTestLogger<CommandExecutor>();
        var executor = new CommandExecutor(manager, commandExecutorLogger);
        var logger = this.CreateTestLogger<ConsoleAppHostedService>();
        var service = new ConsoleAppHostedService(lifetime, settings, executor, logger)
        {
            SetExitCode = exitCode => { },
        };
        var cancellationToken = new CancellationToken(false);

        // Act
        await service.StartAsync(cancellationToken);

        // Assert
        var record = this.LogCollector.GetSnapshot()
            .Single(log => log.Level == LogLevel.Information);
        Assert.Equal(1101, record.Id);
        Assert.Equal("dummy-command コマンドのホストの処理を開始します。", record.Message);
    }

    [Fact]
    public async Task StartAsync_パラメーターの検証エラー_エラーの情報がエラーログに記録される()
    {
        // Arrange
        var lifetime = Mock.Of<IHostApplicationLifetime>();
        var settings = new ConsoleAppSettings();
        var commandAttribute = new CommandAttribute("validation-error-command", typeof(ValidationErrorCommand));
        var parameter = new CommandParameter();
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var command = new ValidationErrorCommand();
        command.Initialize(context);
        var managerMock = CreateCommandManagerMock(command);
        var manager = managerMock.Object;
        var commandExecutorLogger = this.CreateTestLogger<CommandExecutor>();
        var executor = new CommandExecutor(manager, commandExecutorLogger);
        var logger = this.CreateTestLogger<ConsoleAppHostedService>();
        var service = new ConsoleAppHostedService(lifetime, settings, executor, logger)
        {
            SetExitCode = exitCode => { },
        };
        var cancellationToken = new CancellationToken(false);

        // Act
        await service.StartAsync(cancellationToken);

        // Assert
        var record = this.LogCollector.GetSnapshot()
            .Single(log => log.Level == LogLevel.Error);
        Assert.Equal(1102, record.Id);
        Assert.Equal("validation-error-command コマンドの実行時に例外が発生し、処理が失敗しました。", record.Message);
        var exception = Assert.IsType<InvalidParameterException>(record.Exception);
        Assert.Equal("コマンドのパラメーターに入力エラーがあります。", exception.Message);
        var innerException = Assert.IsType<ArgumentException>(exception.InnerException);
        Assert.StartsWith("検証エラー", innerException.Message);
    }

    [Fact]
    public async Task StartAsync_コマンドの実行エラー_エラーの情報がエラーログに記録される()
    {
        // Arrange
        var lifetime = Mock.Of<IHostApplicationLifetime>();
        var settings = new ConsoleAppSettings();
        var commandAttribute = new CommandAttribute("error-command", typeof(TestCommand));
        var parameter = new CommandParameter();
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var command = new TestCommand();
        command.Initialize(context);
        var managerMock = CreateCommandManagerMock(command);
        var manager = managerMock.Object;
        var commandExecutorLogger = this.CreateTestLogger<CommandExecutor>();
        var executor = new CommandExecutor(manager, commandExecutorLogger);
        var logger = this.CreateTestLogger<ConsoleAppHostedService>();
        var service = new ConsoleAppHostedService(lifetime, settings, executor, logger)
        {
            SetExitCode = exitCode => { },
        };
        var cancellationToken = new CancellationToken(false);

        // Act
        await service.StartAsync(cancellationToken);

        // Assert
        var record = this.LogCollector.GetSnapshot()
            .Single(log => log.Level == LogLevel.Error);
        Assert.Equal(1103, record.Id);
        Assert.Equal("error-command コマンドの実行時に例外が発生し、処理が失敗しました。", record.Message);
        var exception = Assert.IsType<NotImplementedException>(record.Exception);
    }

    [Fact]
    public async Task StopAsync_コマンドの実行前に呼び出す_終了コード0のホスト終了ログが情報レベルで出力される()
    {
        // Arrange
        var lifetime = Mock.Of<IHostApplicationLifetime>();
        var settings = new ConsoleAppSettings();
        var commandAttribute = new CommandAttribute("sync-command", typeof(SyncCommandImpl));
        var parameter = new CommandParameter();
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var exitCode = 456;
        var command = new SyncCommandImpl(exitCode);
        command.Initialize(context);
        var managerMock = CreateCommandManagerMock(command);
        var manager = managerMock.Object;
        var commandExecutorLogger = this.CreateTestLogger<CommandExecutor>();
        var executor = new CommandExecutor(manager, commandExecutorLogger);
        var logger = this.CreateTestLogger<ConsoleAppHostedService>();
        var service = new ConsoleAppHostedService(lifetime, settings, executor, logger);
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        await service.StopAsync(cancellationToken);

        // Assert
        var record = this.LogCollector.LatestRecord;
        Assert.Equal(LogLevel.Information, record.Level);
        Assert.Equal(1104, record.Id);
        Assert.StartsWith($"sync-command コマンドのホストの処理が終了コード 0 で完了しました。", record.Message);
    }

    [Fact]
    public async Task StopAsync_コマンドの実行完了後に呼び出す_コマンドの返却した終了コードがホスト終了ログに出力される()
    {
        // Arrange
        var lifetime = Mock.Of<IHostApplicationLifetime>();
        var settings = new ConsoleAppSettings();
        var commandAttribute = new CommandAttribute("sync-command", typeof(SyncCommandImpl));
        var parameter = new CommandParameter();
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var exitCode = 456;
        var command = new SyncCommandImpl(exitCode);
        command.Initialize(context);
        var managerMock = CreateCommandManagerMock(command);
        var manager = managerMock.Object;
        var commandExecutorLogger = this.CreateTestLogger<CommandExecutor>();
        var executor = new CommandExecutor(manager, commandExecutorLogger);
        var logger = this.CreateTestLogger<ConsoleAppHostedService>();
        var service = new ConsoleAppHostedService(lifetime, settings, executor, logger);
        var cancellationToken = new CancellationToken(false);
        await service.StartAsync(cancellationToken);

        // Act
        await service.StopAsync(cancellationToken);

        // Assert
        var record = this.LogCollector.LatestRecord;
        Assert.Equal(LogLevel.Information, record.Level);
        Assert.Equal(1104, record.Id);
        Assert.StartsWith($"sync-command コマンドのホストの処理が終了コード {exitCode} で完了しました。", record.Message);
    }

    [Fact]
    public async Task StopAsync_コマンドの実行時間がログに出力される()
    {
        // Arrange
        var lifetime = Mock.Of<IHostApplicationLifetime>();
        var settings = new ConsoleAppSettings();
        var commandAttribute = new CommandAttribute("sync-command", typeof(SyncCommandImpl));
        var parameter = new CommandParameter();
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var exitCode = 456;
        var command = new SyncCommandImpl(exitCode);
        command.Initialize(context);
        var managerMock = CreateCommandManagerMock(command);
        var manager = managerMock.Object;
        var commandExecutorLogger = this.CreateTestLogger<CommandExecutor>();
        var executor = new CommandExecutor(manager, commandExecutorLogger);
        var logger = this.CreateTestLogger<ConsoleAppHostedService>();
        var fakeTimeProvider = new FakeTimeProvider();
        var service = new ConsoleAppHostedService(lifetime, settings, executor, logger, fakeTimeProvider);
        var cancellationToken = new CancellationToken(false);
        await service.StartAsync(cancellationToken);

        // Act
        fakeTimeProvider.Advance(TimeSpan.FromMilliseconds(1000));
        await service.StopAsync(cancellationToken);

        // Assert
        var record = this.LogCollector.LatestRecord;
        Assert.Contains($"実行時間は 1000 ms でした。", record.Message);
    }

    private static Mock<ICommandManager> CreateCommandManagerMock(CommandBase? creatingCommand = null)
    {
        var command = creatingCommand ?? new TestCommand();
        var mock = new Mock<ICommandManager>();
        mock.Setup(manager => manager.CreateCommand())
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
