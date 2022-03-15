using Dressca.ApplicationCore.Baskets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dressca.EfInfrastructure.Configurations.Baskets;

/// <summary>
///  <see cref="BasketItem" /> エンティティの構成を提供します。
/// </summary>
internal class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<BasketItem> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("BasketItems");
        builder.Property(basketItem => basketItem.CatalogItemId)
            .IsRequired();
        builder.Property(basketItem => basketItem.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal(18,6)");
        builder.Property(basketItem => basketItem.Quantity)
            .IsRequired();
        builder.HasOne(basketItem => basketItem.Basket)
            .WithMany(basket => basket.Items)
            .HasForeignKey(basketItem => basketItem.BasketId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_BasketItems_Baskets");
    }
}
