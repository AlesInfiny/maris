using Maris.ConsoleApp.Core;

namespace Maris.ConsoleApp.UnitTests.Core;

public class CommandAttributeTest
{
    [Fact]
    public void Constructor_コマンドの型がnull_ArgumentNullExceptionが発生する()
    {
        // Arrange
        string name = "command";
        Type? commandType = null;

        // Act
        var action = () => new CommandAttribute(name, commandType!);

        // Assert
        Assert.Throws<ArgumentNullException>("commandType", action);
    }

    [Fact]
    public void Constructor_コマンドの型がコマンドの定義を満たしていない_ArgumentExceptionが発生する()
    {
        // Arrange
        string name = "command";
        Type commandType = typeof(NotCommandClass);

        // Act
        var action = () => new CommandAttribute(name, commandType);

        // Assert
        var ex = Assert.Throws<ArgumentException>("value", action);
        Assert.StartsWith($"指定されたコマンドの型 {typeof(NotCommandClass)} は {typeof(SyncCommand<>)} または {typeof(AsyncCommand<>)} を実装していません。", ex.Message);
    }

    [Fact]
    public void CommandType_コンストラクターで指定したコマンドの型を取得できる()
    {
        // Arrange
        string name = "command";
        Type commandType = typeof(CommandClass);

        // Act
        var attribute = new CommandAttribute(name, commandType);

        // Assert
        Assert.Equal(commandType, attribute.CommandType);
    }

    private class NotCommandClass
    {
    }

    private class CommandParameter
    {
    }

    private class CommandClass : SyncCommand<CommandParameter>
    {
        protected internal override ICommandResult Execute(CommandParameter parameter)
            => throw new NotImplementedException();
    }
}
