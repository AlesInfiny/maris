using Microsoft.Extensions.Logging;

namespace Maris.ConsoleApp.Hosting;

/// <summary>
///  イベント ID を管理するクラスです。
/// </summary>
internal class Events
{
    /// <summary>
    ///  パラメータのパースの開始を示すイベント ID
    /// </summary>
    internal static readonly EventId StartParseParameter = new(1001, nameof(StartParseParameter));

    /// <summary>
    ///  ホスティングサービスの開始を示すイベント ID
    /// </summary>
    internal static readonly EventId StartHostingService = new(1101, nameof(StartHostingService));

    /// <summary>
    ///  不正なパラメータが検出されたことを示すイベント ID
    /// </summary>
    internal static readonly EventId InvalidParameterDetected = new(1102, nameof(InvalidParameterDetected));

    /// <summary>
    ///  コマンドの実行時に例外が発生したことを示すイベント ID
    /// </summary>
    internal static readonly EventId CommandExecutorRaiseException = new(1103, nameof(CommandExecutorRaiseException));

    /// <summary>
    ///  ホスティングサービスの終了を示すイベント ID
    /// </summary>
    internal static readonly EventId StopHostingService = new(1104, nameof(StopHostingService));

    /// <summary>
    ///  デバッグ用のイベント ID
    /// </summary>
    internal static readonly EventId DebugEvent = new(9999, nameof(DebugEvent));
}
