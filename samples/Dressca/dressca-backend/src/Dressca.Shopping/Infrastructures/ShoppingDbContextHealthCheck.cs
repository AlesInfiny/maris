using Dressca.Shopping.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Dressca.Shopping.Infrastructures;

/// <summary>
///  <see cref="ShoppingDbContext"/> のヘルスチェックを実装します。
/// </summary>
internal class ShoppingDbContextHealthCheck : IHealthCheck
{
    private readonly ShoppingDbContext dbContext;
    private readonly ILogger<ShoppingDbContextHealthCheck> logger;

    /// <summary>
    ///  <see cref="ShoppingDbContextHealthCheck"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="dbContext">データアクセスに使用する <see cref="ShoppingDbContext"/> オブジェクト。</param>
    /// <param name="logger">ロガー。</param>
    public ShoppingDbContextHealthCheck(
        ShoppingDbContext dbContext,
        ILogger<ShoppingDbContextHealthCheck> logger)
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
