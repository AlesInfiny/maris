using Dressca.ApplicationCore.Catalog;
using Dressca.EfInfrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dressca.EfInfrastructure.Configurations.Catalog;

/// <summary>
///  <see cref="CatalogItem" /> エンティティの構成を提供します。
/// </summary>
internal class CatalogItemConfiguration : IEntityTypeConfiguration<CatalogItem>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("CatalogItems");
        builder.Property(catalogItem => catalogItem.Id)
            .ValueGeneratedNever();
        builder.Property(catalogItem => catalogItem.Name)
            .HasMaxLength(512)
            .IsRequired();
        builder.Property(catalogItem => catalogItem.Description)
            .IsRequired();
        builder.Property(catalogItem => catalogItem.Price)
            .IsRequired()
            .HasColumnType("decimal(18,6)");
        builder.Property(catalogItem => catalogItem.ProductCode)
            .HasMaxLength(128)
            .IsRequired();
        builder.Property(catalogItem => catalogItem.RowVersion)
            .IsRowVersion();
        builder.Property(catalogItem => catalogItem.IsDeleted)
            .IsRequired();
        builder.HasOne(catalogItem => catalogItem.CatalogCategory)
            .WithMany(catalogCategory => catalogCategory.Items)
            .HasForeignKey(catalogItem => catalogItem.CatalogCategoryId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_CatalogItems_CatalogCategories");
        builder.HasOne(catalogItem => catalogItem.CatalogBrand)
            .WithMany(catalogBrand => catalogBrand.Items)
            .HasForeignKey(catalogItem => catalogItem.CatalogBrandId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_CatalogItems_CatalogBrands");

        builder.HasIndex(catalogItem => catalogItem.ProductCode);

        builder.HasData(
        [
            new() { CatalogCategoryId = DresscaSeedIds.Category1, CatalogBrandId = DresscaSeedIds.Brand3, Description = "定番の無地ロングTシャツです。", Name = "クルーネック Tシャツ - ブラック", Price = 1980m, ProductCode = "C000000001", Id = DresscaSeedIds.Item1 },
            new() { CatalogCategoryId = DresscaSeedIds.Category1, CatalogBrandId = DresscaSeedIds.Brand2, Description = "暖かいのに着膨れしない起毛デニムです。", Name = "裏起毛 スキニーデニム", Price = 4800m, ProductCode = "C000000002", Id = DresscaSeedIds.Item2 },
            new() { CatalogCategoryId = DresscaSeedIds.Category1, CatalogBrandId = DresscaSeedIds.Brand1, Description = "あたたかく肌ざわりも良いウール100%のロングコートです。", Name = "ウールコート", Price = 49800m, ProductCode = "C000000003", Id = DresscaSeedIds.Item3 },
            new() { CatalogCategoryId = DresscaSeedIds.Category1, CatalogBrandId = DresscaSeedIds.Brand2, Description = "コットン100%の柔らかい着心地で、春先から夏、秋口まで万能に使いやすいです。", Name = "無地 ボタンダウンシャツ", Price = 2800m, ProductCode = "C000000004", Id = DresscaSeedIds.Item4 },
            new() { CatalogCategoryId = DresscaSeedIds.Category2, CatalogBrandId = DresscaSeedIds.Brand3, Description = "コンパクトサイズのバッグですが収納力は抜群です", Name = "レザーハンドバッグ", Price = 18800m, ProductCode = "B000000001", Id = DresscaSeedIds.Item5 },
            new() { CatalogCategoryId = DresscaSeedIds.Category2, CatalogBrandId = DresscaSeedIds.Brand2, Description = "エイジング加工したレザーを使用しています。", Name = "ショルダーバッグ", Price = 38000m, ProductCode = "B000000002", Id = DresscaSeedIds.Item6 },
            new() { CatalogCategoryId = DresscaSeedIds.Category2, CatalogBrandId = DresscaSeedIds.Brand3, Description = "春の季節にぴったりのトートバッグです。インナーポーチまたは単体でも使用可能なポーチ付。", Name = "トートバッグ ポーチ付き", Price = 24800m, ProductCode = "B000000003", Id = DresscaSeedIds.Item7 },
            new() { CatalogCategoryId = DresscaSeedIds.Category2, CatalogBrandId = DresscaSeedIds.Brand1, Description = "さらりと気軽に纏える、キュートなミニサイズショルダー。", Name = "ショルダーバッグ", Price = 2800m, ProductCode = "B000000004", Id = DresscaSeedIds.Item8 },
            new() { CatalogCategoryId = DresscaSeedIds.Category2, CatalogBrandId = DresscaSeedIds.Brand1, Description = "エレガントな雰囲気を放つキルティングデザインです。", Name = "レザー チェーンショルダーバッグ", Price = 258000m, ProductCode = "B000000005", Id = DresscaSeedIds.Item9 },
            new() { CatalogCategoryId = DresscaSeedIds.Category3, CatalogBrandId = DresscaSeedIds.Brand2, Description = "柔らかいソールは快適な履き心地で、ランニングに最適です。", Name = "ランニングシューズ - ブルー", Price = 12800m, ProductCode = "S000000001", Id = DresscaSeedIds.Item10 },
            new() { CatalogCategoryId = DresscaSeedIds.Category3, CatalogBrandId = DresscaSeedIds.Brand1, Description = "イタリアの職人が丁寧に手作業で作り上げた一品です。", Name = "メダリオン ストレートチップ ドレスシューズ", Price = 23800m, ProductCode = "S000000002", Id = DresscaSeedIds.Item11 },
        ]);
    }
}
