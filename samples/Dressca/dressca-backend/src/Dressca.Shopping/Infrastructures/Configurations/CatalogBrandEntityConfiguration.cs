using Dressca.Shopping.Infrastructures.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dressca.Shopping.Infrastructures.Configurations;

/// <summary>
///  <see cref="CatalogBrandEntity" /> エンティティの構成を提供します。
///  テーブルはカタログ管理コンテキストが所有するため、マイグレーションの対象外とします。
/// </summary>
internal class CatalogBrandEntityConfiguration : IEntityTypeConfiguration<CatalogBrandEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<CatalogBrandEntity> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("CatalogBrands", table => table.ExcludeFromMigrations());
    }
}
