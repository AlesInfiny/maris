using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Dressca.EfInfrastructure;

/// <summary>
///  <see cref="IHealthChecksBuilder"/> の拡張メソッドを提供します。
/// </summary>
public static class HealthChecksBuilderExtensions
{
    extension(IHealthChecksBuilder builder)
    {
        /// <summary>
        ///  <see cref="DresscaDbContext"/> のヘルスチェックを追加します。
        /// </summary>
        /// <param name="name">ヘルスチェックの名称。</param>
        /// <param name="failureStatus">ヘルスチェックが失敗した場合の<see cref="HealthStatus"/> 。</param>
        /// <param name="tags">ヘルスチェックのタグ。</param>
        /// <returns><see cref="DresscaDbContext"/> のヘルスチェックを実装した<see cref="IHealthChecksBuilder"/>。</returns>
        public IHealthChecksBuilder AddDresscaDbContextCheck(string name, HealthStatus? failureStatus = default, IEnumerable<string>? tags = default)
        {
            return builder.AddCheck<DresscaDbContextHealthCheck>(
                name,
                failureStatus,
                tags ?? []);
        }
    }
}
