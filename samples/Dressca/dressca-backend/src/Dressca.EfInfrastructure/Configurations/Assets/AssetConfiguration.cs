using Dressca.ApplicationCore.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dressca.EfInfrastructure.Configurations.Assets;

/// <summary>
///  <see cref="Asset" /> エンティティの構成を提供します。
/// </summary>
internal class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("Assets");
        builder.Property(asset => asset.AssetCode)
            .HasMaxLength(32)
            .IsRequired();
        builder.Property(asset => asset.AssetType)
            .HasMaxLength(128)
            .IsRequired();

        builder.HasIndex(asset => asset.AssetCode);

        builder.HasData(
        [
            new() { AssetCode = "b52dc7f712d94ca5812dd995bf926c04", AssetType = AssetTypes.Png, Id = 1L }, // TOP 用
            new() { AssetCode = "80bc8e167ccb4543b2f9d51913073492", AssetType = AssetTypes.Png, Id = 2L }, // TOP 用
            new() { AssetCode = "05d38fad5693422c8a27dd5b14070ec8", AssetType = AssetTypes.Png, Id = 3L }, // TOP 用
            new() { AssetCode = "45c22ba3da064391baac91341067ffe9", AssetType = AssetTypes.Png, Id = 4L },
            new() { AssetCode = "4aed07c4ed5d45a5b97f11acedfbb601", AssetType = AssetTypes.Png, Id = 5L },
            new() { AssetCode = "082b37439ecc44919626ba00fc60ee85", AssetType = AssetTypes.Png, Id = 6L },
            new() { AssetCode = "f5f89954281747fa878129c29e1e0f83", AssetType = AssetTypes.Png, Id = 7L },
            new() { AssetCode = "a8291ef2e8e14869a7048e272915f33c", AssetType = AssetTypes.Png, Id = 8L },
            new() { AssetCode = "66237018c769478a90037bd877f5fba1", AssetType = AssetTypes.Png, Id = 9L },
            new() { AssetCode = "d136d4c81b86478990984dcafbf08244", AssetType = AssetTypes.Png, Id = 10L },
            new() { AssetCode = "47183f32f6584d7fb661f9216e11318b", AssetType = AssetTypes.Png, Id = 11L },
            new() { AssetCode = "cf151206efd344e1b86854f4aa49fdef", AssetType = AssetTypes.Png, Id = 12L },
            new() { AssetCode = "ab2e78eb7fe3408aadbf1e17a9945a8c", AssetType = AssetTypes.Png, Id = 13L },
            new() { AssetCode = "0e557e96bc054f10bc91c27405a83e85", AssetType = AssetTypes.Png, Id = 14L },
            new() { AssetCode = "e622b0098808492cb883831c05486b58", AssetType = AssetTypes.Png, Id = 15L }, // now printing
        ]);
    }
}
