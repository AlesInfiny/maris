using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Maris.ConsoleApp.Core;
using Maris.ConsoleApp.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Maris.ConsoleApp.UnitTests.Hosting;

public class ServiceCollectionExtensionsTest(ITestOutputHelper testOutputHelper) : TestBase(testOutputHelper)
{
    [Fact]
    public void AddConsoleAppService_必要なサービスが登録される()
    {
        // Arrange
        var services = new ServiceCollection();
        var args = new string[] { "aaa" };

        // Act
        services.AddConsoleAppService(args);

        // Assert
        Assert.Collection(
            services,
            service =>
            {
                Assert.Equal(typeof(IHostedService), service.ServiceType);
                Assert.Equal(ServiceLifetime.Singleton, service.Lifetime);
            },
            service =>
            {
                Assert.Equal(typeof(IApplicationProcess), service.ServiceType);
                Assert.Equal(typeof(ConsoleAppProcess), service.ImplementationType);
            },
            service =>
            {
                Assert.Equal(typeof(ConsoleAppSettings), service.ServiceType);
            },
            service =>
            {
                Assert.Equal(typeof(ConsoleAppContextFactory), service.ServiceType);
            },
            service =>
            {
                Assert.Equal(typeof(ConsoleAppContext), service.ServiceType);
            },
            service =>
            {
                Assert.Equal(typeof(CommandExecutor), service.ServiceType);
            },
            service =>
            {
                Assert.Equal(typeof(ICommandManager), service.ServiceType);
                Assert.Equal(typeof(DefaultCommandManager), service.ImplementationType);
            });
    }

    [Fact]
    public void AddConsoleAppSettings_ConsoleAppSettingsの設定処理を指定しない_既定のオブジェクトで初期化される()
    {
        // Arrange
        var services = new ServiceCollection();
        Action<ConsoleAppSettings>? options = null;

        // Act
        services.AddConsoleAppSettings(options);

        // Assert
        var provider = services.BuildServiceProvider();
        var settings = provider.GetRequiredService<ConsoleAppSettings>();
        Assert.Equal(int.MinValue, settings.DefaultValidationErrorExitCode);
        Assert.Equal(int.MaxValue, settings.DefaultErrorExitCode);
    }

    [Fact]
    public void AddConsoleAppSettings_ConsoleAppSettingsの設定処理を指定する_指定した値で初期化される()
    {
        // Arrange
        var services = new ServiceCollection();
        var defaultExitCode = 1000;
        var defaultValidationErrorCode = 1001;

        // Act
        services.AddConsoleAppSettings(settings =>
        {
            settings.DefaultErrorExitCode = defaultExitCode;
            settings.DefaultValidationErrorExitCode = defaultValidationErrorCode;
        });

        // Assert
        var provider = services.BuildServiceProvider();
        var settings = provider.GetRequiredService<ConsoleAppSettings>();
        Assert.Equal(defaultValidationErrorCode, settings.DefaultValidationErrorExitCode);
        Assert.Equal(defaultExitCode, settings.DefaultErrorExitCode);
    }

    [Fact]
    public void アセンブリ内にコマンドが登録されていない_InvalidOperationExceptionが発生する()
    {
        // Arrange
        var services = new ServiceCollection();
        var args = new string[] { "command-is-not-exists-in-assembly" };
        Action<ConsoleAppSettings>? options = null;
        services.AddConsoleAppSettings(options);
        var testApplicationProcess = new TestApplicationProcess();
        services.AddSingleton<IApplicationProcess, TestApplicationProcess>(provider => testApplicationProcess);
        var types = Array.Empty<Type>();
        var assembly1 = new TestAssembly1(types);
        var assembly2 = new TestAssembly2([]);
        services.AddTestLogging(this.LoggerManager);
        services.AddSingleton<ConsoleAppContextFactory>();

        // Act
        services.AddConsoleAppContext(args, types =>
        {
            types.AddCommandParameterTypeFrom(assembly1);
            types.AddCommandParameterTypeFrom(assembly2);
        });
        var provider = services.BuildServiceProvider();
        var action = () => provider.GetRequiredService<ConsoleAppContext>();

        // Assert
        var ex = Assert.Throws<InvalidOperationException>(action);
        Assert.Equal($"{typeof(CommandAttribute)} 属性を追加したコマンドパラメーターの型が読み込まれたアセンブリ TestAssembly1,TestAssembly2 に見つかりません。", ex.Message);
    }

    [Fact]
    public void 登録されていないコマンド名を指定_検証エラーのエラーコードを伴ってApplicationExceptionが発生する()
    {
        // Arrange
        var services = new ServiceCollection();
        var args = new string[] { "not-exists-command" };
        var validationErrorExitCode = 1010;
        services.AddConsoleAppSettings(settings => settings.DefaultValidationErrorExitCode = validationErrorExitCode);
        var testApplicationProcess = new TestApplicationProcess();
        services.AddSingleton<IApplicationProcess, TestApplicationProcess>(provider => testApplicationProcess);
        var types = new Type[] { typeof(TestParameter1) };
        var assembly = new TestAssembly1(types);
        services.AddTestLogging(this.LoggerManager);
        services.AddSingleton<ConsoleAppContextFactory>();

        // Act
        services.AddConsoleAppContext(args, types =>
        {
            types.AddCommandParameterTypeFrom(assembly);
        });
        var provider = services.BuildServiceProvider();
        var action = () => provider.GetRequiredService<ConsoleAppContext>();

        // Assert
        Assert.Throws<ApplicationException>(action);
        Assert.Equal(validationErrorExitCode, testApplicationProcess.ExitCode);
    }

    [Theory]
    [InlineData("test-command1", typeof(TestParameter1), typeof(TestCommand1))]
    [InlineData("test-command2", typeof(TestParameter2), typeof(TestCommand2))]
    public void 登録されているコマンド名を指定_DIコンテナーからConsoleAppContextが生成できる(string commandName, Type parameterType, Type commandType)
    {
        // Arrange
        var services = new ServiceCollection();
        var args = new string[] { commandName };
        var validationErrorExitCode = 1010;
        services.AddConsoleAppSettings(settings => settings.DefaultValidationErrorExitCode = validationErrorExitCode);
        var testApplicationProcess = new TestApplicationProcess();
        services.AddSingleton<IApplicationProcess, TestApplicationProcess>(provider => testApplicationProcess);
        var types1 = new Type[] { typeof(TestParameter1) };
        var assembly1 = new TestAssembly1(types1);
        var types2 = new Type[] { typeof(TestParameter2) };
        var assembly2 = new TestAssembly1(types2);
        services.AddTestLogging(this.LoggerManager);
        services.AddSingleton<ConsoleAppContextFactory>();

        // Act
        services.AddConsoleAppContext(args, types =>
        {
            types.AddCommandParameterTypeFrom(assembly1);
            types.AddCommandParameterTypeFrom(assembly2);
        });
        var provider = services.BuildServiceProvider();
        var context = provider.GetRequiredService<ConsoleAppContext>();

        // Assert
        Assert.Equal(commandName, context.CommandName);
        Assert.IsType(parameterType, context.Parameter);
        Assert.Equal(commandType, context.CommandType);
    }

    private class TestAssembly1 : Assembly
    {
        private readonly Type[] types;

        internal TestAssembly1(Type[] types) => this.types = types;

        public override Type[] GetTypes() => this.types;

        public override AssemblyName GetName() => new(nameof(TestAssembly1));
    }

    private class TestAssembly2 : Assembly
    {
        private readonly Type[] types;

        internal TestAssembly2(Type[] types) => this.types = types;

        public override Type[] GetTypes() => this.types;

        public override AssemblyName GetName() => new(nameof(TestAssembly2));
    }

    [Command("test-command1", typeof(TestCommand1))]
    private class TestParameter1
    {
    }

    private class TestCommand1 : SyncCommand<TestParameter1>
    {
        protected internal override ICommandResult Execute(TestParameter1 parameter)
            => throw new NotImplementedException();
    }

    [Command("test-command2", typeof(TestCommand2))]
    private class TestParameter2
    {
    }

    private class TestCommand2 : SyncCommand<TestParameter2>
    {
        protected internal override ICommandResult Execute(TestParameter2 parameter)
            => throw new NotImplementedException();
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
