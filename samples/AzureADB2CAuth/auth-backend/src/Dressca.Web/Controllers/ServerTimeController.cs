using Dressca.Web.Dto.ServerTime;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Dressca.Web.Controllers;

/// <summary>
///  サーバー時間に関する情報にアクセスする API コントローラーです。
/// </summary>
[Route("api/servertime")]
[ApiController]
[Produces("application/json")]
public class ServerTimeController : ControllerBase
{
    private readonly ILogger<ServerTimeController> logger;
    private readonly TimeProvider timeProvider;

    /// <summary>
    ///  <see cref="ServerTimeController"/> の新しいインスタンスを作成します。
    /// </summary>
    /// <param name="logger">ロガー。</param>
    public ServerTimeController(ILogger<ServerTimeController> logger)
        : this(logger, TimeProvider.System)
    {
    }

    /// <summary>
    ///  <see cref="ServerTimeController"/> の新しいインスタンスを作成します。
    /// </summary>
    /// <param name="logger">ロガー。</param>
    /// <param name="timeProvider">タイムプロバイダー。</param>
    internal ServerTimeController(ILogger<ServerTimeController> logger, TimeProvider timeProvider)
    {
        this.logger = logger;
        this.timeProvider = timeProvider;
    }

    /// <summary>
    ///  認証不要で現在のサーバー時間を取得します。
    /// </summary>
    /// <returns>サーバー時間。</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServerTimeResponse))]
    [OpenApiOperation("getServerTime")]
    public IActionResult Get()
    {
        var currentTime = this.timeProvider.GetLocalNow();
        return this.Ok(new ServerTimeResponse { ServerTime = currentTime.ToString("G") });
    }
}
