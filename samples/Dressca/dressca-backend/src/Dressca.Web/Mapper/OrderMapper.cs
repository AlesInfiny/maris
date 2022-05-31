using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Ordering;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Dto.Catalog;
using Dressca.Web.Dto.Ordering;

namespace Dressca.Web.Mapper;

/// <summary>
///  <see cref="Order"/> と <see cref="OrderResponse"/> のマッパーです。
/// </summary>
public class OrderMapper : IObjectMapper<Order, OrderResponse>
{
    /// <inheritdoc/>
    [return: NotNullIfNotNull("value")]
    public OrderResponse? Convert(Order? value)
    {
        if (value is null)
        {
            return null;
        }

        return new()
        {
            Id = value.Id,
            BuyerId = value.BuyerId,
            OrderDate = value.OrderDate,
            FullName = value.ShipToAddress.FullName,
            PostalCode = value.ShipToAddress.Address.PostalCode,
            Todofuken = value.ShipToAddress.Address.Todofuken,
            Shikuchoson = value.ShipToAddress.Address.Shikuchoson,
            AzanaAndOthers = value.ShipToAddress.Address.AzanaAndOthers,
            Account = new()
            {
                ConsumptionTaxRate = value.ConsumptionTaxRate,
                TotalItemsPrice = value.TotalItemsPrice,
                DeliveryCharge = value.DeliveryCharge,
                ConsumptionTax = value.ConsumptionTax,
                TotalPrice = value.TotalPrice,
            },
            OrderItems = value.OrderItems.Select(item => ConvertToOrderItemDto(item)).ToList(),
        };

        static OrderItemDto ConvertToOrderItemDto(OrderItem orderItem)
        {
            return new OrderItemDto
            {
                Id = orderItem.Id,
                Quantity = orderItem.Quantity,
                UnitPrice = orderItem.UnitPrice,
                SubTotal = orderItem.GetSubTotal(),
                ItemOrdered = new CatalogItemSummaryDto
                {
                    Id = orderItem.ItemOrdered.CatalogItemId,
                    Name = orderItem.ItemOrdered.ProductName,
                    ProductCode = orderItem.ItemOrdered.ProductCode,
                    AssetCodes = orderItem.Assets.Select(asset => asset.AssetCode).ToList(),
                },
            };
        }
    }
}
