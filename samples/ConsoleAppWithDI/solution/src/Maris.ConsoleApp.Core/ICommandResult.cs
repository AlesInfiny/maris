namespace Maris.ConsoleApp.Core;

/// <summary>
///  コマンドの実行結果を表すインターフェースです。
/// </summary>
public interface ICommandResult
{
    /// <summary>
    ///  コマンドの終了コードを取得します。
    /// </summary>
    int ExitCode { get; }
}
