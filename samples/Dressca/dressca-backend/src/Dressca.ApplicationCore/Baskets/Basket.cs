using System.Diagnostics.CodeAnalysis;

namespace Dressca.ApplicationCore.Baskets;

/// <summary>
///  買い物かごエンティティ。
/// </summary>
public class Basket
{
    private readonly List<BasketItem> items = new();
    private string buyerId;

    /// <summary>
    ///  <see cref="Basket"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="buyerId">購入者 Id 。</param>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="buyerId"/> が <see langword="null"/> です.
    /// </exception>
    public Basket(string buyerId) => this.BuyerId = buyerId;

    /// <summary>
    ///  買い物かご Id を取得します。
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    ///  購入者 Id を取得します。
    /// </summary>
    public string BuyerId
    {
        get => this.buyerId;

        [MemberNotNull(nameof(buyerId))]
        private set
        {
            this.buyerId = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    ///  買い物かごアイテムを取得します。
    /// </summary>
    public IReadOnlyCollection<BasketItem> Items => this.items.AsReadOnly();

    /// <summary>
    ///  買い物かごにアイテムを追加します。
    ///  同一アイテムが既に買い物かご内に存在する場合は、<paramref name="quantity"/> 分だけ数量が加算されます。
    ///  またこのとき、<paramref name="quantity"/> が負の値の場合は、数量を減算します。
    /// </summary>
    /// <param name="catalogItemId">カタログアイテム Id 。</param>
    /// <param name="unitPrice">単価。</param>
    /// <param name="quantity">数量。</param>
    /// <exception cref="InvalidOperationException">アイテムの数量が負値になる場合。</exception>
    public void AddItem(long catalogItemId, decimal unitPrice, int quantity = 1)
    {
        if (!this.items.Any(i => i.CatalogItemId == catalogItemId))
        {
            this.items.Add(new BasketItem(catalogItemId, unitPrice, quantity));
            return;
        }

        var existingItem = this.items.First(i => i.CatalogItemId == catalogItemId);
        existingItem.AddQuantity(quantity);
    }

    /// <summary>
    ///  数量が 0 のアイテムを買い物かごから除去します。
    /// </summary>
    public void RemoveEmptyItems() => _ = this.items.RemoveAll(i => i.Quantity == 0);

    /// <summary>
    ///  この買い物かごの購入者 Id を変更します。
    /// </summary>
    /// <param name="buyerId">購入者 Id 。</param>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="buyerId"/> が <see langword="null"/> です.
    /// </exception>
    public void SetNewBuyerId(string buyerId) => this.BuyerId = buyerId;
}
