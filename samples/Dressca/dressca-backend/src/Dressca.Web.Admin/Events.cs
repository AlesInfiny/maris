namespace Dressca.Web.Admin;

/// <summary>
/// イベント ID を管理するクラスです。
/// </summary>
internal static class Events
{
    /// <summary>
    /// 試行した操作を実行する権限がないことを示すイベント ID です。
    /// </summary>
    internal static readonly EventId PermissionDenied = new(1001, nameof(PermissionDenied));

    /// <summary>
    /// 指定したカタログアイテムがレポジトリに存在しないことを示すイベント ID です。
    /// </summary>
    internal static readonly EventId CatalogItemNotExistingInRepository = new(1002, nameof(CatalogItemNotExistingInRepository));

    /// <summary>
    /// デバッグ用のイベント ID です。
    /// </summary>
    internal static readonly EventId DebugEvent = new(9999, nameof(DebugEvent));
}
