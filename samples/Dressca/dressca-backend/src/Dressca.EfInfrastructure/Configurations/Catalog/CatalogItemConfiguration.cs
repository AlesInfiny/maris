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
            new(1L, 3L, "定番の無地ロングTシャツです。", "クルーネック Tシャツ - グレー", 1980, "C000000001") { Id = 1L },
            new(1L, 2L, "裏起毛で温かいパーカーです。", "無地 パーカー", 5800, "C000000002") { Id = 2L },
            new(1L, 1L, "ウール生地のテーラードジャケットです。セットアップだけでなく単体でも使いやすい商品となっています。", "テーラードジャケット", 49800, "C000000003") { Id = 3L },
            new(1L, 2L, "ファー襟付きのデニムジャケットです。", "デニムジャケット", 19800, "C000000004") { Id = 4L },
            new(2L, 3L, "シンプルなデザインのトートバッグです。", "トートバッグ", 18800, "B000000001") { Id = 5L },
            new(2L, 2L, "軽くしなやかなレザーを使用しています。", "ショルダーバッグ", 38000, "B000000002") { Id = 6L },
            new(2L, 3L, "外側は高級感のある牛革、内側に丈夫で傷つきにくい豚革を採用した細身で持ち運びやすいビジネスバッグです。", "ビジネスバッグ", 24800, "B000000003") { Id = 7L },
            new(2L, 1L, "丁寧に編み込まれた馬革ハンドバッグです。落ち着いた色調で、オールシーズン使いやすくなっています。", "編み込みトートバッグ", 58800, "B000000004") { Id = 8L },
            new(2L, 1L, "卓越した素材と緻密な縫製作業によって、デザインが完璧に表現されています。", "ハンドバッグ", 258000, "B000000005") { Id = 9L },
            new(3L, 2L, "定番のハイカットスニーカーです。", "ハイカットスニーカー - ブラック", 12800, "S000000001") { Id = 10L },
            new(3L, 1L, "イタリアの職人が丁寧に手作業で作り上げた一品です。", "メダリオン ストレートチップ ドレスシューズ", 23800, "S000000002") { Id = 11L },
        });
    }
}
