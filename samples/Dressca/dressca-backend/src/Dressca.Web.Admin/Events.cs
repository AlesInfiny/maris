namespace Dressca.Web.Admin;

/// <summary>
/// イベントIDを管理するクラスです。
/// </summary>
internal static class Events
{
    /// <summary>
    /// 業務例外が検出されたことを示すイベントID
    /// </summary>
    internal static readonly EventId BusinessExceptionHandled = new(1001, nameof(BusinessExceptionHandled));

    /// <summary>
    /// 試行した操作を実行する権限がないことを示すイベントID。
    /// </summary>
    internal static readonly EventId PermissionDenied = new(1002, nameof(PermissionDenied));

    /// <summary>
    /// 指定したカタログアイテムがレポジトリに存在しないことを示すイベントID。
    /// </summary>
    internal static readonly EventId CatalogItemNotExistingInRepository = new(1003, nameof(CatalogItemNotExistingInRepository));

    /// <summary>
    /// デバッグ用のイベントID
    /// </summary>
    internal static readonly EventId DebugEvent = new(9999, nameof(DebugEvent));
}
