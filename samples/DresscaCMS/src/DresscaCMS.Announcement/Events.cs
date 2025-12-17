using Microsoft.Extensions.Logging;

namespace DresscaCMS.Announcement;

/// <summary>
/// イベントIDを管理するクラスです。
/// </summary>
internal class Events
{
    /// <summary>
    /// データベースのヘルスチェックに失敗したことを示すイベントID
    /// </summary>
    internal static readonly EventId FailedDatabaseHealthCheck = new(1001, nameof(FailedDatabaseHealthCheck));
}
