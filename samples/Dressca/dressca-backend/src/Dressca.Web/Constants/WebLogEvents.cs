namespace Dressca.Web.Constants;

/// <summary>
/// イベントIDを管理するクラスです。
/// </summary>
internal static class WebLogEvents
{
    /// <summary>
    /// 業務例外が検出されたことを示すイベントID
    /// </summary>
    internal static readonly EventId BusinessExceptionHandled = new(1001, "BusinessExceptionHandled");

    /// <summary>
    /// クライアント側から不正なHTTPリクエストを受信したことを示すイベントID
    /// </summary>
    internal static readonly EventId ReceiveHttpBadRequest = new(1101, "ReceiveHttpBadRequest");

    /// <summary>
    /// アセットが見つからなかったことを示すイベントID
    /// </summary>
    internal static readonly EventId AssetNotFound = new(1201, "AssetNotFound");

    /// <summary>
    /// 指定された商品が買い物かご内に存在しなかったことを示すイベントID
    /// </summary>
    internal static readonly EventId CatalogItemIdDoesNotExistInBasket = new(1301, "CatalogItemIdDoesNotExistInBasket");

    /// <summary>
    /// 注文情報が見つからなかったことを示すイベントID
    /// </summary>
    internal static readonly EventId OrderNotFound = new(1401, "OrderNotFound");

    /// <summary>
    /// デバッグ用のイベントID
    /// </summary>
    internal static readonly EventId DebugEvent = new(9999, "DebugEvent");
}
