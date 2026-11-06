using Microsoft.Extensions.Logging;

namespace Dressca.AssetsManagement;

/// <summary>
/// イベント ID を管理するクラスです。
/// </summary>
internal static class Events
{
    /// <summary>
    /// アセットIDに紐づく画像ファイルが見つからなかったこと示すイベントID
    /// </summary>
    internal static readonly EventId AssetImageFileNotFound = new(1001, nameof(AssetImageFileNotFound));

    /// <summary>
    /// データベースのヘルスチェックに失敗したことを示すイベントID
    /// </summary>
    internal static readonly EventId FailedDatabaseHealthCheck = new(1002, nameof(FailedDatabaseHealthCheck));

    /// <summary>
    /// デバッグ用のイベント ID
    /// </summary>
    internal static readonly EventId DebugEvent = new(9999, nameof(DebugEvent));
}
