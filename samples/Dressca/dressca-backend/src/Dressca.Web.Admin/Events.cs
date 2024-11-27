namespace Dressca.Web.Admin;

/// <summary>
/// イベントIDを管理するクラスです。
/// </summary>
internal static class Events
{
    /// <summary>
    /// 試行した操作を実行する権限がないことを示すイベントID。
    /// </summary>
    internal static readonly EventId PermissionDenied = new(1001, nameof(PermissionDenied));

    /// <summary>
    /// 指定したカタログアイテムがレポジトリに存在しないことを示すイベントID。
    /// </summary>
    internal static readonly EventId CatalogItemNotExistingInRepository = new(1002, nameof(CatalogItemNotExistingInRepository));

    /// <summary>
    ///  データベースの更新の競合が発生したことを示すイベントID。
    /// </summary>
    internal static readonly EventId DbUpdateConcurrencyOccured = new(1003, nameof(DbUpdateConcurrencyOccured));

    /// <summary>
    /// デバッグ用のイベントID
    /// </summary>
    internal static readonly EventId DebugEvent = new(9999, nameof(DebugEvent));
}
