﻿namespace Dressca.Web.Consumer;

/// <summary>
/// イベントIDを管理するクラスです。
/// </summary>
internal static class Events
{
    /// <summary>
    /// クライアント側から不正なHTTPリクエストを受信したことを示すイベントIDです。
    /// </summary>
    internal static readonly EventId ReceiveHttpBadRequest = new(1001, nameof(ReceiveHttpBadRequest));

    /// <summary>
    /// アセットが見つからなかったことを示すイベントIDです。
    /// </summary>
    internal static readonly EventId AssetNotFound = new(1101, nameof(AssetNotFound));

    /// <summary>
    /// 指定された商品が買い物かご内に存在しなかったことを示すイベントIDです。
    /// </summary>
    internal static readonly EventId CatalogItemIdDoesNotExistInBasket = new(1201, nameof(CatalogItemIdDoesNotExistInBasket));

    /// <summary>
    /// 注文情報が見つからなかったことを示すイベントIDです。
    /// </summary>
    internal static readonly EventId OrderNotFound = new(1301, nameof(OrderNotFound));

    /// <summary>
    /// デバッグ用のイベントIDです。
    /// </summary>
    internal static readonly EventId DebugEvent = new(9999, nameof(DebugEvent));
}
