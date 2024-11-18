using Dressca.ApplicationCore.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dressca.EfInfrastructure.Configurations.Catalog;

/// <summary>
///  <see cref="CatalogCategory" /> エンティティの構成を提供します。
/// </summary>
internal class CatalogCategoryConfiguration : IEntityTypeConfiguration<CatalogCategory>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<CatalogCategory> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("CatalogCategories");
        builder.Property(catalogItem => catalogItem.Name)
            .HasMaxLength(128)
            .IsRequired();

        builder.HasData(
        [
            new() { Name = "服", Id = 1L },
            new() { Name = "バッグ", Id = 2L },
            new() { Name = "シューズ", Id = 3L },
        ]);
    }
}
