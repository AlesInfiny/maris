using Dressca.ApplicationCore.Assets;

namespace Dressca.EfInfrastructure;

/// <summary>
///  Entity Framework Core を利用したアセットリポジトリの実装です。
/// </summary>
internal class EfAssetRepository : IAssetRepository
{
    private readonly DresscaDbContext dbContext;

    /// <summary>
    ///  データアクセスに使用する <see cref="DresscaDbContext"/> を指定して
    ///  <see cref="EfAssetRepository"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="dbContext">データアクセスに使用する <see cref="DresscaDbContext"/> オブジェクト。</param>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="dbContext"/> が <see langword="null"/> です。
    /// </exception>
    public EfAssetRepository(DresscaDbContext dbContext)
        => this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    /// <inheritdoc/>
    public Asset? Find(string? assetCode, CancellationToken cancellationToken = default)
    {
        return this.dbContext.Assets
            .Where(asset => asset.AssetCode == assetCode)
            .FirstOrDefault();
    }
}
