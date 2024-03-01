using Dressca.ApplicationCore.Baskets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dressca.EfInfrastructure.Configurations.Baskets;

/// <summary>
///  <see cref="Basket" /> エンティティの構成を提供します。
/// </summary>
internal class BasketConfiguration : IEntityTypeConfiguration<Basket>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Basket> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("Baskets");
        builder.Property(basket => basket.BuyerId)
            .HasMaxLength(64)
            .IsRequired();
    }
}
