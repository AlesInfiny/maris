using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Ordering;

/// <summary>
///  注文アイテムエンティティ。
///  注文内の各アイテム毎の詳細情報（単価や数量など）を保持します。
/// </summary>
public class OrderItem
{
    private CatalogItemOrdered? itemOrdered;
    private Order? order;

    /// <summary>
    ///  <see cref="OrderItem"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="itemOrdered">注文されたカタログアイテム。</param>
    /// <param name="unitPrice">単価。</param>
    /// <param name="quantity">数量。</param>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="itemOrdered"/> が <see langword="null"/> です。
    /// </exception>
    public OrderItem(CatalogItemOrdered itemOrdered, decimal unitPrice, int quantity)
    {
        this.ItemOrdered = itemOrdered;
        this.UnitPrice = unitPrice;
        this.Quantity = quantity;
    }

    private OrderItem()
    {
        // Required by EF Core.
    }

    /// <summary>
    ///  注文アイテム Id を取得します。
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    ///  注文された商品（カタログアイテム）を取得します。
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="ItemOrdered"/> が設定されていません。</exception>
    /// <exception cref="ArgumentNullException"><see langword="null"/> を設定できません。</exception>
    public CatalogItemOrdered ItemOrdered
    {
        get => this.itemOrdered ?? throw new InvalidOperationException(string.Format(ApplicationCoreMessages.PropertyNotInitialized, nameof(this.ItemOrdered)));

        [MemberNotNull(nameof(itemOrdered))]
        private set => this.itemOrdered = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    ///  単価を取得します。
    ///  <see cref="ItemOrdered"/> プロパティも商品情報ですが、計算や情報管理の都合で単価はその中には含めていません。
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    ///  数量を取得します。
    ///  カタログアイテムごとに取り扱い単位が異なる可能性があります。
    ///  例えば、1 ダース単位に販売する商品の場合、この数量の単位は"ダース"です。
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    ///  注文 Id を取得します。
    /// </summary>
    public long OrderId { get; private set; }

    /// <summary>
    ///  注文（ナビゲーションプロパティ）を取得します。
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="Order"/> が設定されていません。</exception>
    /// <exception cref="ArgumentNullException"><see langword="null"/> を設定できません。</exception>
    public Order Order
    {
        get => this.order ?? throw new InvalidOperationException(string.Format(ApplicationCoreMessages.PropertyNotInitialized, nameof(this.Order)));
        private set => this.order = value ?? throw new ArgumentNullException(nameof(value));
    }
}
