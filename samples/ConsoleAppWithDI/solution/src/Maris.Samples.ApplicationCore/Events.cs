using Microsoft.Extensions.Logging;

namespace Maris.Samples.ApplicationCore;

/// <summary>
///  イベント ID を管理するクラスです。
/// </summary>
internal class Events
{

    internal static readonly EventId GetProductsByCategoryStart = new(1001, nameof(GetProductsByCategoryStart));

    internal static readonly EventId GetProductsByCategoryNormalEnd = new(1002, nameof(GetProductsByCategoryNormalEnd));

    internal static readonly EventId GetProductsByUnitPriceRangeStart = new(1003, nameof(GetProductsByUnitPriceRangeStart));

    internal static readonly EventId GetProductsByUnitPriceRangeNormalEnd = new(1004, nameof(GetProductsByUnitPriceRangeNormalEnd));

    internal static readonly EventId DebugEvent = new(9999, nameof(DebugEvent));
}

