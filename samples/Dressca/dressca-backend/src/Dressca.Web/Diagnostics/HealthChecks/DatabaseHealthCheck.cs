using System.Data.Common;
using Dressca.ApplicationCore.Diagnostics;
using Dressca.EfInfrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Dressca.Web.Diagnostics.HealthChecks;

/// <summary>
/// データベースのヘルスチェックを行うクラスです。
/// </summary>
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IDresscaHealthChecker healthChecker;

    /// <summary>
    ///  <see cref="DatabaseHealthCheck"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="healthChecker">ヘルスチェッカー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="healthChecker"/> が <see langword="null"/> です。
    /// </exception>
    public DatabaseHealthCheck(IDresscaHealthChecker healthChecker)
        => this.healthChecker = healthChecker ?? throw new ArgumentNullException(nameof(healthChecker));

    /// <summary>
    /// データベースへの接続を確認します。
    /// </summary>
    /// <param name="context">ヘルスチェックコンテキスト。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>ヘルスチェックの結果。</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await this.healthChecker.IsHealthyAsync(cancellationToken);
        }
        catch (DbException ex)
        {
            return new HealthCheckResult(status: context.Registration.FailureStatus, exception: ex);
        }

        return HealthCheckResult.Healthy();
    }
}
