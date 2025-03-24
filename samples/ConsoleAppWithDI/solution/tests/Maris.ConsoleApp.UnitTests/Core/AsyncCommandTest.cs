using Maris.ConsoleApp.Core;

namespace Maris.ConsoleApp.UnitTests.Core;

public class AsyncCommandTest
{
    public interface IParameter
    {
    }

    [Fact]
    public void Parameter_ConsoleAppContextに設定したパラメーターを取得できる()
    {
        // Arrange
        var parameter = new CommandParameter();
        var commandAttribute = new CommandAttribute("dummy-command", typeof(AsyncCommandImpl));
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var command = new AsyncCommandImpl();
        command.Initialize(context);

        // Act
        var actualParameter = command.ParameterProxy;

        // Assert
        Assert.Same(parameter, actualParameter);
    }

    [Fact]
    public void ValidateAllParameter_ValidateParameterメソッドがコンテキストに指定したパラメーターを伴って1回呼び出される()
    {
        // Arrange
        var parameter = new CommandParameter();
        var commandAttribute = new CommandAttribute("dummy-command", typeof(AsyncCommandImpl));
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var commandMock = new Mock<AsyncCommandImpl>();
        var command = commandMock.Object;
        command.Initialize(context);

        // Act
        command.ValidateAllParameter();

        // Assert
        commandMock.Verify(mock => mock.ValidateParameter(parameter), Times.Once);
    }

    [Fact]
    public void IAsyncCommandのExecuteAsync_ExecuteAsyncメソッドがコンテキストに指定したパラメーターとキャンセルトークンを伴って1回呼び出される()
    {
        // Arrange
        var parameter = new CommandParameter();
        var commandAttribute = new CommandAttribute("dummy-command", typeof(AsyncCommandImpl));
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var commandMock = new Mock<AsyncCommandImpl>();
        var command = commandMock.Object;
        command.Initialize(context);
        IAsyncCommand asyncCommand = command;
        var cancellationToken = new CancellationToken(false);

        // Act
        asyncCommand.ExecuteAsync(cancellationToken);

        // Assert
        commandMock.Verify(mock => mock.ExecuteAsync(parameter, cancellationToken), Times.Once);
    }

    [Fact]
    public void Initialize_パラメーターの型にインターフェースを使用できる()
    {
        // Arrange
        IParameter parameter = new ParameterWithInterface();
        var commandAttribute = new CommandAttribute("dummy-command", typeof(CommandWithInterface));
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var commandMock = new Mock<CommandWithInterface>();
        var command = commandMock.Object;

        // Act & Assert 例外が発生しない
        command.Initialize(context);
    }

    [Fact]
    public void Initialize_パラメーターの型とコンテキストのパラメーター型が一致しない_InvalidOperationExceptionが発生する()
    {
        // Arrange
        var parameter = new ParameterWithInterface();
        var commandAttribute = new CommandAttribute("dummy-command", typeof(AsyncCommandImpl));
        var context = new ConsoleAppContext(commandAttribute, parameter);
        var commandMock = new Mock<AsyncCommandImpl>();
        var command = commandMock.Object;

        // Act
        var action = () => command.Initialize(context);

        // Assert
        var ex = Assert.Throws<InvalidOperationException>(action);
        Assert.Equal($"invalid コンソールアプリケーションの実行コンテキストに設定されているパラメーター {typeof(ParameterWithInterface)} のオブジェクトを、コマンドクラスの実装時に指定した型 {typeof(CommandParameter)} として取り扱うことができません。 dummy-command コマンドまたはそのパラメーターの実装を修正してください。", ex.Message);
    }

    public class CommandParameter
    {
    }

    public class AsyncCommandImpl : AsyncCommand<CommandParameter>
    {
        internal CommandParameter ParameterProxy => this.Parameter;

        protected internal override Task<ICommandResult> ExecuteAsync(CommandParameter parameter, CancellationToken cancellationToken)
            => Task.FromResult<ICommandResult>(new CommandResult());
    }

    public class ParameterWithInterface : IParameter
    {
    }

    public class CommandWithInterface : AsyncCommand<IParameter>
    {
        protected internal override Task<ICommandResult> ExecuteAsync(IParameter parameter, CancellationToken cancellationToken)
            => Task.FromResult<ICommandResult>(new CommandResult());
    }

    private class CommandResult : ICommandResult
    {
        public int ExitCode => 0;
    }
}
