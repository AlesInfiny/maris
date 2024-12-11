using System.Linq.Expressions;
using Dressca.ApplicationCore.Catalog;
using Microsoft.EntityFrameworkCore;

namespace Dressca.EfInfrastructure;

/// <summary>
///  Entity Framework Core を利用したカタログリポジトリの実装です。
/// </summary>
internal class EfCatalogRepository : ICatalogRepository
{
    private readonly DresscaDbContext dbContext;

    /// <summary>
    ///  データアクセスに使用する <see cref="DresscaDbContext"/> を指定して
    ///  <see cref="EfCatalogRepository"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="dbContext">データアクセスに使用する <see cref="DresscaDbContext"/> オブジェクト。</param>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="dbContext"/> が <see langword="null"/> です。
    /// </exception>
    public EfCatalogRepository(DresscaDbContext dbContext)
        => this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    /// <inheritdoc/>
    public Task<int> CountAsync(Expression<Func<CatalogItem, bool>> specification, CancellationToken cancellationToken = default)
        => this.dbContext.CatalogItems.CountAsync(specification, cancellationToken);

    /// <inheritdoc/>
    public Task<IReadOnlyList<CatalogItem>> FindAsync(Expression<Func<CatalogItem, bool>> specification, CancellationToken cancellationToken = default)
        => this.FindAsync(specification, 0, 0, cancellationToken);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<CatalogItem>> FindAsync(Expression<Func<CatalogItem, bool>> specification, int skip, int take, CancellationToken cancellationToken = default)
    {
        IQueryable<CatalogItem> query = this.dbContext.CatalogItems
            .Where(specification)
            .Include(catalogItem => catalogItem.Assets)
            .OrderBy(item => item.Id);

        query = skip > 0 ? query.Skip(skip) : query;
        query = take > 0 ? query.Take(take) : query;

        return await query.ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<CatalogItem?> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        return await this.dbContext.CatalogItems
            .Where(catalogItem => catalogItem.Id == id)
            .Include(catalogItem => catalogItem.Assets)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<int> UpdateAsync(CatalogItem entity, CancellationToken cancellationToken = default)
    {
        using (var updateContext = new DresscaDbContext())
        {
            updateContext.Entry(entity).State = EntityState.Modified;
            return await updateContext.SaveChangesAsync(cancellationToken);
        }
    }

    /// <inheritdoc/>
    public async Task<CatalogItem> AddAsync(CatalogItem entity, CancellationToken cancellationToken = default)
    {
        this.dbContext.CatalogItems.Add(entity);
        _ = await this.dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    /// <inheritdoc/>
    public async Task<int> RemoveAsync(long id, byte[] rowVersion, CancellationToken cancellationToken = default)
    {
        using (var deleteContext = new DresscaDbContext())
        {
            var deleteTarget = new CatalogItem
            {
                Id = id,
                Name = "削除用アイテム",
                Description = "削除用アイテム",
                Price = 0,
                ProductCode = "DELETE",
                CatalogBrandId = 3,
                CatalogCategoryId = 3L,
                RowVersion = rowVersion,
            };
            deleteContext.Entry(deleteTarget).State = EntityState.Deleted;
            return await deleteContext.SaveChangesAsync(cancellationToken);
        }
    }

    /// <inheritdoc/>
    public async Task<bool> AnyAsync(long id, CancellationToken cancellationToken = default)
    {
        return await this.dbContext.CatalogItems.AnyAsync(catalogItem => catalogItem.Id == id, cancellationToken);
    }
}
