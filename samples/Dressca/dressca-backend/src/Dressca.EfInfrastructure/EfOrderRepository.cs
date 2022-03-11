using Dressca.ApplicationCore.Ordering;

namespace Dressca.EfInfrastructure;

/// <summary>
///  Entity Framework Core を利用した注文リポジトリの実装です。
/// </summary>
internal class EfOrderRepository : IOrderRepository
{
    private readonly DresscaDbContext dbContext;

    /// <summary>
    ///  データアクセスに使用する <see cref="DresscaDbContext"/> を指定して
    ///  <see cref="EfOrderRepository"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="dbContext">データアクセスに使用する <see cref="DresscaDbContext"/> オブジェクト。</param>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="dbContext"/> が <see langword="null"/> です。
    /// </exception>
    public EfOrderRepository(DresscaDbContext dbContext)
        => this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    /// <inheritdoc/>
    public async Task<Order> AddAsync(Order entity, CancellationToken cancellationToken = default)
    {
        this.dbContext.Orders.Add(entity);
        _ = await this.dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }
}
