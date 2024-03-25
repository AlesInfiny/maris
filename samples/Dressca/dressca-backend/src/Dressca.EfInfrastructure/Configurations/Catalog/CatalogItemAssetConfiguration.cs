using Dressca.ApplicationCore.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dressca.EfInfrastructure.Configurations.Catalog;

/// <summary>
///  <see cref="CatalogItemAsset" /> エンティティの構成を提供します。
/// </summary>
internal class CatalogItemAssetConfiguration : IEntityTypeConfiguration<CatalogItemAsset>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<CatalogItemAsset> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("CatalogItemAssets");
        builder.Property(catalogItemAsset => catalogItemAsset.AssetCode)
            .HasMaxLength(32)
            .IsRequired();
        builder.HasOne(catalogItemAsset => catalogItemAsset.CatalogItem)
            .WithMany(catalogItem => catalogItem.Assets)
            .HasForeignKey(catalogItemAsset => catalogItemAsset.CatalogItemId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_CatalogItemAssets_CatalogItems");

        builder.HasData(new CatalogItemAsset[]
        {
            new() { AssetCode = "45c22ba3da064391baac91341067ffe9", CatalogItemId = 1L, Id = 1L },
            new() { AssetCode = "4aed07c4ed5d45a5b97f11acedfbb601", CatalogItemId = 2L, Id = 2L },
            new() { AssetCode = "082b37439ecc44919626ba00fc60ee85", CatalogItemId = 3L, Id = 3L },
            new() { AssetCode = "f5f89954281747fa878129c29e1e0f83", CatalogItemId = 4L, Id = 4L },
            new() { AssetCode = "a8291ef2e8e14869a7048e272915f33c", CatalogItemId = 5L, Id = 5L },
            new() { AssetCode = "66237018c769478a90037bd877f5fba1", CatalogItemId = 6L, Id = 6L },
            new() { AssetCode = "d136d4c81b86478990984dcafbf08244", CatalogItemId = 7L, Id = 7L },
            new() { AssetCode = "47183f32f6584d7fb661f9216e11318b", CatalogItemId = 8L, Id = 8L },
            new() { AssetCode = "cf151206efd344e1b86854f4aa49fdef", CatalogItemId = 9L, Id = 9L },
            new() { AssetCode = "ab2e78eb7fe3408aadbf1e17a9945a8c", CatalogItemId = 10L, Id = 10L },
            new() { AssetCode = "0e557e96bc054f10bc91c27405a83e85", CatalogItemId = 11L, Id = 11L },
        });
    }
}
