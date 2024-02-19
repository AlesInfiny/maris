using Microsoft.Extensions.Logging;

namespace Maris.Samples.Cli.Constants;
/// <summary>
/// イベントIDを管理するクラスです。
/// </summary>
internal class SampleCliLogEvents
{
    internal static readonly EventId Over10ProductsFoundInRange = new(1001, "Over10ProductsFound");

    internal static readonly EventId DebugEvent = new (9999,"DebugEvent");
}
