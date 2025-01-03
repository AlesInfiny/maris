﻿using Dressca.ApplicationCore.ApplicationService;
using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Consumer.Dto.Catalog;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Dressca.Web.Consumer.Controllers;

/// <summary>
///  <see cref="CatalogBrand"/> の情報にアクセスする API コントローラーです。
/// </summary>
[Route("api/catalog-brands")]
[ApiController]
[Produces("application/json")]
public class CatalogBrandsController : ControllerBase
{
    private readonly CatalogApplicationService service;
    private readonly IObjectMapper<CatalogBrand, CatalogBrandResponse> mapper;

    /// <summary>
    ///  <see cref="CatalogBrandsController"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="service">カタログアプリケーションサービス。</param>
    /// <param name="mapper"><see cref="CatalogBrand"/> と <see cref="CatalogBrandResponse"/> のマッパー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="service"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="mapper"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public CatalogBrandsController(
        CatalogApplicationService service,
        IObjectMapper<CatalogBrand, CatalogBrandResponse> mapper)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    ///  カタログブランドの一覧を取得します。
    /// </summary>
    /// <returns>カタログブランドの一覧。</returns>
    /// <response code="200">成功。</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CatalogBrandResponse>), StatusCodes.Status200OK)]
    [OpenApiOperation("getCatalogBrands")]
    public async Task<IActionResult> GetCatalogBrandsAsync()
    {
        var brands = await this.service.GetBrandsAsync();
        return this.Ok(brands
            .Select(brand => this.mapper.Convert(brand))
            .ToArray());
    }
}
