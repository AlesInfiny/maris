using DresscaCMS.Authentication.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace DresscaCMS.Authentication.Infrastructures;

/// <summary>
///  <see cref="AuthenticationDbContext"/> のヘルスチェックを実装します。
/// </summary>
internal class AuthenticationDbContextHealthCheck : IHealthCheck
{
    private readonly IDbContextFactory<AuthenticationDbContext> dbContextFactory;
    private readonly ILogger<AuthenticationDbContextHealthCheck> logger;

    /// <summary>
    ///  <see cref="AuthenticationDbContextHealthCheck"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="dbContextFactory">データアクセスに使用する <see cref="IDbContextFactory{AuthenticationDbContext}"/> オブジェクト。</param>
    /// <param name="logger">ロガー。</param>
    public AuthenticationDbContextHealthCheck(
        IDbContextFactory<AuthenticationDbContext> dbContextFactory,
        ILogger<AuthenticationDbContextHealthCheck> logger)
    {
        this.dbContextFactory = dbContextFactory;
        this.logger = logger;
    }

    /// <inheritdoc/>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var dbContext = await this.dbContextFactory.CreateDbContextAsync(cancellationToken);
            await dbContext.Database.ExecuteSqlRawAsync("SELECT 1", cancellationToken);

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
