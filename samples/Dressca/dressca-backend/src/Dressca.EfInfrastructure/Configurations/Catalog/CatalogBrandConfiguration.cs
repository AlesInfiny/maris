using Dressca.ApplicationCore.Catalog;
using Dressca.EfInfrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dressca.EfInfrastructure.Configurations.Catalog;

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
            new() { Name = "高級なブランド", Id = DresscaSeedIds.Brand1 },
            new() { Name = "カジュアルなブランド", Id = DresscaSeedIds.Brand2 },
            new() { Name = "ノーブランド", Id = DresscaSeedIds.Brand3 },
        ]);
    }
}
