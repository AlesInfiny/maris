using System.Diagnostics.CodeAnalysis;

namespace Maris.ConsoleApp.Hosting;

/// <summary>
///  アプリケーションのプロセスに関連するインターフェースです。
/// </summary>
internal interface IApplicationProcess
{
    /// <summary>
    ///  指定した終了コードでこのプロセスを終了します。
    /// </summary>
    /// <param name="exitCode">終了コード。</param>
    [DoesNotReturn]
    void Exit(int exitCode);
}
