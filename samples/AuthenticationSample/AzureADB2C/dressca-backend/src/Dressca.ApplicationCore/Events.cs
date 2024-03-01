using Microsoft.Extensions.Logging;

namespace Dressca.ApplicationCore;

/// <summary>
/// イベントIDを管理するクラスです。
/// </summary>
internal class Events
{
    /// <summary>
    /// カタログIDがレポジトリ内に存在しないことを示すイベントID
    /// </summary>
    internal static readonly EventId CatalogItemIdDoesNotExistInRepository = new(1001, nameof(CatalogItemIdDoesNotExistInRepository));

    /// <summary>
    /// デバッグ用のイベントID
    /// </summary>
    internal static readonly EventId DebugEvent = new(9999, nameof(DebugEvent));
}
