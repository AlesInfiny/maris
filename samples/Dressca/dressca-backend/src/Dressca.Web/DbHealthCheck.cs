using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Dressca.Web;

/// <summary>
/// データベースのヘルスチェックを行うクラスです。
/// </summary>
public class DbHealthCheck : IHealthCheck
{
    private string connectionString = @"Server=(localdb)\mssqllocaldb;Database=Dressca.Eshop;Integrated Security=True";

    /// <summary>
    /// データベースへの接続を確認します。
    /// </summary>
    /// <param name="context">ヘルスチェックコンテキスト。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>ヘルスチェックの結果。</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        using (var connection = new SqlConnection(this.connectionString))
        {
            try
            {
                await connection.OpenAsync(cancellationToken);

                var command = connection.CreateCommand();
                command.CommandText = "SELECT 1";

                await command.ExecuteNonQueryAsync(cancellationToken);
            }
            catch (DbException ex)
            {
                return new HealthCheckResult(status: context.Registration.FailureStatus, exception: ex);
            }
        }

        return HealthCheckResult.Healthy();
    }
}
