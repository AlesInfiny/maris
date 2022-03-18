using Dressca.ApplicationCore.Catalog;
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

        builder.HasData(new CatalogItem[]
        {
            new(1L, 3L, "定番の無地ロングTシャツです。", "クルーネック Tシャツ - ブラック", 1980m, "C000000001") { Id = 1L },
            new(1L, 2L, "暖かいのに着膨れしない起毛デニムです。", "裏起毛 スキニーデニム", 4800m, "C000000002") { Id = 2L },
            new(1L, 1L, "あたたかく肌ざわりも良いウール100%のロングコートです。", "ウールコート", 49800m, "C000000003") { Id = 3L },
            new(1L, 2L, "コットン100%の柔らかい着心地で、春先から夏、秋口まで万能に使いやすいです。", "無地 ボタンダウンシャツ", 2800m, "C000000004") { Id = 4L },
            new(2L, 3L, "コンパクトサイズのバッグですが収納力は抜群です", "レザーハンドバッグ", 18800m, "B000000001") { Id = 5L },
            new(2L, 2L, "エイジング加工したレザーを使用しています。", "ショルダーバッグ", 38000m, "B000000002") { Id = 6L },
            new(2L, 3L, "春の季節にぴったりのトートバッグです。インナーポーチまたは単体でも使用可能なポーチ付。", "トートバッグ ポーチ付き", 24800m, "B000000003") { Id = 7L },
            new(2L, 1L, "さらりと気軽に纏える、キュートなミニサイズショルダー。", "ショルダーバッグ", 2800m, "B000000004") { Id = 8L },
            new(2L, 1L, "エレガントな雰囲気を放つキルティングデザインです。", "レザー チェーンショルダーバッグ", 258000m, "B000000005") { Id = 9L },
            new(3L, 2L, "柔らかいソールは快適な履き心地で、ランニングに最適です。", "ランニングシューズ - ブルー", 12800m, "S000000001") { Id = 10L },
            new(3L, 1L, "イタリアの職人が丁寧に手作業で作り上げた一品です。", "メダリオン ストレートチップ ドレスシューズ", 23800m, "S000000002") { Id = 11L },
        });
    }
}
