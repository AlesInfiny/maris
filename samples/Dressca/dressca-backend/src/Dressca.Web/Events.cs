using Microsoft.Extensions.Logging;

namespace Dressca.Web;

/// <summary>
/// イベントIDを管理するクラスです。
/// </summary>
internal static class Events
{
    /// <summary>
    /// 業務例外が検出されたことを示すイベントID
    /// </summary>
    internal static readonly EventId BusinessExceptionHandled = new(0001, nameof(BusinessExceptionHandled));
}
