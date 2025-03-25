using Maris.ConsoleApp.Hosting;
using Maris.ConsoleApp.IntegrationTests.ScopeTests.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Maris.ConsoleApp.IntegrationTests.ScopeTests;

[Collection(nameof(ScopeTests))]
public class SingletonTest(ITestOutputHelper testOutputHelper) : TestBase(testOutputHelper)
{
    [Fact]
    public async Task Singletonで登録したインスタンスはコマンド実行時に1回だけ初期化される()
    {
        // Arrange
        ObjectStateHistory.Clear();
        using var app = this.CreateHost();
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        await app.RunAsync(cancellationToken);

        // Assert
        var createHistories = ObjectStateHistory.Histories
            .Where(h => h.Condition == Condition.Creating)
            .OrderBy(h => h.ObjectType.Name)
            .ToList();
        Assert.Collection(
            createHistories,
            h => Assert.Equal(typeof(TestObject1), h.ObjectType),
            h => Assert.Equal(typeof(TestObject2), h.ObjectType));
    }

    [Fact]
    public async Task Singletonで登録したインスタンスはコマンドの実行時に破棄されていない()
    {
        // Arrange
        ObjectStateHistory.Clear();
        using var app = this.CreateHost();
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        await app.RunAsync(cancellationToken);

        // Assert
        var createHistories = ObjectStateHistory.Histories
            .Where(h => h.Condition == Condition.Alive)
            .OrderBy(h => h.ObjectType.Name)
            .ToList();
        Assert.Collection(
            createHistories,
            h => Assert.Equal(typeof(TestObject1), h.ObjectType),
            h => Assert.Equal(typeof(TestObject2), h.ObjectType),
            h => Assert.Equal(typeof(TestObject2), h.ObjectType));
    }

    [Fact]
    public async Task Singletonで登録したインスタンスはホストの終了時に1回破棄が実行される()
    {
        // Arrange
        ObjectStateHistory.Clear();
        using var app = this.CreateHost();
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        await app.RunAsync(cancellationToken);

        // Assert
        var createHistories = ObjectStateHistory.Histories
            .Where(h => h.Condition == Condition.ObjectDisposing)
            .OrderBy(h => h.ObjectType.Name)
            .ToList();
        Assert.Collection(
            createHistories,
            h => Assert.Equal(typeof(TestObject1), h.ObjectType),
            h => Assert.Equal(typeof(TestObject2), h.ObjectType));
    }

    [Fact]
    public async Task Singletonで登録したインスタンスはホストの終了時に1回破棄される()
    {
        // Arrange
        ObjectStateHistory.Clear();
        using var app = this.CreateHost();
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        await app.RunAsync(cancellationToken);

        // Assert
        var createHistories = ObjectStateHistory.Histories
            .Where(h => h.Condition == Condition.ObjectDisposed)
            .OrderBy(h => h.ObjectType.Name)
            .ToList();
        Assert.Collection(
            createHistories,
            h => Assert.Equal(typeof(TestObject1), h.ObjectType),
            h => Assert.Equal(typeof(TestObject2), h.ObjectType));
    }

    [Fact]
    public async Task Singletonで登録したインスタンスは型ごとに1つだけ()
    {
        // Arrange
        ObjectStateHistory.Clear();
        using var app = this.CreateHost();
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        await app.RunAsync(cancellationToken);

        // Assert
        var createHistories = ObjectStateHistory.Histories
            .GroupBy(h => h.ObjectType)
            .OrderBy(g => g.Key.Name)
            .ToList();
        Assert.Collection(
            createHistories,
            g => Assert.Single(g.Select(g => g.ObjectId).Distinct()),
            g => Assert.Single(g.Select(g => g.ObjectId).Distinct()));
    }

    private IHost CreateHost()
    {
        var args = new string[] { Command.CommandName };
        var builder = Host.CreateDefaultBuilder(args);
        builder.ConfigureServices((context, services) =>
        {
            services.AddTestLogging(this.LoggerManager);
            services.AddSingleton<TestObject1>(); // Command 内で利用
            services.AddSingleton<TestObject2>(); // Command, TestObject1 内で利用
            services.AddConsoleAppService(args);
        });

        return builder.Build();
    }
}
