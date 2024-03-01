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
            new("45c22ba3da064391baac91341067ffe9", 1L) { Id = 1L },
            new("4aed07c4ed5d45a5b97f11acedfbb601", 2L) { Id = 2L },
            new("082b37439ecc44919626ba00fc60ee85", 3L) { Id = 3L },
            new("f5f89954281747fa878129c29e1e0f83", 4L) { Id = 4L },
            new("a8291ef2e8e14869a7048e272915f33c", 5L) { Id = 5L },
            new("66237018c769478a90037bd877f5fba1", 6L) { Id = 6L },
            new("d136d4c81b86478990984dcafbf08244", 7L) { Id = 7L },
            new("47183f32f6584d7fb661f9216e11318b", 8L) { Id = 8L },
            new("cf151206efd344e1b86854f4aa49fdef", 9L) { Id = 9L },
            new("ab2e78eb7fe3408aadbf1e17a9945a8c", 10L) { Id = 10L },
            new("0e557e96bc054f10bc91c27405a83e85", 11L) { Id = 11L },
        });
    }
}
