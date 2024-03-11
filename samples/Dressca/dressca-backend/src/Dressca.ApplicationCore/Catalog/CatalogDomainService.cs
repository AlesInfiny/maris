using Dressca.ApplicationCore.Resources;
using Microsoft.Extensions.Logging;

namespace Dressca.ApplicationCore.Catalog;

/// <summary>
///  カタログに関するドメインサービスを提供します。
/// </summary>
internal class CatalogDomainService : ICatalogDomainService
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
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<(bool ExistsAll, IReadOnlyList<CatalogItem> CatalogItems)> ExistsAllAsync(IEnumerable<long> catalogItemIds, CancellationToken cancellationToken = default)
    {
        var items =
            await this.catalogRepository.FindAsync(
                catalogItem => catalogItemIds.Contains(catalogItem.Id),
                cancellationToken);
        var notExistsCatalogItemIds = catalogItemIds
            .Where(catalogItemId => !items.Any(catalogItem => catalogItem.Id == catalogItemId))
            .ToArray();
        if (notExistsCatalogItemIds.Any())
        {
            this.logger.LogInformation(
                Events.CatalogItemIdDoesNotExistInRepository,
                Messages.CatalogItemIdDoesNotExistInRepository,
                notExistsCatalogItemIds);
            return (ExistsAll: false, CatalogItems: items);
        }
        else
        {
            return (ExistsAll: true, CatalogItems: items);
        }
    }
}
