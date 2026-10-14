using Dressca.Shopping.ApplicationCore.Baskets;
using Dressca.Shopping.ApplicationCore.Ordering;
using Dressca.Shopping.Infrastructures.Configurations;
using Dressca.Shopping.Infrastructures.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dressca.Shopping.Infrastructures;

/// <summary>
///  買い物コンテキストのデータベースにアクセスする <see cref="DbContext" /> です。
/// </summary>
internal class ShoppingDbContext : DbContext
{
    /// <summary>
    ///  <see cref="ShoppingDbContext" /> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public ShoppingDbContext()
    {
    }

    /// <summary>
    ///  オプションを指定して
    ///  <see cref="ShoppingDbContext" /> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="options">オプション。</param>
    public ShoppingDbContext(DbContextOptions<ShoppingDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    ///  買い物かごを取得します。
    /// </summary>
    public DbSet<Basket> Baskets { get; set; }

    /// <summary>
    ///  買い物かごアイテムを取得します。
    /// </summary>
    public DbSet<BasketItem> BasketItems { get; set; }

    /// <summary>
    ///  注文を取得します。
    /// </summary>
    public DbSet<Order> Orders { get; set; }

    /// <summary>
    ///  注文アイテムを取得します。
    /// </summary>
    public DbSet<OrderItem> OrderItems { get; set; }

    /// <summary>
    ///  注文アイテムアセットを取得します。
    /// </summary>
    public DbSet<OrderItemAsset> OrderItemAssets { get; set; }

    /// <summary>陳列品とカタログの対応を取得します。</summary>
    public DbSet<DisplayItemEntity> DisplayItems { get; set; }

    /// <summary>
    ///  陳列品の情報源となるカタログアイテムを取得します。
    ///  カタログ管理コンテキストが所有するテーブルのため、読み取り専用で使用します。
    /// </summary>
    public DbSet<CatalogItemEntity> CatalogItems { get; set; }

    /// <summary>
    ///  陳列品の情報源となる商品ブランドを取得します。
    ///  カタログ管理コンテキストが所有するテーブルのため、読み取り専用で使用します。
    /// </summary>
    public DbSet<CatalogBrandEntity> CatalogBrands { get; set; }

    /// <summary>
    ///  陳列品の情報源となる商品カテゴリを取得します。
    ///  カタログ管理コンテキストが所有するテーブルのため、読み取り専用で使用します。
    /// </summary>
    public DbSet<CatalogCategoryEntity> CatalogCategories { get; set; }

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

        // 買い物かご
        modelBuilder.ApplyConfiguration(new BasketConfiguration());
        modelBuilder.ApplyConfiguration(new BasketItemConfiguration());

        // 陳列品
        modelBuilder.ApplyConfiguration(new DisplayItemConfiguration());

        // カタログ（読み取り専用）
        modelBuilder.ApplyConfiguration(new CatalogItemEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogItemAssetEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogBrandEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogCategoryEntityConfiguration());

        // 注文
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemAssetConfiguration());
    }
}
