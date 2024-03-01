using Dressca.ApplicationCore.Accounting;
using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Ordering;

/// <summary>
///  注文エンティティ。
/// </summary>
public class Order
{
    private readonly List<OrderItem> orderItems = new();
    private readonly Account? account;
    private string? buyerId;
    private ShipTo? shipToAddress;

    /// <summary>
    ///  <see cref="Order"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="buyerId">購入者 Id 。</param>
    /// <param name="shipToAddress">配送先住所。</param>
    /// <param name="orderItems">注文アイテムのリスト。</param>
    /// <exception cref="ArgumentException">
    ///  <list type="bullet">
    ///   <item><paramref name="buyerId"/> が <see langword="null"/> または空の文字列です。</item>
    ///   <item><paramref name="orderItems"/> が <see langword="null"/> または空のリストです。</item>
    ///  </list>
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="shipToAddress"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public Order(string buyerId, ShipTo shipToAddress, List<OrderItem> orderItems)
    {
        if (orderItems is null || !orderItems.Any())
        {
            throw new ArgumentException(Messages.ArgumentIsNullOrEmptyList, nameof(orderItems));
        }

        this.BuyerId = buyerId;
        this.ShipToAddress = shipToAddress;
        this.orderItems = orderItems;
        this.account = new Account(orderItems.Select(item => new AccountItem(item.Quantity, item.UnitPrice)));
        this.ConsumptionTaxRate = Account.ConsumptionTaxRate;
        this.TotalItemsPrice = this.account.GetItemsTotalPrice();
        this.DeliveryCharge = this.account.GetDeliveryCharge();
        this.ConsumptionTax = this.account.GetConsumptionTax();
        this.TotalPrice = this.account.GetTotalPrice();
    }

    private Order()
    {
        // Required by EF Core.
    }

    /// <summary>
    ///  注文 Id を取得します。
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    ///  購入者 Id を取得します。
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="BuyerId"/> が設定されていません。</exception>
    /// <exception cref="ArgumentException"><see langword="null"/> または空の文字列を設定できません。</exception>
    public string BuyerId
    {
        get => this.buyerId ?? throw new InvalidOperationException(string.Format(Messages.PropertyNotInitialized, nameof(this.BuyerId)));
        private set
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
    public DateTimeOffset OrderDate { get; private set; } = DateTimeOffset.Now;

    /// <summary>
    ///  お届け先を取得します。
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="ShipToAddress"/> が設定されていません。</exception>
    /// <exception cref="ArgumentNullException"><see langword="null"/> を設定できません。</exception>
    public ShipTo ShipToAddress
    {
        get => this.shipToAddress ?? throw new InvalidOperationException(string.Format(Messages.PropertyNotInitialized, nameof(this.shipToAddress)));
        private set => this.shipToAddress = value ?? throw new ArgumentNullException(nameof(value));
    }

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
