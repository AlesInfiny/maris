using Dressca.ApplicationCore.Baskets;
using Microsoft.EntityFrameworkCore;

namespace Dressca.EfInfrastructure;

/// <summary>
///  Entity Framework Core を利用した買い物かごリポジトリの実装です。
/// </summary>
internal class EfBasketRepository : IBasketRepository
{
    private readonly DresscaDbContext dbContext;

    /// <summary>
    ///  データアクセスに使用する <see cref="DresscaDbContext"/> を指定して
    ///  <see cref="EfBasketRepository"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="dbContext">データアクセスに使用する <see cref="DresscaDbContext"/> オブジェクト。</param>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="dbContext"/> が <see langword="null"/> です。
    /// </exception>
    public EfBasketRepository(DresscaDbContext dbContext)
        => this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    /// <inheritdoc/>
    public async Task<Basket> AddAsync(Basket entity, CancellationToken cancellationToken = default)
    {
        this.dbContext.Baskets.Add(entity);
        await this.dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    /// <inheritdoc/>
    public Task<Basket?> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        var keys = new object[] { id };
        return this.dbContext.Baskets.FindAsync(keys, cancellationToken).AsTask();
    }

    /// <inheritdoc/>
    public Task<Basket?> GetWithBasketItemsAsync(long basketId, CancellationToken cancellationToken = default)
    {
        return this.dbContext.Baskets
            .Where(basket => basket.Id == basketId)
            .Include(basket => basket.Items)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task<Basket?> GetWithBasketItemsAsync(string buyerId, CancellationToken cancellationToken = default)
    {
        return this.dbContext.Baskets
            .Where(basket => basket.BuyerId == buyerId)
            .Include(basket => basket.Items)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(Basket entity, CancellationToken cancellationToken = default)
    {
        this.dbContext.Baskets.Remove(entity);
        _ = await this.dbContext.SaveChangesAsync(cancellationToken);
        return;
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Basket entity, CancellationToken cancellationToken = default)
    {
        this.dbContext.Update(entity);
        _ = await this.dbContext.SaveChangesAsync(cancellationToken);
        return;
    }
}
