using Dressca.CatalogManagement.ApplicationCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dressca.CatalogManagement.Infrastructures.Configurations;

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
        builder.Property(catalogCategory => catalogCategory.Id)
            .ValueGeneratedNever();
        builder.Property(catalogItem => catalogItem.Name)
            .HasMaxLength(128)
            .IsRequired();

        builder.HasData(
        [
            new() { Name = "服", Id = CatalogManagementSeedIds.Category1 },
            new() { Name = "バッグ", Id = CatalogManagementSeedIds.Category2 },
            new() { Name = "シューズ", Id = CatalogManagementSeedIds.Category3 },
        ]);
    }
}
