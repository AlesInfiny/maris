﻿using Dressca.ApplicationCore.Catalog;
using Microsoft.EntityFrameworkCore;

namespace Dressca.EfInfrastructure;

/// <summary>
///  Entity Framework Core を利用したカタログブランドリポジトリの実装です。
/// </summary>
internal class EfCatalogBrandRepository : ICatalogBrandRepository
{
    private readonly DresscaDbContext dbContext;

    /// <summary>
    ///  データアクセスに使用する <see cref="DresscaDbContext"/> を指定して
    ///  <see cref="EfCatalogBrandRepository"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="dbContext">データアクセスに使用する <see cref="DresscaDbContext"/> オブジェクト。</param>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="dbContext"/> が <see langword="null"/> です。
    /// </exception>
    public EfCatalogBrandRepository(DresscaDbContext dbContext)
        => this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    /// <inheritdoc/>
    public async Task<IReadOnlyList<CatalogBrand>> GetAllAsync(CancellationToken cancellationToken = default)
        => await this.dbContext.CatalogBrands.ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public Task<CatalogBrand?> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        var keys = new object[] { id };
        return this.dbContext.CatalogBrands.FindAsync(keys, cancellationToken).AsTask();
    }
}
