using Dressca.AssetsManagement.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Dressca.AssetsManagement.Infrastructures;

/// <summary>
///  <see cref="AssetsManagementDbContext"/> のヘルスチェックを実装します。
/// </summary>
internal class AssetsManagementDbContextHealthCheck : IHealthCheck
{
    private readonly AssetsManagementDbContext dbContext;
    private readonly ILogger<AssetsManagementDbContextHealthCheck> logger;

    /// <summary>
    ///  <see cref="AssetsManagementDbContextHealthCheck"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="dbContext">データアクセスに使用する <see cref="AssetsManagementDbContext"/> オブジェクト。</param>
    /// <param name="logger">ロガー。</param>
    public AssetsManagementDbContextHealthCheck(
        AssetsManagementDbContext dbContext,
        ILogger<AssetsManagementDbContextHealthCheck> logger)
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
