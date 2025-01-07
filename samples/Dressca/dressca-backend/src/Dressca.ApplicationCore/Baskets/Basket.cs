using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Accounting;

namespace Dressca.ApplicationCore.Baskets;

/// <summary>
///  買い物かごを表現するエンティティです。
/// </summary>
public class Basket
{
    private readonly List<BasketItem> items = [];
    private string buyerId;

    /// <summary>
    ///  <see cref="Basket"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public Basket()
    {
    }

    /// <summary>
    ///  買い物かご Id を取得します。
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    ///  購入者 Id を取得します。
    /// </summary>
    /// <exception cref="ArgumentNullException"><see langword="null"/> を設定できません。</exception>
    public required string BuyerId
    {
        get => this.buyerId;

        [MemberNotNull(nameof(buyerId))]
        init
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
            this.items.Add(new BasketItem { CatalogItemId = catalogItemId, UnitPrice = unitPrice, Quantity = quantity });
            return;
        }

        var existingItem = this.items.First(i => i.CatalogItemId == catalogItemId);
        existingItem.AddQuantity(quantity);
    }

    /// <summary>
    ///  買い物かご内のアイテムの数量を一括で設定します。
    /// </summary>
    /// <param name="quantities">各アイテムの数量。</param>
    public void SetItemsQuantity(Dictionary<long, int> quantities)
    {
        foreach (var item in this.Items)
        {
            if (quantities.TryGetValue(item.CatalogItemId, out var quantity))
            {
                item.SetQuantity(quantity);
            }
        }
    }

    /// <summary>
    ///  数量が 0 のアイテムを買い物かごから除去します。
    /// </summary>
    public void RemoveEmptyItems() => _ = this.items.RemoveAll(i => i.Quantity == 0);

    /// <summary>
    ///  この買い物かご内に指定したカタログアイテム Id の商品が含まれているかどうか示す値を取得します。
    /// </summary>
    /// <param name="catalogItemId">検査するカタログアイテム Id 。</param>
    /// <returns>含まれている場合は <see langword="true"/> 、そうでない場合は <see langword="false"/> 。</returns>
    public bool IsInCatalogItem(long catalogItemId)
        => this.items.Any(item => item.CatalogItemId == catalogItemId);

    /// <summary>
    ///  この買い物かごの情報をもとにした会計情報を取得します。
    /// </summary>
    /// <returns>会計情報。</returns>
    public Account GetAccount()
    {
        var accountItems = this.items
            .Select(basketItem => new AccountItem(basketItem.Quantity, basketItem.UnitPrice));
        return new Account(accountItems);
    }

    /// <summary>
    ///  買い物かごアイテムが空かどうか示す値を取得します。
    /// </summary>
    /// <returns>買い物かごアイテムが空の場合は <see langword="true"/> 、そうでない場合は <see langword="false"/> 。</returns>
    public bool IsEmpty() => this.items.Count == 0;
}
