using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Ordering;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Consumer.Dto.Catalog;
using Dressca.Web.Consumer.Dto.Ordering;

namespace Dressca.Web.Consumer.Mapper;

/// <summary>
///  <see cref="Order"/> と <see cref="GetOrderByIdResponse"/> のマッパーです。
/// </summary>
public class OrderMapper : IObjectMapper<Order, GetOrderByIdResponse>
{
    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(value))]
    public GetOrderByIdResponse? Convert(Order? value)
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

        static OrderItemApiModel ConvertToOrderItemDto(OrderItem orderItem)
        {
            return new OrderItemApiModel
            {
                Id = orderItem.Id,
                Quantity = orderItem.Quantity,
                UnitPrice = orderItem.UnitPrice,
                SubTotal = orderItem.GetSubTotal(),
                ItemOrdered = new CatalogItemSummaryApiModel
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
