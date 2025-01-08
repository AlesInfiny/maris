using Microsoft.Extensions.Logging;

namespace Dressca.Web;

/// <summary>
///  イベント ID を管理するクラスです。
/// </summary>
internal static class Events
{
    /// <summary>
    ///  業務例外が検出されたことを示すイベント ID です。
    /// </summary>
    internal static readonly EventId BusinessExceptionHandled = new(0001, nameof(BusinessExceptionHandled));

    /// <summary>
    ///  データベースの更新の競合が発生したことを示すイベント ID です。
    /// </summary>
    internal static readonly EventId DbUpdateConcurrencyOccurred = new(1001, nameof(DbUpdateConcurrencyOccurred));
}
