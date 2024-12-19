using Dressca.ApplicationCore.Catalog;
using Microsoft.EntityFrameworkCore;

namespace Dressca.EfInfrastructure;

/// <summary>
///  Entity Framework Core を利用したカタログカテゴリリポジトリの実装です。
/// </summary>
internal class EfCatalogCategoryRepository : ICatalogCategoryRepository
{
    private readonly DresscaDbContext dbContext;

    /// <summary>
    ///  データアクセスに使用する <see cref="DresscaDbContext"/> を指定して
    ///  <see cref="EfCatalogCategoryRepository"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="dbContext">データアクセスに使用する <see cref="DresscaDbContext"/> オブジェクト。</param>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="dbContext"/> が <see langword="null"/> です。
    /// </exception>
    public EfCatalogCategoryRepository(DresscaDbContext dbContext)
        => this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    /// <inheritdoc/>
    public async Task<IReadOnlyList<CatalogCategory>> GetAllAsync(CancellationToken cancellationToken = default)
        => await this.dbContext.CatalogCategories.ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task<CatalogCategory?> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        var keys = new object[] { id };
        return await this.dbContext.CatalogCategories.FindAsync(keys, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> AnyAsync(long id, CancellationToken cancellationToken = default)
    {
        return await this.dbContext.CatalogCategories.AnyAsync(catalogCategory => catalogCategory.Id == id, cancellationToken);
    }
}
