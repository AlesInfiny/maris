using Dressca.Shopping.Infrastructures.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dressca.Shopping.Infrastructures.Configurations;

/// <summary>
///  <see cref="CatalogItemAssetEntity" /> エンティティの構成を提供します。
///  テーブルはカタログ管理コンテキストが所有するため、マイグレーションの対象外とします。
/// </summary>
internal class CatalogItemAssetEntityConfiguration : IEntityTypeConfiguration<CatalogItemAssetEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<CatalogItemAssetEntity> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("CatalogItemAssets", table => table.ExcludeFromMigrations());
    }
}
