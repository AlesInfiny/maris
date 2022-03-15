using Dressca.ApplicationCore.Resources;
using Microsoft.Extensions.Logging;

namespace Dressca.ApplicationCore.Catalog;

/// <summary>
///  カタログに関するドメインサービスを提供します。
/// </summary>
public class CatalogDomainService
{
    private readonly ICatalogRepository catalogRepository;
    private readonly ILogger<CatalogDomainService> logger;

    /// <summary>
    ///  <see cref="CatalogDomainService"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="catalogRepository">カタログリポジトリ。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="catalogRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public CatalogDomainService(
        ICatalogRepository catalogRepository,
        ILogger<CatalogDomainService> logger)
    {
        this.catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(CatalogDomainService.logger));
    }

    /// <summary>
    ///  指定したカタログアイテム Id がリポジトリ内にすべて存在するか示す値を取得します。
    /// </summary>
    /// <param name="catalogItemIds">存在することを確認するカタログアイテム Id 。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>すべて存在する場合は <see langword="true"/> 、一部でも不在の場合は <see langword="false"/> 。</returns>
    public async Task<bool> ExistsAllAsync(IEnumerable<long> catalogItemIds, CancellationToken cancellationToken = default)
    {
        var catalogItems =
            await this.catalogRepository.FindAsync(
                catalogItem => catalogItemIds.Contains(catalogItem.Id),
                cancellationToken);
        var notExistsCatalogItemIds = catalogItemIds
            .Where(catalogItemId => !catalogItems.Any(catalogItem => catalogItem.Id == catalogItemId))
            .ToArray();
        if (notExistsCatalogItemIds.Any())
        {
            this.logger.LogWarning(
                ApplicationCoreMessages.CatalogItemIdDoesNotExistInRepository,
                string.Join(',', notExistsCatalogItemIds));
            return false;
        }
        else
        {
            return true;
        }
    }
}
