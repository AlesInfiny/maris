using Maris.ConsoleApp.Core.Resources;

namespace Maris.ConsoleApp.Core;

/// <summary>
///  処理の成功を表す <see cref="ICommandResult"/> です。
/// </summary>
public class SuccessResult : ICommandResult
{
    /// <inheritdoc/>
    public int ExitCode => 0;

    /// <inheritdoc/>
    public override string ToString() => Messages.SuccessResult.Embed(this.ExitCode);
}
