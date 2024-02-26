using Microsoft.Extensions.Logging;

namespace Dressca.Store.Assets.StaticFiles.Constants;

/// <summary>
/// イベントIDを管理するクラスです。
/// </summary>
internal static class Events
{
    /// <summary>
    /// アセットIDに紐づく画像ファイルが見つからなかったこと示すイベントID
    /// </summary>
    internal static readonly EventId AssetImageFileNotFound = new(1001, nameof(AssetImageFileNotFound));

    /// <summary>
    /// デバッグ用のイベントID
    /// </summary>
    internal static readonly EventId DebugEvent = new(9999, nameof(DebugEvent));
}
