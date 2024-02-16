using Dressca.ApplicationCore.Assets;
using Dressca.Web.Assets;
using Dressca.Web.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Dressca.Web.Controllers;

/// <summary>
///  <see cref="Asset"/> の情報にアクセスする API コントローラーです。
/// </summary>
[Route("api/assets")]
[ApiController]
public class AssetsController : ControllerBase
{
    private readonly AssetApplicationService service;
    private readonly ILogger<AssetsController> logger;

    /// <summary>
    ///  <see cref="AssetsController"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="service">アセットアプリケーションサービス。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="service"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public AssetsController(
        AssetApplicationService service,
        ILogger<AssetsController> logger)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///  アセットを取得します。
    /// </summary>
    /// <param name="assetCode">アセットコード。</param>
    /// <returns>アセットのファイル。</returns>
    /// <response code="200">成功。</response>
    /// <response code="404">アセットコードに対応するアセットがない。</response>
    [HttpGet("{assetCode}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Get(string assetCode)
    {
        try
        {
            var assetStreamInfo = this.service.GetAssetStreamInfo(assetCode);
            var contentType = assetStreamInfo.Asset.GetContentType();
            return this.File(assetStreamInfo.Stream, contentType);
        }
        catch (AssetNotFoundException ex)
        {
            this.logger.LogWarning(WebLogEvents.AssetNotFound, ex, ex.Message);
            return this.NotFound();
        }
    }
}
