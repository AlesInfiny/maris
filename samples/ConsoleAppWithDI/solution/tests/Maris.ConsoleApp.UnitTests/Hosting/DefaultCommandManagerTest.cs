using Maris.ConsoleApp.Core;
using Maris.ConsoleApp.Hosting;

namespace Maris.ConsoleApp.UnitTests.Hosting;

public class DefaultCommandManagerTest
{
    [Fact]
    public void Constructor_ConsoleAppContextがnullの場合は例外()
    {
        // Arrange
        ConsoleAppContext? context = null;
        var provider = Mock.Of<IServiceProvider>();

        // Act
        var action = () => new DefaultCommandManager(context!, provider);

        // Assert
        Assert.Throws<ArgumentNullException>("context", action);
    }

    [Fact]
    public void Constructor_IServiceProviderがnullの場合は例外()
    {
        // Arrange
        var parameter = new TestParameter();
        var context = new ConsoleAppContext(parameter);
        IServiceProvider? provider = null;

        // Act
        var action = () => new DefaultCommandManager(context, provider!);

        // Assert
        Assert.Throws<ArgumentNullException>("provider", action);
    }

    [Fact]
    public void マネージャーに設定したコンテキストの情報が初期化処理を通してコマンドにも設定される()
    {
        // Arrange
        var provider = Mock.Of<IServiceProvider>();
        var parameter = new TestParameter();
        var context = new ConsoleAppContext(parameter);
        var manager = new DefaultCommandManagerMock(context, provider);

        // Act
        var command = manager.CreateCommand();

        // Assert
        Assert.Same(context, command.Context);
    }

    [Fact]
    public void ReleaseCommandを呼び出すとスコープがクローズされる()
    {
        // Arrange
        var provider = Mock.Of<IServiceProvider>();
        var parameter = new TestParameter();
        var context = new ConsoleAppContext(parameter);
        var manager = new DefaultCommandManagerMock(context, provider);

        // Act
        manager.ReleaseCommand();

        // Assert
        Assert.True(manager.ScopeClosed);
    }

    [Fact]
    public void ReleaseCommandを複数回呼び出しても例外にならずスコープはクローズされる()
    {
        // Arrange
        var provider = Mock.Of<IServiceProvider>();
        var parameter = new TestParameter();
        var context = new ConsoleAppContext(parameter);
        var manager = new DefaultCommandManagerMock(context, provider);

        // Act
        manager.ReleaseCommand();
        manager.ReleaseCommand();

        // Assert
        Assert.True(manager.ScopeClosed);
    }

    [Command("test-command", typeof(TestCommand))]
    private class TestParameter
    {
    }

    private class TestCommand : SyncCommand<TestParameter>
    {
        protected internal override ICommandResult Execute(TestParameter parameter)
            => throw new NotImplementedException();
    }

    private class DefaultCommandManagerMock : DefaultCommandManager
    {
        public DefaultCommandManagerMock(ConsoleAppContext context, IServiceProvider provider)
            : base(context, provider)
        {
        }

        internal override CommandBase CreateCommandInScope()
            => new TestCommand();
    }
}
