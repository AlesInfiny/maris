using Microsoft.Extensions.Logging;

namespace Dressca.Web;

/// <summary>
/// イベント ID を管理するクラスです。
/// </summary>
internal static class Events
{
    /// <summary>
    /// 業務例外が検出されたことを示すイベント ID です。
    /// </summary>
    internal static readonly EventId BusinessExceptionHandled = new(0001, nameof(BusinessExceptionHandled));
}
