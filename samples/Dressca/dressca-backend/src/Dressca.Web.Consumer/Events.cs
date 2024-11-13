namespace Dressca.Web.Consumer;

/// <summary>
/// イベントIDを管理するクラスです。
/// </summary>
internal static class Events
{
    /// <summary>
    /// クライアント側から不正なHTTPリクエストを受信したことを示すイベントID
    /// </summary>
    internal static readonly EventId ReceiveHttpBadRequest = new(1101, nameof(ReceiveHttpBadRequest));

    /// <summary>
    /// アセットが見つからなかったことを示すイベントID
    /// </summary>
    internal static readonly EventId AssetNotFound = new(1201, nameof(AssetNotFound));

    /// <summary>
    /// 指定された商品が買い物かご内に存在しなかったことを示すイベントID
    /// </summary>
    internal static readonly EventId CatalogItemIdDoesNotExistInBasket = new(1301, nameof(CatalogItemIdDoesNotExistInBasket));

    /// <summary>
    /// 注文情報が見つからなかったことを示すイベントID
    /// </summary>
    internal static readonly EventId OrderNotFound = new(1401, nameof(OrderNotFound));

    /// <summary>
    /// デバッグ用のイベントID
    /// </summary>
    internal static readonly EventId DebugEvent = new(9999, nameof(DebugEvent));
}
