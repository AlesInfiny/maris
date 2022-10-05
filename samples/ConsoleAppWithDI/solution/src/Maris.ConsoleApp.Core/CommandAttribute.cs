using System.Diagnostics.CodeAnalysis;
using CommandLine;
using Maris.ConsoleApp.Core.Resources;

namespace Maris.ConsoleApp.Core;

/// <summary>
///  コマンドパラメーターを表すクラスに、そのパラメーターを利用するコマンドを設定するためのカスタム属性です。
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class CommandAttribute : VerbAttribute
{
    private Type commandType;

    /// <summary>
    ///  <see cref="CommandAttribute"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="name">コマンドの名前。</param>
    /// <param name="commandType">コマンドの型。</param>
    /// <param name="isDefault">既定のコマンドかどうか示す値。未指定の場合は <see langword="false"/> 。</param>
    /// <param name="aliases">コマンドの名前のエイリアス。未指定の場合は <see langword="null"/> 。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="commandType"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    /// <exception cref="ArgumentException">
    ///  <list type="bullet">
    ///   <item>
    ///    コマンドの型は <see cref="SyncCommand{TParam}"/> または <see cref="AsyncCommand{TParam}"/> を実装していません。
    ///   </item>
    ///  </list>
    /// </exception>
    public CommandAttribute(string name, Type commandType, bool isDefault = false, string[]? aliases = null)
                : base(name, isDefault, aliases)
        => this.CommandType = commandType ?? throw new ArgumentNullException(nameof(commandType));

    /// <summary>
    ///  コマンドの型を取得します。
    /// </summary>
    /// <exception cref="ArgumentException">
    ///  <list type="bullet">
    ///   <item>
    ///    コマンドの型は <see cref="SyncCommand{TParam}"/> または <see cref="AsyncCommand{TParam}"/> を実装していません。
    ///   </item>
    ///  </list>
    /// </exception>
    public Type CommandType
    {
        get => this.commandType;

        [MemberNotNull(nameof(this.commandType))]
        init
        {
            ArgumentNullException.ThrowIfNull(value);
            if (!value.IsCommandType())
            {
                throw new ArgumentException(
                    Messages.InvalidCommandType.Embed(value, typeof(SyncCommand<>), typeof(AsyncCommand<>)),
                    nameof(value));
            }

            this.commandType = value;
        }
    }
}
