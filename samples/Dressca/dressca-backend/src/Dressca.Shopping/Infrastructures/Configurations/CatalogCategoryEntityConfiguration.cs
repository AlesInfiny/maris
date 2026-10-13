using Dressca.Shopping.Infrastructures.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dressca.Shopping.Infrastructures.Configurations;

/// <summary>
///  <see cref="CatalogCategoryEntity" /> エンティティの構成を提供します。
///  テーブルはカタログ管理コンテキストが所有するため、マイグレーションの対象外とします。
/// </summary>
internal class CatalogCategoryEntityConfiguration : IEntityTypeConfiguration<CatalogCategoryEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<CatalogCategoryEntity> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("CatalogCategories", table => table.ExcludeFromMigrations());
    }
}
