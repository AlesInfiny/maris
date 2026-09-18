using Dressca.ApplicationCore.DisplayItems;
using Microsoft.EntityFrameworkCore;

namespace Dressca.EfInfrastructure;

/// <summary>陳列品のカテゴリを読み取るリポジトリです。</summary>
internal class EfDisplayItemCategoryRepository : IDisplayItemCategoryRepository
{
    private readonly DresscaDbContext dbContext;

    /// <summary>リポジトリを初期化します。</summary>
    /// <param name="dbContext">データベースコンテキスト。</param>
    public EfDisplayItemCategoryRepository(DresscaDbContext dbContext)
        => this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    /// <inheritdoc/>
    public async Task<IReadOnlyList<DisplayItemCategory>> GetAllAsync(CancellationToken cancellationToken = default)
        => await this.dbContext.CatalogCategories.AsNoTracking().OrderBy(item => item.Id)
            .Select(item => new DisplayItemCategory { Id = item.Id, Name = item.Name })
            .ToListAsync(cancellationToken);
}
