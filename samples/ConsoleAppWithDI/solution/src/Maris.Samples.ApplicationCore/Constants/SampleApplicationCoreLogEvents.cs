using Microsoft.Extensions.Logging;

namespace Maris.Samples.ApplicationCore.Constants;
/// <summary>
/// イベントIDを管理するクラスです。
/// </summary>
internal class SampleApplicationCoreLogEvents
{   

    internal static readonly EventId GetProductsByCategoryStart = new (1001, "GetProductsByCategoryStart");
    
    internal static readonly EventId GetProductsByCategoryNormalEnd = new(1002, "GetProductsByCategoryNormalEnd");
    
    internal static readonly EventId GetProductsByUnitPriceRangeStart = new(1003, "GetProductsByUnitPriceRangeStart");
    
    internal static readonly EventId GetProductsByUnitPriceRangeNormalEnd = new(1004, "GetProductsByUnitPriceRangeNormalEnd");
    
    internal static readonly EventId DebugEvent = new(9999, "DebugEvent");
}

