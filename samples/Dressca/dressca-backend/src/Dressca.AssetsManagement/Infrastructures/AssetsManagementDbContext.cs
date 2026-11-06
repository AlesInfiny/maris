using Dressca.AssetsManagement.ApplicationCore;
using Dressca.AssetsManagement.Infrastructures.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Dressca.AssetsManagement.Infrastructures;

/// <summary>
///  アセット管理コンテキストのデータベースにアクセスする <see cref="DbContext" /> です。
/// </summary>
internal class AssetsManagementDbContext : DbContext
{
    /// <summary>
    ///  <see cref="AssetsManagementDbContext" /> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public AssetsManagementDbContext()
    {
    }

    /// <summary>
    ///  オプションを指定して
    ///  <see cref="AssetsManagementDbContext" /> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="options">オプション。</param>
    public AssetsManagementDbContext(DbContextOptions<AssetsManagementDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    ///  アセットを取得します。
    /// </summary>
    public DbSet<Asset> Assets { get; set; }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Dressca.Eshop;Integrated Security=True");
        }
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new AssetConfiguration());
    }
}
