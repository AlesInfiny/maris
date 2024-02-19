using Microsoft.Extensions.Logging;

namespace Maris.ConsoleApp.Hosting.Constants;

/// <summary>
/// イベントIDを管理するクラスです。
/// </summary>
internal class ConsoleAppHostingLogEvents
{
    /// <summary>
    /// パラメータのパースの開始を示すイベントID
    /// </summary>
    internal static readonly EventId StartParseParameter = new(1001, "StartParseParameter");

    /// <summary>
    /// ホスティングサービスの開始を示すイベントID
    /// </summary>
    internal static readonly EventId StartHostingService = new(1101, "StartHostingService");

    /// <summary>
    /// 不正なパラメータが検出されたことを示すイベントID
    /// </summary>
    internal static readonly EventId InvalidParameterDetected = new(1102, "InvalidParameterDetected");

    /// <summary>
    /// 例外が発生したことを示すイベントID
    /// </summary>
    internal static readonly EventId ExceptionrDetected = new(1103, "ExceptionrDetected");

    /// <summary>
    /// ホスティングサービスの終了を示すイベントID
    /// </summary>
    internal static readonly EventId StopHostingService = new(1104, "StopHostingService");

    /// <summary>
    /// デバッグ用のイベントID
    /// </summary>
    internal static readonly EventId DebugEvent = new(9999, "DebugEvent");
}
