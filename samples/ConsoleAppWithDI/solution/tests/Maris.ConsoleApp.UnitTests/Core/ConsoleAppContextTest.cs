using Maris.ConsoleApp.Core;

namespace Maris.ConsoleApp.UnitTests.Core;

public class ConsoleAppContextTest
{
    [Fact]
    public void Constructor_パラメーターがnullの場合は例外()
    {
        // Arrange
        object? parameter = null;

        // Act
        var action = () => new ConsoleAppContext(parameter!);

        // Assert
        Assert.Throws<ArgumentNullException>("parameter", action);
    }

    [Fact]
    public void Constructor_パラメーターにCommandAttributeがついていない場合は例外()
    {
        // Arrange
        object parameter = new NoCommandAttributeParameter();

        // Act
        var action = () => new ConsoleAppContext(parameter);

        // Assert
        var ex = Assert.Throws<ArgumentException>("parameter", action);
        Assert.StartsWith("指定したパラメーターのクラス Maris.ConsoleApp.UnitTests.Core.ConsoleAppContextTest+NoCommandAttributeParameter に Maris.ConsoleApp.Core.CommandAttribute 属性が追加されていません。", ex.Message);
    }

    [Fact]
    public void Parameter_コンストラクタに指定したパラメーターを取得できる()
    {
        // Arrange
        object parameter = new DummyParameter();

        // Act
        var context = new ConsoleAppContext(parameter);

        // Assert
        Assert.Same(parameter, context.Parameter);
    }

    [Fact]
    public void CommandName_パラメータークラスに付与したコマンドの名前を取得できる()
    {
        // Arrange
        object parameter = new DummyParameter();

        // Act
        var context = new ConsoleAppContext(parameter);

        // Assert
        Assert.Equal("dummy", context.CommandName);
    }

    [Fact]
    public void CommandType_パラメータークラスに付与したコマンドの型を取得できる()
    {
        // Arrange
        object parameter = new DummyParameter();

        // Act
        var context = new ConsoleAppContext(parameter);

        // Assert
        Assert.Equal(typeof(DummyCommand), context.CommandType);
    }

    private class NoCommandAttributeParameter
    {
    }

    [Command("dummy", typeof(DummyCommand))]
    private class DummyParameter
    {
    }

    private class DummyCommand : SyncCommand<DummyParameter>
    {
        protected internal override ICommandResult Execute(DummyParameter parameter)
            => throw new NotImplementedException();
    }
}
