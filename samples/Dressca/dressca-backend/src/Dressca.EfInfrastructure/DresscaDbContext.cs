﻿using Dressca.ApplicationCore.Assets;
using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.Catalog;
using Dressca.ApplicationCore.Ordering;
using Dressca.EfInfrastructure.Configurations.Assets;
using Dressca.EfInfrastructure.Configurations.Baskets;
using Dressca.EfInfrastructure.Configurations.Catalog;
using Dressca.EfInfrastructure.Configurations.Ordering;
using Microsoft.EntityFrameworkCore;

namespace Dressca.EfInfrastructure;

/// <summary>
///  Dressca オンラインショップのデータベースにアクセスする <see cref="DbContext" /> です。
/// </summary>
internal class DresscaDbContext : DbContext
{
    /// <summary>
    ///  <see cref="DresscaDbContext" /> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public DresscaDbContext()
    {
    }

    /// <summary>
    ///  オプションを指定して
    ///  <see cref="DresscaDbContext" /> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="options">オプション。</param>
    public DresscaDbContext(DbContextOptions options)
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

        // 買い物かご
        modelBuilder.ApplyConfiguration(new BasketConfiguration());
        modelBuilder.ApplyConfiguration(new BasketItemConfiguration());

        // カタログ
        modelBuilder.ApplyConfiguration(new CatalogBrandConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogItemConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogItemAssetConfiguration());

        // 注文
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemAssetConfiguration());

        // アセット
        modelBuilder.ApplyConfiguration(new AssetConfiguration());
    }
}
