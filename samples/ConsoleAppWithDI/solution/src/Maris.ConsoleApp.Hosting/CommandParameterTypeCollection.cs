using System.Collections;
using System.Reflection;
using Maris.ConsoleApp.Core;
using Maris.ConsoleApp.Hosting.Resources;

namespace Maris.ConsoleApp.Hosting;

/// <summary>
///  コマンドの名前とコマンドの型を管理するコレクションです。
/// </summary>
internal class CommandParameterTypeCollection : IEnumerable<Type>
{
    private readonly Dictionary<string, Type> commandParameterTypes = new();
    private readonly List<Assembly> loadedAssemblies = new();

    /// <summary>
    ///  <see cref="CommandParameterTypeCollection"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    internal CommandParameterTypeCollection()
    {
    }

    /// <summary>
    ///  読み込まれたアセンブリの一覧を取得します。
    /// </summary>
    internal IReadOnlyCollection<Assembly> LoadedAssemblies => this.loadedAssemblies.AsReadOnly();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    /// <summary>
    ///  コマンドの型の列挙子を取得します。
    /// </summary>
    /// <returns>コマンドの型の列挙子。</returns>
    public IEnumerator<Type> GetEnumerator() => this.commandParameterTypes.Values.GetEnumerator();

    /// <summary>
    ///  現在のアプリケーションドメインに読み込まれているすべてのアセンブリから、
    ///  コマンドのパラメーターとして設定されている型をすべて登録します。
    /// </summary>
    /// <exception cref="ArgumentException">
    ///  <list type="bullet">
    ///   <item>同じ名前のコマンドが登録されています。</item>
    ///  </list>
    /// </exception>
    internal void InitializeFromAllAssemblies()
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            this.AddCommandParameterTypeFrom(assembly);
        }
    }

    /// <summary>
    ///  指定したアセンブリから
    ///  コマンドのパラメーターとして設定されている型をすべて登録します。
    /// </summary>
    /// <param name="assembly">対象のアセンブリ。</param>
    /// <exception cref="ArgumentException">
    ///  <list type="bullet">
    ///   <item>同じ名前のコマンドが登録されています。</item>
    ///  </list>
    /// </exception>
    internal virtual void AddCommandParameterTypeFrom(Assembly assembly)
    {
        this.loadedAssemblies.Add(assembly);
        foreach (var type in assembly.GetTypes())
        {
            var commandAttribute = type.GetCustomAttribute<CommandAttribute>();
            if (commandAttribute is null)
            {
                continue;
            }

            this.Add(type, commandAttribute.Name);
        }
    }

    /// <summary>
    ///  指定したコマンドパラメーターの型とコマンド名を登録します。
    /// </summary>
    /// <param name="parameterType">コマンドパラメーターの型。</param>
    /// <param name="commandName">コマンド名。</param>
    /// <exception cref="ArgumentException">
    ///  <list type="bullet">
    ///   <item><paramref name="commandName"/> のコマンドは既に登録されています。</item>
    ///  </list>
    /// </exception>
    private void Add(Type parameterType, string commandName)
    {
        if (this.commandParameterTypes.TryGetValue(commandName, out var registeredType))
        {
            throw new ArgumentException(
                Messages.CommandNameDuplicated.Embed(commandName, parameterType, registeredType),
                nameof(commandName));
        }

        this.commandParameterTypes.Add(commandName, parameterType);
    }
}
