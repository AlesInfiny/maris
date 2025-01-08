using Microsoft.Extensions.Logging;

namespace Maris.ConsoleApp.Core;

/// <summary>
///  イベント ID を管理するクラスです。
/// </summary>
internal class Events
{
    /// <summary>
    ///  デバッグ用のイベント ID 。
    /// </summary>
    internal static readonly EventId DebugEvent = new(9999, nameof(DebugEvent));
}
