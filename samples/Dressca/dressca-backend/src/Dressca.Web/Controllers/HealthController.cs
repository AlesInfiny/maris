using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Dressca.Web.Controllers;

/// <summary>
/// ヘルスチェック用のコントローラー。
/// </summary>
[Route("api/health")]
[ApiController]
public class HealthController : ControllerBase
{
    private readonly HealthCheckService healthCheckService;

    /// <summary>
    /// コンストラクター。
    /// </summary>
    /// <param name="healthCheckService">ヘルスチェックサービス。</param>
    public HealthController(HealthCheckService healthCheckService)
    {
        this.healthCheckService = healthCheckService;
    }

    /// <summary>
    /// ヘルスチェックの結果を返します。
    /// </summary>
    /// <returns>ヘルスチェックの結果。</returns>
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        HealthReport report = await this.healthCheckService.CheckHealthAsync();

        var result = new
        {
            status = report.Status.ToString(),
            result = report.Entries
            .Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                description = entry.Value.Description?.ToString(),
                data = entry.Value.Data,
                exception = new
                {
                    source = entry.Value.Exception?.Source,
                    message = entry.Value.Exception?.Message,
                    stackTrace = entry.Value.Exception?.StackTrace,
                },
            }),
        };
        return report.Status == HealthStatus.Healthy ? this.Ok(result) : this.StatusCode((int)HttpStatusCode.ServiceUnavailable, result);
    }

    /// <summary>
    /// ヘルスチェックの結果を返します。
    /// </summary>
    /// <returns>ヘルスチェックの結果。</returns>
    [HttpHead]
    public async Task<ActionResult> Head()
    {
        HealthReport report = await this.healthCheckService.CheckHealthAsync();
        return report.Status == HealthStatus.Healthy ? this.Ok() : this.StatusCode((int)HttpStatusCode.ServiceUnavailable);
    }
}
