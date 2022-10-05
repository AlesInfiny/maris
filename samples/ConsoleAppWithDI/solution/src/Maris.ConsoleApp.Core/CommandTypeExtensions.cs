namespace Maris.ConsoleApp.Core;

/// <summary>
///  コマンドの型に対する拡張メソッドを提供します。
/// </summary>
public static class CommandTypeExtensions
{
    private static readonly Type CommandBaseType = typeof(CommandBase);
    private static readonly Type SyncCommandType = typeof(ISyncCommand);
    private static readonly Type AsyncCommandType = typeof(IAsyncCommand);

    /// <summary>
    ///  指定した型がコマンドの型であるかどうか示す値を取得します。
    /// </summary>
    /// <param name="type">コマンドの型か検査する <see cref="Type"/> オブジェクト。</param>
    /// <returns>コマンドの型である場合は <see langword="true"/> 、そうでない場合は <see langword="false"/> 。</returns>
    public static bool IsCommandType(this Type type)
    {
        // プロジェクト外から見ると、コマンドは SyncCommand<TParam> または AsyncCommand<TParam> を継承していることが条件。
        // ISyncCommand と IAsyncCommand は内部インターフェースであり、SyncCommand<TParam> と AsyncCommand<TParam> が
        // これらのインターフェースを持った唯一の外部公開されているクラス。
        // なのでコマンドかどうかは、これらのインターフェースどちらかを実装しているか確認すればよい。
        // 念のためコマンドのベースクラスである CommandBase も確認対象に含めておく。
        return CommandBaseType.IsAssignableFrom(type) &&
            (SyncCommandType.IsAssignableFrom(type) || AsyncCommandType.IsAssignableFrom(type));
    }
}
