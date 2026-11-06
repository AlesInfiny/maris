using Dressca.CatalogManagement.ApplicationCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dressca.CatalogManagement.Infrastructures.Configurations;

/// <summary>
///  <see cref="CatalogBrand" /> エンティティの構成を提供します。
/// </summary>
internal class CatalogBrandConfiguration : IEntityTypeConfiguration<CatalogBrand>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<CatalogBrand> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("CatalogBrands");
        builder.Property(catalogBrand => catalogBrand.Id)
            .ValueGeneratedNever();
        builder.Property(catalogItem => catalogItem.Name)
            .HasMaxLength(128)
            .IsRequired();

        builder.HasData(
        [
            new() { Name = "高級なブランド", Id = CatalogManagementSeedIds.Brand1 },
            new() { Name = "カジュアルなブランド", Id = CatalogManagementSeedIds.Brand2 },
            new() { Name = "ノーブランド", Id = CatalogManagementSeedIds.Brand3 },
        ]);
    }
}
