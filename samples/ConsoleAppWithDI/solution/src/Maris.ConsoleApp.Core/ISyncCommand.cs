namespace Maris.ConsoleApp.Core;

/// <summary>
///  同期実行するコマンドの内部インターフェースです。
///  同期実行するコマンドを作成する場合は、 <see cref="SyncCommand{TParam}"/> を実装してください。
/// </summary>
internal interface ISyncCommand
{
    /// <summary>
    ///  コマンドを同期実行します。
    /// </summary>
    /// <returns>コマンドの実行結果。</returns>
    internal ICommandResult Execute();
}
