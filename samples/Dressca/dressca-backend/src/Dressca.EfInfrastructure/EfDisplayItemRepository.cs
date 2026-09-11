using System.Linq.Expressions;
using Dressca.ApplicationCore.DisplayItems;
using Microsoft.EntityFrameworkCore;

namespace Dressca.EfInfrastructure;

/// <summary>カタログの情報を陳列品として読み取るリポジトリです。</summary>
internal class EfDisplayItemRepository : IDisplayItemRepository
{
    private readonly DresscaDbContext dbContext;

    /// <summary>リポジトリを初期化します。</summary>
    /// <param name="dbContext">データベースコンテキスト。</param>
    public EfDisplayItemRepository(DresscaDbContext dbContext)
        => this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    /// <inheritdoc/>
    public Task<int> CountAsync(Expression<Func<DisplayItem, bool>> specification, CancellationToken cancellationToken = default)
        => this.Query().CountAsync(specification, cancellationToken);

    /// <inheritdoc/>
    public Task<IReadOnlyList<DisplayItem>> FindAsync(Expression<Func<DisplayItem, bool>> specification, CancellationToken cancellationToken = default)
        => this.FindAsync(specification, 0, 0, cancellationToken);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<DisplayItem>> FindAsync(Expression<Func<DisplayItem, bool>> specification, int skip, int take, CancellationToken cancellationToken = default)
    {
        IQueryable<DisplayItem> query = this.Query().Where(specification).OrderBy(item => item.Id);
        query = skip > 0 ? query.Skip(skip) : query;
        query = take > 0 ? query.Take(take) : query;
        return await query.ToListAsync(cancellationToken);
    }

    private IQueryable<DisplayItem> Query()
        => this.dbContext.DisplayItems.AsNoTracking().Select(item => new DisplayItem
        {
            Id = item.Id,
            Name = item.CatalogItem.Name,
            Description = item.CatalogItem.Description,
            Price = item.CatalogItem.Price,
            ProductCode = item.CatalogItem.ProductCode,
            DisplayItemBrandId = item.CatalogItem.CatalogBrandId,
            DisplayItemCategoryId = item.CatalogItem.CatalogCategoryId,
            IsDeleted = item.CatalogItem.IsDeleted,
            Assets = item.CatalogItem.Assets.OrderBy(asset => asset.Id).Select(asset => new DisplayItemAsset
            {
                DisplayItemId = item.Id,
                AssetCode = asset.AssetCode,
            }).ToList(),
        });
}
