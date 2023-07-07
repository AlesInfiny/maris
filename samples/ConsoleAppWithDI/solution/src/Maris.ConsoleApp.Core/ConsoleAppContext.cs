using System.Reflection;
using Maris.ConsoleApp.Core.Resources;

namespace Maris.ConsoleApp.Core;

/// <summary>
///  コンソールアプリケーションの実行コンテキストを管理します。
/// </summary>
public class ConsoleAppContext
{
    private readonly CommandAttribute commandAttribute;

    /// <summary>
    ///  コンソールアプリケーションの起動パラメーターオブジェクトを指定して
    ///  <see cref="ConsoleAppContext"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="parameter">コンソールアプリケーションの起動パラメーターオブジェクト。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="parameter"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    /// <exception cref="ArgumentException">
    ///  <list type="bullet">
    ///   <item><paramref name="parameter"/> のクラスに <see cref="CommandAttribute"/> が付与されていません。</item>
    ///  </list>
    /// </exception>
    public ConsoleAppContext(object parameter)
    {
        this.Parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
        this.commandAttribute = parameter.GetType().GetCustomAttribute<CommandAttribute>()
            ?? throw new ArgumentException(
                Messages.NoCustomAttributesAddedToParameterClass.Embed(parameter.GetType(), typeof(CommandAttribute)),
                nameof(parameter));
    }

    /// <summary>
    ///  テストコードからのみ利用します。
    ///  <see cref="ConsoleAppContext"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="commandAttribute">コマンド属性。</param>
    /// <param name="parameter">コンソールアプリケーションの起動パラメーターオブジェクト。</param>
    internal ConsoleAppContext(CommandAttribute commandAttribute, object parameter)
    {
        this.commandAttribute = commandAttribute;
        this.Parameter = parameter;
    }

    /// <summary>
    ///  コンソールアプリケーションの起動パラメーターオブジェクトを取得します。
    /// </summary>
    public object Parameter { get; private set; }

    /// <summary>
    ///  実行するコマンド名を取得します。
    /// </summary>
    public string CommandName => this.commandAttribute.Name;

    /// <summary>
    ///  実行するコマンドの型を取得します。
    /// </summary>
    public Type CommandType => this.commandAttribute.CommandType;
}
