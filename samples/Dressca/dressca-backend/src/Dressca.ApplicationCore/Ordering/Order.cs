using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Ordering;

/// <summary>
///  注文エンティティ。
/// </summary>
public class Order
{
    private readonly List<OrderItem> orderItems = new();
    private string? buyerId;
    private ShipTo? shipToAddress;

    /// <summary>
    ///  <see cref="Order"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="buyerId">購入者 Id 。</param>
    /// <param name="shipToAddress">配送先住所。</param>
    /// <param name="orderItems">注文アイテムのリスト。</param>
    /// <exception cref="ArgumentException">
    ///  <paramref name="buyerId"/> が <see langword="null"/> または空の文字列です。
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="shipToAddress"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="orderItems"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public Order(string buyerId, ShipTo shipToAddress, List<OrderItem> orderItems)
    {
        this.BuyerId = buyerId;
        this.ShipToAddress = shipToAddress;
        this.orderItems = orderItems ?? throw new ArgumentNullException(nameof(orderItems));
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
        get => this.buyerId ?? throw new InvalidOperationException(string.Format(ApplicationCoreMessages.PropertyNotInitialized, nameof(this.BuyerId)));
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(ApplicationCoreMessages.ArgumentIsNullOrWhiteSpace, nameof(value));
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
        get => this.shipToAddress ?? throw new InvalidOperationException(string.Format(ApplicationCoreMessages.PropertyNotInitialized, nameof(this.shipToAddress)));
        private set => this.shipToAddress = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    ///  注文アイテムのリストを取得します。
    /// </summary>
    public IReadOnlyCollection<OrderItem> OrderItems => this.orderItems.AsReadOnly();

    /// <summary>
    ///  合計注文金額を計算します。
    ///  これは、注文アイテムの単価×数量の総計であり、配送料やポイント適用による割引を考慮しません。
    /// </summary>
    /// <returns>合計注文金額。</returns>
    public decimal Total()
    {
        var total = 0m;
        foreach (var item in this.orderItems)
        {
            total += item.UnitPrice * item.Quantity;
        }

        return total;
    }
}
