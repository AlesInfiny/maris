using Dressca.ApplicationCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Dressca.EfInfrastructure;

/// <summary>
///  Entity Framework Core を利用してデータベースのヘルスチェックを実装します。
/// </summary>
internal class DresscaHealthChecker : IDresscaHealthChecker
{
    private readonly DresscaDbContext dbContext;

    /// <summary>
    ///  <see cref="DresscaHealthChecker"/> の新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="dbContext">データアクセスに使用する <see cref="DresscaDbContext"/> オブジェクト。</param>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="dbContext"/> が <see langword="null"/> です。
    /// </exception>
    public DresscaHealthChecker(DresscaDbContext dbContext)
        => this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    /// <inheritdoc/>
    public async Task<bool> IsHealthyAsync(CancellationToken token)
    {
        await this.dbContext.Database.ExecuteSqlRawAsync("SELECT 1", token);

        return true;
    }
}
