using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Accounting;
using Dressca.ApplicationCore.Baskets;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Consumer.Dto.Baskets;

namespace Dressca.Web.Consumer.Mapper;

/// <summary>
///  <see cref="Basket"/> と <see cref="BasketResponse"/> のマッパーです。
/// </summary>
public class BasketMapper : IObjectMapper<Basket, BasketResponse>
{
    private readonly IObjectMapper<BasketItem, BasketItemResponse> basketItemMapper;

    /// <summary>
    ///  <see cref="BasketMapper"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="basketItemMapper">買い物かごアイテムのマッパー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="basketItemMapper"/> が <see langword="null"/> です。
    /// </exception>
    public BasketMapper(IObjectMapper<BasketItem, BasketItemResponse> basketItemMapper)
        => this.basketItemMapper = basketItemMapper ?? throw new ArgumentNullException(nameof(basketItemMapper));

    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(value))]
    public BasketResponse? Convert(Basket? value)
    {
        if (value is null)
        {
            return null;
        }

        var account = value.GetAccount();
        return new()
        {
            BuyerId = value.BuyerId,
            Account = new()
            {
                ConsumptionTax = account.GetConsumptionTax(),
                ConsumptionTaxRate = Account.ConsumptionTaxRate,
                DeliveryCharge = account.GetDeliveryCharge(),
                TotalItemsPrice = account.GetItemsTotalPrice(),
                TotalPrice = account.GetTotalPrice(),
            },
            BasketItems = value.Items.Select(item => this.basketItemMapper.Convert(item)).ToList(),
        };
    }
}
