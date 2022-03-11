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
    public async Task<IReadOnlyList<CatalogItem>> AddRangeAsync(IEnumerable<CatalogItem> entities, CancellationToken cancellationToken = default)
    {
        this.dbContext.CatalogItems.AddRange(entities);
        _ = await this.dbContext.SaveChangesAsync(cancellationToken);
        return entities.ToList();
    }

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
            .OrderBy(item => item.Id);

        query = skip > 0 ? query.Skip(skip) : query;
        query = take > 0 ? query.Take(take) : query;

        return await query.ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task<CatalogItem?> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        var keys = new object[] { id };
        return this.dbContext.CatalogItems.FindAsync(keys, cancellationToken).AsTask();
    }
}
