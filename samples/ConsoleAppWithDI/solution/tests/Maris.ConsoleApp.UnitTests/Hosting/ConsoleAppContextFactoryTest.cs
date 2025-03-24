using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CommandLine;
using Maris.ConsoleApp.Core;
using Maris.ConsoleApp.Hosting;
using Microsoft.Extensions.Logging;

namespace Maris.ConsoleApp.UnitTests.Hosting;

public class ConsoleAppContextFactoryTest(ITestOutputHelper testOutputHelper) : TestBase(testOutputHelper)
{
    [Fact]
    public void Constructor_IApplicationProcessがnull_ArgumentNullExceptionが発生する()
    {
        // Arrange
        IApplicationProcess? appProcess = null;
        var settings = new ConsoleAppSettings();
        var logger = this.CreateTestLogger<ConsoleAppContextFactory>();

        // Act
        var action = () => new ConsoleAppContextFactory(appProcess!, settings, logger);

        // Assert
        Assert.Throws<ArgumentNullException>("appProcess", action);
    }

    [Fact]
    public void Constructor_ConsoleAppSettingsがnull_ArgumentNullExceptionが発生する()
    {
        // Arrange
        var appProcess = new TestApplicationProcess();
        ConsoleAppSettings? settings = null;
        var logger = this.CreateTestLogger<ConsoleAppContextFactory>();

        // Act
        var action = () => new ConsoleAppContextFactory(appProcess, settings!, logger);

        // Assert
        Assert.Throws<ArgumentNullException>("settings", action);
    }

    [Fact]
    public void Constructor_ILoggerがnull_ArgumentNullExceptionが発生する()
    {
        // Arrange
        var appProcess = new TestApplicationProcess();
        var settings = new ConsoleAppSettings();
        ILogger<ConsoleAppContextFactory>? logger = null;

        // Act
        var action = () => new ConsoleAppContextFactory(appProcess, settings, logger!);

        // Assert
        Assert.Throws<ArgumentNullException>("logger", action);
    }

    [Fact]
    public void CreateConsoleAppContext_起動パラメーターが情報レベルでログに出力される()
    {
        // Arrange
        var args = new string[] { "console-app-context-factory-test", "--category-id", "123" };
        var types = new Type[] { typeof(TestParameter) };
        var assembly = new TestAssembly(types);
        Action<CommandParameterTypeCollection>? commandParametersOption = collection => collection.AddCommandParameterTypeFrom(assembly);
        var appProcess = new TestApplicationProcess();
        var settings = new ConsoleAppSettings();
        var logger = this.CreateTestLogger<ConsoleAppContextFactory>();
        var factory = new ConsoleAppContextFactory(appProcess, settings, logger);

        // Act
        _ = factory.CreateConsoleAppContext(args, commandParametersOption);

        // Assert
        Assert.Equal(1, this.LogCollector.Count);
        var record = this.LogCollector.LatestRecord;
        Assert.Equal(LogLevel.Information, record.Level);
        Assert.Equal(1001, record.Id);
        Assert.Equal("起動パラメーター:console-app-context-factory-test --category-id 123 のパースを行います。", record.Message);
    }

    [Fact]
    public void CreateConsoleAppContext_正常系_起動パラメーターの情報が含まれたコンテキストを生成できる()
    {
        // Arrange
        var args = new string[] { "console-app-context-factory-test", "--category-id", "123" };
        var types = new Type[] { typeof(TestParameter) };
        var assembly = new TestAssembly(types);
        Action<CommandParameterTypeCollection>? commandParametersOption = collection => collection.AddCommandParameterTypeFrom(assembly);
        var appProcess = new TestApplicationProcess();
        var settings = new ConsoleAppSettings();
        var logger = this.CreateTestLogger<ConsoleAppContextFactory>();
        var factory = new ConsoleAppContextFactory(appProcess, settings, logger);

        // Act
        var context = factory.CreateConsoleAppContext(args, commandParametersOption);

        // Assert
        var parameter = Assert.IsType<TestParameter>(context.Parameter);
        Assert.Equal(123, parameter.CategoryId);
    }

    [Fact]
    public void CreateConsoleAppContext_読み込んだアセンブリ内にCommandAttributeを付与したパラメーターがない_InvalidOperationExceptionが発生する()
    {
        // Arrange
        var args = new string[] { "console-app-context-factory-test", "--category-id", "123" };
        Type[] types = [];
        var assembly = new TestAssembly(types);
        Action<CommandParameterTypeCollection>? commandParametersOption = collection => collection.AddCommandParameterTypeFrom(assembly);
        var appProcess = new TestApplicationProcess();
        var settings = new ConsoleAppSettings();
        var logger = this.CreateTestLogger<ConsoleAppContextFactory>();
        var factory = new ConsoleAppContextFactory(appProcess, settings, logger);

        // Act
        var action = () => factory.CreateConsoleAppContext(args, commandParametersOption);

        // Assert
        var ex = Assert.Throws<InvalidOperationException>(action);
        Assert.Equal("Maris.ConsoleApp.Core.CommandAttribute 属性を追加したコマンドパラメーターの型が読み込まれたアセンブリ TestAssembly に見つかりません。", ex.Message);
    }

    [Fact]
    public void CreateConsoleAppContext_起動パラメーターのパースに失敗する_DefaultValidationErrorExitCodeに設定した終了コードでプロセスが終了する()
    {
        // Arrange
        var args = new string[] { "console-app-context-factory-test", "--category-id", "invalid-value" };
        var types = new Type[] { typeof(TestParameter) };
        var assembly = new TestAssembly(types);
        Action<CommandParameterTypeCollection>? commandParametersOption = collection => collection.AddCommandParameterTypeFrom(assembly);
        var appProcess = new TestApplicationProcess();
        var exitCode = 789;
        var settings = new ConsoleAppSettings { DefaultValidationErrorExitCode = exitCode };
        var logger = this.CreateTestLogger<ConsoleAppContextFactory>();
        var factory = new ConsoleAppContextFactory(appProcess, settings, logger);

        // Act
        var action = () => factory.CreateConsoleAppContext(args, commandParametersOption);

        // Assert
        var ex = Assert.Throws<ApplicationException>(action);
        Assert.Equal("アプリケーションの終了処理が呼び出されました。", ex.Message);
        Assert.Equal(exitCode, appProcess.ExitCode);
    }

    private class TestAssembly : Assembly
    {
        private readonly Type[] types;

        internal TestAssembly(Type[] types) => this.types = types;

        public override Type[] GetTypes() => this.types;

        public override AssemblyName GetName() => new(nameof(TestAssembly));
    }

    [Command("console-app-context-factory-test", typeof(TestCommand))]
    private class TestParameter
    {
        [Option("category-id", Required = true)]
        public long CategoryId { get; set; }
    }

    private class TestCommand : SyncCommand<TestParameter>
    {
        protected internal override ICommandResult Execute(TestParameter parameter)
            => new SuccessResult();
    }

    private class TestApplicationProcess : IApplicationProcess
    {
        public int ExitCode { get; private set; }

        [DoesNotReturn]
        public void Exit(int exitCode)
        {
            this.ExitCode = exitCode;
            throw new ApplicationException("アプリケーションの終了処理が呼び出されました。");
        }
    }
}
