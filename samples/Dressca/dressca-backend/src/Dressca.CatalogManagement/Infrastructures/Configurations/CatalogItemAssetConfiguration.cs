using Dressca.CatalogManagement.ApplicationCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dressca.CatalogManagement.Infrastructures.Configurations;

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
        builder.Property(catalogItemAsset => catalogItemAsset.Id)
            .ValueGeneratedNever();
        builder.Property(catalogItemAsset => catalogItemAsset.AssetCode)
            .HasMaxLength(32)
            .IsRequired();
        builder.HasOne(catalogItemAsset => catalogItemAsset.CatalogItem)
            .WithMany(catalogItem => catalogItem.Assets)
            .HasForeignKey(catalogItemAsset => catalogItemAsset.CatalogItemId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_CatalogItemAssets_CatalogItems");

        builder.HasData(
        [
            new() { AssetCode = "45c22ba3da064391baac91341067ffe9", CatalogItemId = CatalogManagementSeedIds.Item1, Id = CatalogManagementSeedIds.ItemAsset1 },
            new() { AssetCode = "4aed07c4ed5d45a5b97f11acedfbb601", CatalogItemId = CatalogManagementSeedIds.Item2, Id = CatalogManagementSeedIds.ItemAsset2 },
            new() { AssetCode = "082b37439ecc44919626ba00fc60ee85", CatalogItemId = CatalogManagementSeedIds.Item3, Id = CatalogManagementSeedIds.ItemAsset3 },
            new() { AssetCode = "f5f89954281747fa878129c29e1e0f83", CatalogItemId = CatalogManagementSeedIds.Item4, Id = CatalogManagementSeedIds.ItemAsset4 },
            new() { AssetCode = "a8291ef2e8e14869a7048e272915f33c", CatalogItemId = CatalogManagementSeedIds.Item5, Id = CatalogManagementSeedIds.ItemAsset5 },
            new() { AssetCode = "66237018c769478a90037bd877f5fba1", CatalogItemId = CatalogManagementSeedIds.Item6, Id = CatalogManagementSeedIds.ItemAsset6 },
            new() { AssetCode = "d136d4c81b86478990984dcafbf08244", CatalogItemId = CatalogManagementSeedIds.Item7, Id = CatalogManagementSeedIds.ItemAsset7 },
            new() { AssetCode = "47183f32f6584d7fb661f9216e11318b", CatalogItemId = CatalogManagementSeedIds.Item8, Id = CatalogManagementSeedIds.ItemAsset8 },
            new() { AssetCode = "cf151206efd344e1b86854f4aa49fdef", CatalogItemId = CatalogManagementSeedIds.Item9, Id = CatalogManagementSeedIds.ItemAsset9 },
            new() { AssetCode = "ab2e78eb7fe3408aadbf1e17a9945a8c", CatalogItemId = CatalogManagementSeedIds.Item10, Id = CatalogManagementSeedIds.ItemAsset10 },
            new() { AssetCode = "0e557e96bc054f10bc91c27405a83e85", CatalogItemId = CatalogManagementSeedIds.Item11, Id = CatalogManagementSeedIds.ItemAsset11 },
        ]);
    }
}
