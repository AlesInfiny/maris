using Dressca.ApplicationCore.Ordering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dressca.EfInfrastructure.Configurations.Ordering;

/// <summary>
///  <see cref="Order" /> エンティティの構成を提供します。
/// </summary>
internal class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("Orders");
        builder.Property(order => order.BuyerId)
            .HasMaxLength(64)
            .IsRequired();
        builder.Property(order => order.OrderDate)
            .IsRequired();
        builder.ComplexProperty(order => order.ShipToAddress)
            .Property(shipTo => shipTo.FullName)
                .HasMaxLength(64)
                .IsRequired()
                .HasColumnName("ShipToFullName");
        builder.ComplexProperty(order => order.ShipToAddress)
            .ComplexProperty(shipTo => shipTo.Address)
                .Property(address => address.PostalCode)
                    .HasMaxLength(16)
                    .IsRequired()
                    .HasColumnName("ShipToPostalCode");
        builder.ComplexProperty(order => order.ShipToAddress)
            .ComplexProperty(shipTo => shipTo.Address)
                .Property(address => address.Todofuken)
                    .HasMaxLength(16)
                    .IsRequired()
                    .HasColumnName("ShipToTodofuken");
        builder.ComplexProperty(order => order.ShipToAddress)
            .ComplexProperty(shipTo => shipTo.Address)
                .Property(address => address.Shikuchoson)
                    .HasMaxLength(32)
                    .IsRequired()
                    .HasColumnName("ShipToShikuchoson");
        builder.ComplexProperty(order => order.ShipToAddress)
            .ComplexProperty(shipTo => shipTo.Address)
                .Property(address => address.AzanaAndOthers)
                    .HasMaxLength(128)
                    .IsRequired()
                    .HasColumnName("ShipToAzanaAndOthers");
        builder.Property(order => order.TotalItemsPrice)
            .IsRequired()
            .HasColumnType("decimal(18,6)");
        builder.Property(order => order.DeliveryCharge)
            .IsRequired()
            .HasColumnType("decimal(18,6)");
        builder.Property(order => order.ConsumptionTax)
            .IsRequired()
            .HasColumnType("decimal(18,6)");
        builder.Property(order => order.ConsumptionTaxRate)
            .IsRequired()
            .HasColumnType("decimal(18,6)");
        builder.Property(order => order.TotalPrice)
            .IsRequired()
            .HasColumnType("decimal(18,6)");
    }
}
