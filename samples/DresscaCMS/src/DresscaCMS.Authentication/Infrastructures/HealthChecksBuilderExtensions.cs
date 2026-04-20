using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DresscaCMS.Authentication.Infrastructures;

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
        ///  <see cref="AuthenticationDbContext"/> のヘルスチェックを追加します。
        /// </summary>
        /// <param name="name">ヘルスチェックの名称。</param>
        /// <param name="failureStatus">ヘルスチェックが失敗した場合の<see cref="HealthStatus"/> 。</param>
        /// <param name="tags">ヘルスチェックのタグ。</param>
        /// <returns><see cref="AuthenticationDbContext"/> のヘルスチェックを実装した<see cref="IHealthChecksBuilder"/>。</returns>
        public IHealthChecksBuilder AddAuthenticationDbContextCheck(string name, HealthStatus? failureStatus = default, IEnumerable<string>? tags = default)
        {
            return builder.AddCheck<AuthenticationDbContextHealthCheck>(
                name,
                failureStatus,
                tags ?? []);
        }
    }
}
