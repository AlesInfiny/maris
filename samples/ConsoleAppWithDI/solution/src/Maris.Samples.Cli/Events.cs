using Microsoft.Extensions.Logging;

namespace Maris.Samples.Cli;

/// <summary>
///  イベント ID を管理するクラスです。
/// </summary>
internal class Events
{
    internal static readonly EventId Over10ProductsFoundInRange = new(1001, nameof(Over10ProductsFoundInRange));

    internal static readonly EventId DebugEvent = new(9999, nameof(DebugEvent));
}
