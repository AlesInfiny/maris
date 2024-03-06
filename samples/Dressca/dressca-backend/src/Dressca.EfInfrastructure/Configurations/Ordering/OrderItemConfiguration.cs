using Dressca.ApplicationCore.Ordering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dressca.EfInfrastructure.Configurations.Ordering;

/// <summary>
///  <see cref="OrderItem" /> エンティティの構成を提供します。
/// </summary>
internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("OrderItems");
        builder.Property(orderItem => orderItem.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal(18,6)");
        builder.Property(orderItem => orderItem.Quantity)
            .IsRequired();
        builder.ComplexProperty(
            orderItem => orderItem.ItemOrdered,
            catalogItemOrderedBuider =>
            {
                catalogItemOrderedBuider.Property(orderedItem => orderedItem.CatalogItemId)
                    .IsRequired()
                    .HasColumnName("OrderedCatalogItemId");
                catalogItemOrderedBuider.Property(orderedItem => orderedItem.ProductName)
                    .HasMaxLength(512)
                    .IsRequired()
                    .HasColumnName("OrderedProductName");
                catalogItemOrderedBuider.Property(orderedItem => orderedItem.ProductCode)
                    .HasMaxLength(128)
                    .IsRequired()
                    .HasColumnName("OrderedProductCode");
            });
        builder.HasOne(orderItem => orderItem.Order)
            .WithMany(order => order.OrderItems)
            .HasForeignKey(orderItem => orderItem.OrderId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_OrderItems_Orders");
    }
}
