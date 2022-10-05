namespace Maris.ConsoleApp.Core;

/// <summary>
///  非同期実行するコマンドの内部インターフェースです。
///  非同期実行するコマンドを作成する場合は、 <see cref="AsyncCommand{TParam}"/> を実装してください。
/// </summary>
internal interface IAsyncCommand
{
    /// <summary>
    ///  コマンドを非同期実行します。
    /// </summary>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>コマンドの実行結果。</returns>
    internal Task<ICommandResult> ExecuteAsync(CancellationToken cancellationToken);
}
