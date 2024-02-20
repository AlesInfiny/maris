using Microsoft.Extensions.Logging;

namespace Dressca.EfInfrastructure.Constants;

/// <summary>
/// イベントIDを管理するクラスです。
/// </summary>
internal class EfInfrastructureLogEvents
{
    /// <summary>
    /// データベースのヘルスチェックに失敗したことを示すイベントID
    /// </summary>
    internal static readonly EventId FailedDatabaseHealthCheck = new(1001, "FailedDatabaseHealthCheck");
}
