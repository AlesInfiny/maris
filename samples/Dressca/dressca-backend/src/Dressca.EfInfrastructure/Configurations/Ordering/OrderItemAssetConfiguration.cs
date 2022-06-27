using Dressca.ApplicationCore.Ordering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dressca.EfInfrastructure.Configurations.Ordering;

/// <summary>
///  <see cref="OrderItemAsset" /> エンティティの構成を提供します。
/// </summary>
internal class OrderItemAssetConfiguration : IEntityTypeConfiguration<OrderItemAsset>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<OrderItemAsset> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("OrderItemAssets");
        builder.Property(orderItemAsset => orderItemAsset.AssetCode)
            .HasMaxLength(32)
            .IsRequired();
        builder.HasOne(orderItemAsset => orderItemAsset.OrderItem)
            .WithMany(orderItem => orderItem.Assets)
            .HasForeignKey(orderItemAsset => orderItemAsset.OrderItemId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_OrderItemAssets_OrderItems");
    }
}
