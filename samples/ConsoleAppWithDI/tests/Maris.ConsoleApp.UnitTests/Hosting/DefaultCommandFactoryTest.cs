using Maris.ConsoleApp.Core;
using Maris.ConsoleApp.Hosting;

namespace Maris.ConsoleApp.UnitTests.Hosting;

public class DefaultCommandFactoryTest
{
    [Fact]
    public void Constructor_IServiceProviderがnullの場合は例外()
    {
        // Arrange
        IServiceProvider? provider = null;
        var parameter = new TestParameter();
        ConsoleAppContext context = new ConsoleAppContext(parameter);

        // Act
        var action = () => new DefaultCommandFactory(provider!, context);

        // Assert
        Assert.Throws<ArgumentNullException>("provider", action);
    }

    [Fact]
    public void Constructor_ConsoleAppContextがnullの場合は例外()
    {
        // Arrange
        IServiceProvider provider = Mock.Of<IServiceProvider>();
        ConsoleAppContext? context = null;

        // Act
        var action = () => new DefaultCommandFactory(provider, context!);

        // Assert
        Assert.Throws<ArgumentNullException>("context", action);
    }

    [Fact]
    public void ファクトリーに設定したコンテキストの情報が初期化処理を通してコマンドにも設定される()
    {
        // Arrange
        IServiceProvider provider = Mock.Of<IServiceProvider>();
        var parameter = new TestParameter();
        ConsoleAppContext context = new ConsoleAppContext(parameter);
        var factory = new DefaultCommandFactoryMock(provider, context);

        // Act
        var command = factory.CreateCommand();

        // Assert
        Assert.Same(context, command.Context);
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

    private class DefaultCommandFactoryMock : DefaultCommandFactory
    {
        public DefaultCommandFactoryMock(IServiceProvider provider, ConsoleAppContext context)
            : base(provider, context)
        {
        }

        internal override CommandBase CreateCommandInScope(ConsoleAppContext context)
            => new TestCommand();
    }
}
