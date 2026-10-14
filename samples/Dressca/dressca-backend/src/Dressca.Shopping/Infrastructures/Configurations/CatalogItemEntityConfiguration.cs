using Dressca.Shopping.Infrastructures.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dressca.Shopping.Infrastructures.Configurations;

/// <summary>
///  <see cref="CatalogItemEntity" /> エンティティの構成を提供します。
///  テーブルはカタログ管理コンテキストが所有するため、マイグレーションの対象外とします。
/// </summary>
internal class CatalogItemEntityConfiguration : IEntityTypeConfiguration<CatalogItemEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<CatalogItemEntity> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("CatalogItems", table => table.ExcludeFromMigrations());
        builder.Property(catalogItem => catalogItem.Price)
            .HasColumnType("decimal(18,6)");
        builder.HasMany(catalogItem => catalogItem.Assets)
            .WithOne()
            .HasForeignKey(catalogItemAsset => catalogItemAsset.CatalogItemId);
    }
}
