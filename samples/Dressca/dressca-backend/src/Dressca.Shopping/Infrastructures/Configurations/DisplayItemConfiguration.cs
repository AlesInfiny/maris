using Dressca.Shopping.Infrastructures.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dressca.Shopping.Infrastructures.Configurations;

/// <summary>陳列品の永続化と初期データを構成します。</summary>
internal class DisplayItemConfiguration : IEntityTypeConfiguration<DisplayItemEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<DisplayItemEntity> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("DisplayItems");
        builder.Property(item => item.Id).ValueGeneratedNever();
        builder.HasOne(item => item.CatalogItem)
            .WithMany()
            .HasForeignKey(item => item.CatalogItemId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_DisplayItems_CatalogItems");
        builder.HasData(
        [
            new() { Id = ShoppingSeedIds.DisplayItem1, CatalogItemId = ShoppingSeedIds.Item1 },
            new() { Id = ShoppingSeedIds.DisplayItem2, CatalogItemId = ShoppingSeedIds.Item2 },
            new() { Id = ShoppingSeedIds.DisplayItem3, CatalogItemId = ShoppingSeedIds.Item3 },
            new() { Id = ShoppingSeedIds.DisplayItem4, CatalogItemId = ShoppingSeedIds.Item4 },
            new() { Id = ShoppingSeedIds.DisplayItem5, CatalogItemId = ShoppingSeedIds.Item5 },
            new() { Id = ShoppingSeedIds.DisplayItem6, CatalogItemId = ShoppingSeedIds.Item6 },
            new() { Id = ShoppingSeedIds.DisplayItem7, CatalogItemId = ShoppingSeedIds.Item7 },
            new() { Id = ShoppingSeedIds.DisplayItem8, CatalogItemId = ShoppingSeedIds.Item8 },
            new() { Id = ShoppingSeedIds.DisplayItem9, CatalogItemId = ShoppingSeedIds.Item9 },
            new() { Id = ShoppingSeedIds.DisplayItem10, CatalogItemId = ShoppingSeedIds.Item10 },
            new() { Id = ShoppingSeedIds.DisplayItem11, CatalogItemId = ShoppingSeedIds.Item11 },
        ]);
    }
}
