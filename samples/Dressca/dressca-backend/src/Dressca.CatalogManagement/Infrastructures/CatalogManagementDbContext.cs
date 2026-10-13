using Dressca.CatalogManagement.ApplicationCore;
using Dressca.CatalogManagement.Infrastructures.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Dressca.CatalogManagement.Infrastructures;

/// <summary>
///  カタログ管理コンテキストのデータベースにアクセスする <see cref="DbContext" /> です。
/// </summary>
internal class CatalogManagementDbContext : DbContext
{
    /// <summary>
    ///  <see cref="CatalogManagementDbContext" /> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public CatalogManagementDbContext()
    {
    }

    /// <summary>
    ///  オプションを指定して
    ///  <see cref="CatalogManagementDbContext" /> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="options">オプション。</param>
    public CatalogManagementDbContext(DbContextOptions<CatalogManagementDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    ///  カタログアイテムを取得します。
    /// </summary>
    public DbSet<CatalogItem> CatalogItems { get; set; }

    /// <summary>
    ///  カタログアイテムアセットを取得します。
    /// </summary>
    public DbSet<CatalogItemAsset> CatalogItemAssets { get; set; }

    /// <summary>
    ///  商品ブランドを取得します。
    /// </summary>
    public DbSet<CatalogBrand> CatalogBrands { get; set; }

    /// <summary>
    ///  商品カテゴリを取得します。
    /// </summary>
    public DbSet<CatalogCategory> CatalogCategories { get; set; }

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

        modelBuilder.ApplyConfiguration(new CatalogBrandConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogItemConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogItemAssetConfiguration());
    }
}
