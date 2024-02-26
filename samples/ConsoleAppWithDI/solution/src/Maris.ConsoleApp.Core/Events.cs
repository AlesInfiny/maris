using Microsoft.Extensions.Logging;

namespace Maris.ConsoleApp.Core;

/// <summary>
/// イベントIDを管理するクラスです。
/// </summary>
internal class Events
{
    /// <summary>
    /// デバッグ用のイベントID
    /// </summary>
    internal static readonly EventId DebugEvent = new(9999, nameof(DebugEvent));
}
