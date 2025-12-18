using System.Diagnostics.CodeAnalysis;
using DresscaCMS.Announcement.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace DresscaCMS.Announcement.Infrastructures;

/// <summary>
///  <see cref="IHealthChecksBuilder"/> の拡張メソッドを提供します。
/// </summary>
[SuppressMessage(
    category: "StyleCop.CSharp.ReadabilityRules",
    checkId: "SA1101:PrefixLocalCallsWithThis",
    Justification = "StyleCop bug. see: https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3954")]
public static class HealthChecksBuilderExtensions
{
    extension(IHealthChecksBuilder builder)
    {
        /// <summary>
        ///  <see cref="AnnouncementDbContext"/> のヘルスチェックを追加します。
        /// </summary>
        /// <param name="name">ヘルスチェックの名称。</param>
        /// <param name="failureStatus">ヘルスチェックが失敗した場合の<see cref="HealthStatus"/> 。</param>
        /// <param name="tags">ヘルスチェックのタグ。</param>
        /// <param name="logLevel">ログレベル。</param>
        /// <returns><see cref="AnnouncementDbContext"/> のヘルスチェックを実装した<see cref="IHealthChecksBuilder"/>。</returns>
        public IHealthChecksBuilder AddAnnouncementDbContextCheck(string? name = null, HealthStatus? failureStatus = default, IEnumerable<string>? tags = default, LogLevel logLevel = LogLevel.Warning)
        {
            return builder.AddDbContextCheck<AnnouncementDbContext>(
            name,
            failureStatus,
            tags,
            async (context, token) =>
            {
                try
                {
                    await context.Database.ExecuteSqlRawAsync("SELECT 1", token);
                    return true;
                }
                catch (Exception ex)
                {
                    var loggerFactory = builder.Services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
                    var logger = loggerFactory.CreateLogger("DresscaCMS.Announcement.Infrastructures.HealthChecksBuilderExtensions");
                    logger.Log(
                        eventId: Events.FailedDatabaseHealthCheck,
                        logLevel: logLevel,
                        exception: ex,
                        message: Messages.FailedDatabaseHealthCheck);
                    return false;
                }
            });
        }
    }
}
