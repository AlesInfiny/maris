namespace Dressca.Web.Admin;

/// <summary>
///  イベント ID を管理するクラスです。
/// </summary>
internal static class Events
{
    /// <summary>
    ///  クライアント側から不正な HTTP リクエストを受信したことを示すイベント ID です。
    /// </summary>
    internal static readonly EventId ReceiveHttpBadRequest = new(1001, nameof(ReceiveHttpBadRequest));

    /// <summary>
    ///  試行した操作を実行する権限がないことを示すイベント ID です。
    /// </summary>
    internal static readonly EventId PermissionDenied = new(1002, nameof(PermissionDenied));

    /// <summary>
    ///  指定したカタログアイテムがレポジトリに存在しないことを示すイベント ID です。
    /// </summary>
    internal static readonly EventId CatalogItemNotExistingInRepository = new(1003, nameof(CatalogItemNotExistingInRepository));

    /// <summary>
    ///  アセットが見つからなかったことを示すイベント ID です。
    /// </summary>
    internal static readonly EventId AssetNotFound = new(1004, nameof(AssetNotFound));

    /// <summary>
    /// カタログアイテムの削除に失敗したことを示すイベント ID です。
    /// </summary>
    internal static readonly EventId CatalogItemNotDeleted = new(1005, nameof(CatalogItemNotDeleted));

    /// <summary>
    ///  デバッグ用のイベント ID です。
    /// </summary>
    internal static readonly EventId DebugEvent = new(9999, nameof(DebugEvent));
}
