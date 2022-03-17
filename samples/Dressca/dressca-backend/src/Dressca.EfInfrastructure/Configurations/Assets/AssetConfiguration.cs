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

        builder.HasData(new Asset[]
        {
            new("b52dc7f712d94ca5812dd995bf926c04", AssetTypes.Png) { Id = 1L }, // TOP 用
            new("80bc8e167ccb4543b2f9d51913073492", AssetTypes.Png) { Id = 2L }, // TOP 用
            new("05d38fad5693422c8a27dd5b14070ec8", AssetTypes.Png) { Id = 3L }, // TOP 用
            new("45c22ba3da064391baac91341067ffe9", AssetTypes.Png) { Id = 4L },
            new("4aed07c4ed5d45a5b97f11acedfbb601", AssetTypes.Png) { Id = 5L },
            new("082b37439ecc44919626ba00fc60ee85", AssetTypes.Png) { Id = 6L },
            new("f5f89954281747fa878129c29e1e0f83", AssetTypes.Png) { Id = 7L },
            new("a8291ef2e8e14869a7048e272915f33c", AssetTypes.Png) { Id = 8L },
            new("66237018c769478a90037bd877f5fba1", AssetTypes.Png) { Id = 9L },
            new("d136d4c81b86478990984dcafbf08244", AssetTypes.Png) { Id = 10L },
            new("47183f32f6584d7fb661f9216e11318b", AssetTypes.Png) { Id = 11L },
            new("cf151206efd344e1b86854f4aa49fdef", AssetTypes.Png) { Id = 12L },
            new("ab2e78eb7fe3408aadbf1e17a9945a8c", AssetTypes.Png) { Id = 13L },
            new("0e557e96bc054f10bc91c27405a83e85", AssetTypes.Png) { Id = 14L },
        });
    }
}
