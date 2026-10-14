using Microsoft.Extensions.Logging;

namespace Dressca.Shopping;

/// <summary>
/// イベント ID を管理するクラスです。
/// </summary>
internal class Events
{
    /// <summary>陳列品 ID がリポジトリ内に存在しないことを示すイベント ID。</summary>
    internal static readonly EventId DisplayItemIdDoesNotExistInRepository = new(1001, nameof(DisplayItemIdDoesNotExistInRepository));

    /// <summary>
    /// データベースのヘルスチェックに失敗したことを示すイベントID
    /// </summary>
    internal static readonly EventId FailedDatabaseHealthCheck = new(1002, nameof(FailedDatabaseHealthCheck));

    /// <summary>
    /// デバッグ用のイベント ID
    /// </summary>
    internal static readonly EventId DebugEvent = new(9999, nameof(DebugEvent));
}
