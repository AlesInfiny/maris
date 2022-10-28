using Maris.ConsoleApp.Core;

namespace Maris.ConsoleApp.UnitTests.Core;

public class CommandTypeExtensionsTest
{
    [Fact]
    public void IsCommandType_コマンドとは無関係の型を指定するとfalseを取得する()
    {
        // Arrange
        Type type = typeof(DateTime);

        // Act
        var result = CommandTypeExtensions.IsCommandType(type);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsCommandType_CommandBaseだけを継承した型を指定するとfalseを取得する()
    {
        // Arrange
        Type type = typeof(OnlyCommandBaseImpl);

        // Act
        var result = CommandTypeExtensions.IsCommandType(type);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(typeof(SyncCommandImpl))]
    [InlineData(typeof(AsyncCommandImpl))]
    public void IsCommandType_SyncCommandまたはAsyncCommandを継承した型を指定するとtrueを取得する(Type t)
    {
        // Arrange
        Type type = t;

        // Act
        var result = CommandTypeExtensions.IsCommandType(type);

        // Assert
        Assert.True(result);
    }

    private class OnlyCommandBaseImpl : CommandBase
    {
        internal override void ValidateParameter()
        {
            throw new NotImplementedException();
        }
    }

    [Command("sync", typeof(SyncCommandImpl))]
    private class SyncCommandParameter
    {
    }

    private class SyncCommandImpl : SyncCommand<SyncCommandParameter>
    {
        protected internal override ICommandResult Execute(SyncCommandParameter parameter)
            => throw new NotImplementedException();
    }

    [Command("async", typeof(AsyncCommandImpl))]
    private class AsyncCommandParameter
    {
    }

    private class AsyncCommandImpl : AsyncCommand<AsyncCommandParameter>
    {
        protected internal override Task<ICommandResult> ExecuteAsync(AsyncCommandParameter parameter, CancellationToken cancellationToken)
            => throw new NotImplementedException();
    }
}
