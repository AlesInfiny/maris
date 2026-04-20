using DresscaCMS.Announcement.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace DresscaCMS.Announcement.Infrastructures;

/// <summary>
///  <see cref="AnnouncementDbContext"/> のヘルスチェックを実装します。
/// </summary>
internal class AnnouncementDbContextHealthCheck : IHealthCheck
{
    private readonly IDbContextFactory<AnnouncementDbContext> dbContextFactory;
    private readonly ILogger<AnnouncementDbContextHealthCheck> logger;

    /// <summary>
    ///  <see cref="AnnouncementDbContextHealthCheck"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="dbContextFactory">データアクセスに使用する <see cref="IDbContextFactory{AnnouncementDbContext}"/> オブジェクト。</param>
    /// <param name="logger">ロガー。</param>
    public AnnouncementDbContextHealthCheck(
        IDbContextFactory<AnnouncementDbContext> dbContextFactory,
        ILogger<AnnouncementDbContextHealthCheck> logger)
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
