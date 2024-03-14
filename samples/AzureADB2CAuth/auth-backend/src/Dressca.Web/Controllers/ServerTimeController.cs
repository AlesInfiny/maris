using Dressca.Web.Dto.ServerTime;
using Microsoft.AspNetCore.Mvc;

namespace Dressca.Web.Controllers;

/// <summary>
/// サーバー時間に関する情報にアクセスする API コントローラーです。
/// </summary>
[Route("api/servertime")]
[ApiController]
[Produces("application/json")]
public class ServerTimeController : ControllerBase
{
    private readonly ILogger<ServerTimeController> logger;

    /// <summary>
    /// <see cref="ServerTimeController" /> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="logger">ロガー。</param>
    public ServerTimeController(ILogger<ServerTimeController> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// 現在のサーバー時間を返します。
    /// </summary>
    /// <returns>サーバー時間。</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServerTimeResponse))]
    public async Task<IActionResult> Get()
    {
        var currentTime = DateTimeOffset.Now.ToString("G");
        return this.Ok(new ServerTimeResponse { ServerTime = currentTime });
    }
}
