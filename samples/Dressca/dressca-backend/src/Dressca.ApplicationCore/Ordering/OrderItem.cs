using Dressca.ApplicationCore.Accounting;
using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Ordering;

/// <summary>
///  注文アイテムエンティティ。
///  注文内の各アイテム毎の詳細情報（単価や数量など）を保持します。
/// </summary>
public class OrderItem
{
    private readonly List<OrderItemAsset> assets = new();
    private Order? order;

    /// <summary>
    ///  <see cref="OrderItem"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public OrderItem()
    {
    }

    /// <summary>
    ///  注文アイテム Id を取得します。
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    ///  注文された商品（カタログアイテム）を取得します。
    /// </summary>
    public required CatalogItemOrdered ItemOrdered { get; init; }

    /// <summary>
    ///  単価を取得します。
    ///  <see cref="ItemOrdered"/> プロパティも商品情報ですが、計算や情報管理の都合で単価はその中には含めていません。
    /// </summary>
    public required decimal UnitPrice { get; init; }

    /// <summary>
    ///  数量を取得します。
    ///  カタログアイテムごとに取り扱い単位が異なる可能性があります。
    ///  例えば、1 ダース単位に販売する商品の場合、この数量の単位は"ダース"です。
    /// </summary>
    public required int Quantity { get; init; }

    /// <summary>
    ///  注文 Id を取得します。
    /// </summary>
    public long OrderId { get; private set; }

    /// <summary>
    ///  注文アイテムのアセットリストを取得します。
    /// </summary>
    public IReadOnlyCollection<OrderItemAsset> Assets => this.assets.AsReadOnly();

    /// <summary>
    ///  注文（ナビゲーションプロパティ）を取得します。
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="Order"/> が設定されていません。</exception>
    /// <exception cref="ArgumentNullException"><see langword="null"/> を設定できません。</exception>
    public Order Order
    {
        get => this.order ?? throw new InvalidOperationException(string.Format(Messages.PropertyNotInitialized, nameof(this.Order)));
        private set => this.order = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    ///  注文アイテムのアセットリストを追加します。
    /// </summary>
    /// <param name="orderItemAssets">注文アイテムのアセットリスト。</param>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="orderItemAssets"/> が <see langword="null"/> です。
    /// </exception>
    public void AddAssets(IEnumerable<OrderItemAsset> orderItemAssets)
    {
        ArgumentNullException.ThrowIfNull(orderItemAssets);
        this.assets.AddRange(orderItemAssets);
    }

    /// <summary>
    ///  注文アイテムの小計を計算して金額を返却します。
    /// </summary>
    /// <returns>注文アイテムの小計額。</returns>
    public decimal GetSubTotal()
        => new AccountItem(this.Quantity, this.UnitPrice).GetSubTotal();
}
