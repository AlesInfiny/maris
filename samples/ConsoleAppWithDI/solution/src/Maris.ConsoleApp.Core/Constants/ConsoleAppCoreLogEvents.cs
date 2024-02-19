using Microsoft.Extensions.Logging;

namespace Maris.ConsoleApp.Core.Constants;

/// <summary>
/// イベントIDを管理するクラスです。
/// </summary>
internal class ConsoleAppCoreLogEvents
{
    /// <summary>
    /// デバッグ用のイベントID
    /// </summary>
    internal static readonly EventId DebugEvent = new(9999, "DebugEvent");
}
