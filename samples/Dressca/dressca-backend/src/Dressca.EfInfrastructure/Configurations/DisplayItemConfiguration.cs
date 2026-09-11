using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dressca.EfInfrastructure.Configurations;

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
            new() { Id = DresscaSeedIds.DisplayItem1, CatalogItemId = DresscaSeedIds.Item1 },
            new() { Id = DresscaSeedIds.DisplayItem2, CatalogItemId = DresscaSeedIds.Item2 },
            new() { Id = DresscaSeedIds.DisplayItem3, CatalogItemId = DresscaSeedIds.Item3 },
            new() { Id = DresscaSeedIds.DisplayItem4, CatalogItemId = DresscaSeedIds.Item4 },
            new() { Id = DresscaSeedIds.DisplayItem5, CatalogItemId = DresscaSeedIds.Item5 },
            new() { Id = DresscaSeedIds.DisplayItem6, CatalogItemId = DresscaSeedIds.Item6 },
            new() { Id = DresscaSeedIds.DisplayItem7, CatalogItemId = DresscaSeedIds.Item7 },
            new() { Id = DresscaSeedIds.DisplayItem8, CatalogItemId = DresscaSeedIds.Item8 },
            new() { Id = DresscaSeedIds.DisplayItem9, CatalogItemId = DresscaSeedIds.Item9 },
            new() { Id = DresscaSeedIds.DisplayItem10, CatalogItemId = DresscaSeedIds.Item10 },
            new() { Id = DresscaSeedIds.DisplayItem11, CatalogItemId = DresscaSeedIds.Item11 },
        ]);
    }
}
