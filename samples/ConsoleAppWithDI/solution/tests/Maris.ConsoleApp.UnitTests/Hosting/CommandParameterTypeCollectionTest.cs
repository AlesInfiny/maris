using System.Reflection;
using Maris.ConsoleApp.Core;
using Maris.ConsoleApp.Hosting;

namespace Maris.ConsoleApp.UnitTests.Hosting;

public class CommandParameterTypeCollectionTest
{
    [Fact]
    public void InitializeFromAllAssemblies_現在のアプリケーションドメインに読み込まれているアセンブリを登録できる()
    {
        // Arrange
        // すべてのアセンブリを AddCommandParameterTypeFrom メソッドに引き渡したかどうかでテストする。
        // AddCommandParameterTypeFrom メソッドを差し替えておかないと、実際に型を登録しようとしたときエラーになる可能性があり、
        // テストが不安定になるため、 InitializeFromAllAssemblies メソッドのテストは単独で行う。
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var collection = new CommandParameterTypeCollectionProxy();

        // Act
        collection.InitializeFromAllAssemblies();

        // Assert
        Assert.Equivalent(assemblies, collection.Assemblies);
    }

    [Fact]
    public void GetEnumerator_要素を追加せずに取得する_空の列挙子()
    {
        // Arrange
        var collection = new CommandParameterTypeCollection();

        // Act
        var enumerator = collection.GetEnumerator();

        // Assert
        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void AddCommandParameterTypeFrom_クラスの定義されていないアセンブリを読み込む_パラメーターの型は登録されない()
    {
        // Arrange
        var collection = new CommandParameterTypeCollection();
        var testAssembly = new TestAssembly([]);

        // Act
        collection.AddCommandParameterTypeFrom(testAssembly);

        // Assert
        Assert.Empty(collection);
    }

    [Fact]
    public void AddCommandParameterTypeFrom_パラメーターではないクラスだけが定義されたアセンブリを読み込む_パラメーターの型は登録されない()
    {
        // Arrange
        var collection = new CommandParameterTypeCollection();
        var testAssembly = new TestAssembly([typeof(int), typeof(DateTime), typeof(CommandParameterTypeCollection)]);

        // Act
        collection.AddCommandParameterTypeFrom(testAssembly);

        // Assert
        Assert.Empty(collection);
    }

    [Fact]
    public void AddCommandParameterTypeFrom_パラメーターを含むクラスが定義されたアセンブリを読み込む_アセンブリからパラメーターの型だけが登録される()
    {
        // Arrange
        var collection = new CommandParameterTypeCollection();
        var testAssembly = new TestAssembly(
            [
                typeof(int),
                typeof(DateTime),
                typeof(CommandParameterTypeCollection),
                typeof(CommandParameter1),
                typeof(CommandParameter2),
            ]);

        // Act
        collection.AddCommandParameterTypeFrom(testAssembly);

        // Assert
        Assert.Collection(
            collection,
            type => Assert.Equal(typeof(CommandParameter1), type),
            type => Assert.Equal(typeof(CommandParameter2), type));
    }

    [Fact]
    public void AddCommandParameterTypeFrom_同じ名前のコマンドがある_ArgumentExceptionが発生する()
    {
        // Arrange
        var collection = new CommandParameterTypeCollection();
        var testAssembly = new TestAssembly(
            [
                typeof(CommandParameter1),
                typeof(CommandParameter2),
                typeof(CommandParameter1Dash),
            ]);

        // Act
        var action = () => collection.AddCommandParameterTypeFrom(testAssembly);

        // Assert
        var exception = Assert.Throws<ArgumentException>("commandName", action);
        Assert.StartsWith($"command1 コマンドは同じ名前で登録されています。登録を試みたコマンドパラメーターの型は {typeof(CommandParameter1Dash)} 、登録されているコマンドパラメーターの型は {typeof(CommandParameter1)} です。", exception.Message);
    }

    [Fact]
    public void LoadedAssemblies_読み込んだアセンブリのリストを取得できる()
    {
        // Arrange
        var collection = new CommandParameterTypeCollection();
        var assembly1 = new TestAssembly([]);
        var assembly2 = new TestAssembly([]);
        collection.AddCommandParameterTypeFrom(assembly1);
        collection.AddCommandParameterTypeFrom(assembly2);

        // Act
        var assemblies = collection.LoadedAssemblies;

        // Assert
        Assert.Equivalent(assemblies, new Assembly[] { assembly1, assembly2 });
    }

    private class CommandParameterTypeCollectionProxy : CommandParameterTypeCollection
    {
        internal ICollection<Assembly> Assemblies { get; } = new List<Assembly>();

        internal override void AddCommandParameterTypeFrom(Assembly assembly) => this.Assemblies.Add(assembly);
    }

    private class TestAssembly : Assembly
    {
        private readonly Type[] types;

        internal TestAssembly(Type[] types)
        {
            this.types = types;
        }

        public override Type[] GetTypes() => this.types;
    }

    [Command("command1", typeof(Command1))]
    private class CommandParameter1
    {
    }

    [Command("command1", typeof(Command1))]
    private class CommandParameter1Dash
    {
    }

    private class Command1 : AsyncCommand<CommandParameter1>
    {
        protected internal override Task<ICommandResult> ExecuteAsync(CommandParameter1 parameter, CancellationToken cancellationToken)
            => throw new NotImplementedException();
    }

    [Command("command2", typeof(Command2))]
    private class CommandParameter2
    {
    }

    private class Command2 : AsyncCommand<CommandParameter2>
    {
        protected internal override Task<ICommandResult> ExecuteAsync(CommandParameter2 parameter, CancellationToken cancellationToken)
            => throw new NotImplementedException();
    }
}
