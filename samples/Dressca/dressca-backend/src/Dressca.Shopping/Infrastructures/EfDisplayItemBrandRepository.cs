using Dressca.Shopping.ApplicationCore.DisplayItems;
using Dressca.Shopping.ApplicationCore.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Dressca.Shopping.Infrastructures;

/// <summary>陳列品のブランドを読み取るリポジトリです。</summary>
internal class EfDisplayItemBrandRepository : IDisplayItemBrandRepository
{
    private readonly ShoppingDbContext dbContext;

    /// <summary>リポジトリを初期化します。</summary>
    /// <param name="dbContext">データベースコンテキスト。</param>
    public EfDisplayItemBrandRepository(ShoppingDbContext dbContext)
        => this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    /// <inheritdoc/>
    public async Task<IReadOnlyList<DisplayItemBrand>> GetAllAsync(CancellationToken cancellationToken = default)
        => await this.dbContext.CatalogBrands.AsNoTracking().OrderBy(item => item.Id)
            .Select(item => new DisplayItemBrand { Id = item.Id, Name = item.Name })
            .ToListAsync(cancellationToken);
}
