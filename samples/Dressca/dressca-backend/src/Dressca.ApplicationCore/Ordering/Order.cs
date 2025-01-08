using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Accounting;
using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Ordering;

/// <summary>
///  注文を表現するエンティティです。
/// </summary>
public class Order
{
    private readonly List<OrderItem> orderItems = [];
    private readonly TimeProvider timeProvider;
    private readonly Account? account;
    private string buyerId;

    /// <summary>
    ///  <see cref="Order"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="orderItems">注文アイテムのリスト。</param>
    /// <exception cref="ArgumentException">null または空のリストを設定できません。</exception>
    public Order(List<OrderItem> orderItems)
        : this(orderItems, TimeProvider.System)
    {
    }

    /// <summary>
    ///  <see cref="Order"/> クラスの新しいインスタンスを初期化します。
    ///  単体テスト用に<see cref="TimeProvider"/> を受け取ることができます。
    /// </summary>
    /// <param name="orderItems">注文アイテムのリスト。</param>
    /// <param name="timeProvider">日時のプロバイダ。通常はシステム日時。</param>
    /// <exception cref="ArgumentException">null または空のリストを設定できません。</exception>
    /// <exception cref="ArgumentNullException"><paramref name="timeProvider"/> が <see langword="null"/> です。</exception>
    internal Order(List<OrderItem> orderItems, TimeProvider timeProvider)
    {
        if (orderItems is null || orderItems.Count == 0)
        {
            throw new ArgumentException(Messages.ArgumentIsNullOrEmptyList, nameof(orderItems));
        }

        this.timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        this.OrderDate = this.timeProvider.GetLocalNow();
        this.ConsumptionTaxRate = Account.ConsumptionTaxRate;
        this.orderItems = orderItems;
        this.account = new Account(this.orderItems.Select(item => new AccountItem(item.Quantity, item.UnitPrice)));
        this.TotalItemsPrice = this.account.GetItemsTotalPrice();
        this.DeliveryCharge = this.account.GetDeliveryCharge();
        this.ConsumptionTax = this.account.GetConsumptionTax();
        this.TotalPrice = this.account.GetTotalPrice();
    }

    private Order()
    {
        this.timeProvider = TimeProvider.System;
    }

    /// <summary>
    ///  注文 Id を取得します。
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    ///  購入者 Id を取得します。
    /// </summary>
    /// <exception cref="ArgumentException"><see langword="null"/> または空の文字列を設定できません。</exception>
    public required string BuyerId
    {
        get => this.buyerId;

        [MemberNotNull(nameof(buyerId))]
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(Messages.ArgumentIsNullOrWhiteSpace, nameof(value));
            }

            this.buyerId = value;
        }
    }

    /// <summary>
    ///  注文日を取得します。
    ///  このクラスのインスタンスが生成されたシステム日時が自動的に設定されます.
    /// </summary>
    public DateTimeOffset OrderDate { get; private set; }

    /// <summary>
    ///  お届け先を取得します。
    /// </summary>
    public required ShipTo ShipToAddress { get; init; }

    /// <summary>
    ///  消費税率を取得します。
    /// </summary>
    public decimal ConsumptionTaxRate { get; private set; }

    /// <summary>
    ///  注文アイテムの税抜き合計金額を取得します。
    /// </summary>
    public decimal TotalItemsPrice { get; private set; }

    /// <summary>
    ///  送料を取得します。
    /// </summary>
    public decimal DeliveryCharge { get; private set; }

    /// <summary>
    ///  消費税額を取得します。
    /// </summary>
    public decimal ConsumptionTax { get; private set; }

    /// <summary>
    ///  送料、税込みの合計金額を取得します。
    /// </summary>
    public decimal TotalPrice { get; private set; }

    /// <summary>
    ///  注文アイテムのリストを取得します。
    /// </summary>
    public IReadOnlyCollection<OrderItem> OrderItems => this.orderItems.AsReadOnly();

    /// <summary>
    ///  このインスタンスの購入者 Id と指定の購入者 Id が一致するか判定します。
    /// </summary>
    /// <param name="buyerId">購入者 Id 。</param>
    /// <returns>一致する場合 <see langword="true"/> 、一致しない場合 <see langword="false"/> を返します。</returns>
    public bool HasMatchingBuyerId(string buyerId)
    {
        return this.BuyerId == buyerId;
    }
}
