using Dressca.AssetsManagement.ApplicationCore;
using Dressca.AssetsManagement.ApplicationCore.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Dressca.AssetsManagement.Infrastructures;

/// <summary>
///  Entity Framework Core を利用したアセットリポジトリの実装です。
/// </summary>
internal class EfAssetRepository : IAssetRepository
{
    private readonly AssetsManagementDbContext dbContext;

    /// <summary>
    ///  データアクセスに使用する <see cref="AssetsManagementDbContext"/> を指定して
    ///  <see cref="EfAssetRepository"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="dbContext">データアクセスに使用する <see cref="AssetsManagementDbContext"/> オブジェクト。</param>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="dbContext"/> が <see langword="null"/> です。
    /// </exception>
    public EfAssetRepository(AssetsManagementDbContext dbContext)
        => this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    /// <inheritdoc/>
    public async Task<Asset?> FindAsync(string? assetCode)
    {
        return await this.dbContext.Assets
            .Where(asset => asset.AssetCode == assetCode)
            .FirstOrDefaultAsync();
    }
}
