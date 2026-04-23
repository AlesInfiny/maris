using Dressca.EfInfrastructure.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Dressca.EfInfrastructure;

/// <summary>
///  <see cref="DresscaDbContext"/> のヘルスチェックを実装します。
/// </summary>
internal class DresscaDbContextHealthCheck : IHealthCheck
{
    private readonly DresscaDbContext dbContext;
    private readonly ILogger<DresscaDbContextHealthCheck> logger;

    /// <summary>
    ///  <see cref="DresscaDbContextHealthCheck"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="dbContext">データアクセスに使用する <see cref="DresscaDbContext"/> オブジェクト。</param>
    /// <param name="logger">ロガー。</param>
    public DresscaDbContextHealthCheck(
        DresscaDbContext dbContext,
        ILogger<DresscaDbContextHealthCheck> logger)
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
