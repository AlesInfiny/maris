using System.Diagnostics.CodeAnalysis;

namespace Maris.ConsoleApp.Hosting;

/// <summary>
///  コンソールアプリケーションのプロセスに関連する処理を提供します。
/// </summary>
internal class ConsoleAppProcess : IApplicationProcess
{
    /// <inheritdoc/>
    [DoesNotReturn]
    public void Exit(int exitCode) => Environment.Exit(exitCode);
}
