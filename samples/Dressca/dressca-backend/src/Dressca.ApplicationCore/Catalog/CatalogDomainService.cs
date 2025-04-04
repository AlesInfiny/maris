﻿using Dressca.ApplicationCore.Resources;
using Microsoft.Extensions.Logging;

namespace Dressca.ApplicationCore.Catalog;

/// <summary>
///  カタログに関するドメインサービスの実装です。
/// </summary>
internal class CatalogDomainService : ICatalogDomainService
{
    private readonly ICatalogRepository catalogRepository;
    private readonly ICatalogBrandRepository catalogBrandRepository;
    private readonly ICatalogCategoryRepository catalogCategoryRepository;
    private readonly ILogger<CatalogDomainService> logger;

    /// <summary>
    ///  <see cref="CatalogDomainService"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="catalogRepository">カタログリポジトリ。</param>
    /// <param name="catalogBrandRepository">カタログブランドレポジトリ。</param>
    /// <param name="catalogCategoryRepository">カタログカテゴリレポジトリ。</param>
    /// <param name="logger">ロガー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="catalogRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="catalogBrandRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="catalogCategoryRepository"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="logger"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public CatalogDomainService(
        ICatalogRepository catalogRepository,
        ICatalogBrandRepository catalogBrandRepository,
        ICatalogCategoryRepository catalogCategoryRepository,
        ILogger<CatalogDomainService> logger)
    {
        this.catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
        this.catalogBrandRepository = catalogBrandRepository ?? throw new ArgumentNullException(nameof(catalogBrandRepository));
        this.catalogCategoryRepository = catalogCategoryRepository ?? throw new ArgumentNullException(nameof(catalogCategoryRepository));
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
        if (notExistsCatalogItemIds.Length != 0)
        {
            this.logger.LogInformation(
                Events.CatalogItemIdDoesNotExistInRepository,
                LogMessages.CatalogItemIdDoesNotExistInRepository,
                notExistsCatalogItemIds);
            return (ExistsAll: false, CatalogItems: items);
        }
        else
        {
            return (ExistsAll: true, CatalogItems: items);
        }
    }

    /// <inheritdoc/>
    public async Task<bool> ItemExistsAsync(long catalogItemId, CancellationToken cancellationToken = default)
    {
        return await this.catalogRepository.AnyAsync(catalogItemId, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> BrandExistsAsync(long catalogBrandId, CancellationToken cancellationToken = default)
    {
        return await this.catalogBrandRepository.AnyAsync(catalogBrandId, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> CategoryExistsAsync(long catalogCategoryId, CancellationToken cancellationToken = default)
    {
        return await this.catalogCategoryRepository.AnyAsync(catalogCategoryId, cancellationToken);
    }
}
