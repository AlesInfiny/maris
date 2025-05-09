using Microsoft.Extensions.Logging;

namespace Maris.Samples.ApplicationCore;

/// <summary>
///  イベント ID を管理するクラスです。
/// </summary>
internal class Events
{
    /// <summary>
    /// 指定した商品カテゴリの商品一覧の取得開始を示すイベント ID 。
    /// </summary>
    internal static readonly EventId GetProductsByCategoryStart = new(1001, nameof(GetProductsByCategoryStart));

    /// <summary>
    /// 指定した商品カテゴリの商品一覧の取得終了を示すイベント ID 。
    /// </summary>
    internal static readonly EventId GetProductsByCategoryNormalEnd = new(1002, nameof(GetProductsByCategoryNormalEnd));

    /// <summary>
    /// 指定した単価内の商品一覧の取得開始を示すイベント ID 。
    /// </summary>
    internal static readonly EventId GetProductsByUnitPriceRangeStart = new(1003, nameof(GetProductsByUnitPriceRangeStart));

    /// <summary>
    /// 指定した単価内の商品一覧の取得終了を示すイベント ID 。
    /// </summary>
    internal static readonly EventId GetProductsByUnitPriceRangeNormalEnd = new(1004, nameof(GetProductsByUnitPriceRangeNormalEnd));

    /// <summary>
    /// デバッグ用のイベント ID 。
    /// </summary>
    internal static readonly EventId DebugEvent = new(9999, nameof(DebugEvent));
}
