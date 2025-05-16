using Microsoft.Extensions.Logging;

namespace Maris.Samples.Cli;

/// <summary>
///  イベント ID を管理するクラスです。
/// </summary>
internal class Events
{
    /// <summary>
    /// 指定した検索条件で取得した商品情報が 10 件以上であることを示すイベント ID 。
    /// </summary>
    internal static readonly EventId Over10ProductsFoundInRange = new(1001, nameof(Over10ProductsFoundInRange));

    /// <summary>
    /// デバッグ用のイベント ID 。
    /// </summary>
    internal static readonly EventId DebugEvent = new(9999, nameof(DebugEvent));
}
