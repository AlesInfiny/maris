using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Dressca.EfInfrastructure;

/// <summary>
///  <see cref="IHealthChecksBuilder"/> の拡張メソッドを提供します。
/// </summary>
public static class HealthChecksBuilderExtensions
{
    /// <summary>
    ///  <see cref="DresscaDbContext"/> のヘルスチェックを追加します。
    /// </summary>
    /// <param name="builder"><see cref="IHealthChecksBuilder"/>。</param>
    /// <param name="name">ヘルスチェックの名称。</param>
    /// <param name="failureStatus">ヘルスチェックが失敗した場合の<see cref="HealthStatus"/> 。</param>
    /// <param name="tags">ヘルスチェックのタグ。</param>
    /// <returns><see cref="DresscaDbContext"/> のヘルスチェックを実装した<see cref="IHealthChecksBuilder"/>。</returns>
    public static IHealthChecksBuilder AddDresscaDbContextCheck(this IHealthChecksBuilder builder, string? name = null, HealthStatus? failureStatus = default, IEnumerable<string>? tags = default)
    {
        return builder.AddDbContextCheck<DresscaDbContext>(
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
                    Console.WriteLine(ex);
                    return false;
                }
            });
    }
}
