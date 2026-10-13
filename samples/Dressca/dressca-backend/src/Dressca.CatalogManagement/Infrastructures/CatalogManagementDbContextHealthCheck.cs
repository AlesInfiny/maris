using Dressca.CatalogManagement.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Dressca.CatalogManagement.Infrastructures;

/// <summary>
///  <see cref="CatalogManagementDbContext"/> のヘルスチェックを実装します。
/// </summary>
internal class CatalogManagementDbContextHealthCheck : IHealthCheck
{
    private readonly CatalogManagementDbContext dbContext;
    private readonly ILogger<CatalogManagementDbContextHealthCheck> logger;

    /// <summary>
    ///  <see cref="CatalogManagementDbContextHealthCheck"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="dbContext">データアクセスに使用する <see cref="CatalogManagementDbContext"/> オブジェクト。</param>
    /// <param name="logger">ロガー。</param>
    public CatalogManagementDbContextHealthCheck(
        CatalogManagementDbContext dbContext,
        ILogger<CatalogManagementDbContextHealthCheck> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    /// <inheritdoc/>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await this.dbContext.Database.ExecuteSqlRawAsync("SELECT 1", cancellationToken);

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            this.logger.Log(
                eventId: Events.FailedDatabaseHealthCheck,
                logLevel: LogLevel.Warning,
                exception: ex,
                message: Messages.FailedDatabaseHealthCheck);
            return new HealthCheckResult(context.Registration.FailureStatus);
        }
    }
}
