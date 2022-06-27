using Dressca.ApplicationCore.Accounting;
using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Baskets;

/// <summary>
///  買い物かごアイテムです。
/// </summary>
public class BasketItem
{
    private Basket? basket;
    private int quantity;

    /// <summary>
    ///  <see cref="BasketItem"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="catalogItemId">カタログアイテム Id 。</param>
    /// <param name="unitPrice">単価。</param>
    /// <param name="quantity">数量。</param>
    /// <exception cref="ArgumentException"><paramref name="quantity"/> が 0 未満の場合。</exception>
    public BasketItem(long catalogItemId, decimal unitPrice, int quantity)
    {
        this.CatalogItemId = catalogItemId;
        this.UnitPrice = unitPrice;
        this.Quantity = quantity;
    }

    /// <summary>
    ///  買い物かごアイテム Id を取得します。
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    ///  買い物かご（ナビゲーションプロパティ）を取得します。
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="Basket"/> が設定されていません。</exception>
    public Basket Basket
    {
        get => this.basket ?? throw new InvalidOperationException(string.Format(Messages.PropertyNotInitialized, nameof(this.Basket)));
        private set => this.basket = value;
    }

    /// <summary>
    ///  買い物かご Id を取得します。
    /// </summary>
    public long BasketId { get; private set; }

    /// <summary>
    ///  カタログアイテム Id を取得します。
    /// </summary>
    public long CatalogItemId { get; private set; }

    /// <summary>
    ///  単価を取得します。
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    ///  数量を取得します。
    /// </summary>
    /// <exception cref="ArgumentException">0 未満を設定した場合。</exception>
    public int Quantity
    {
        get => this.quantity;
        private set
        {
            if (value < 0)
            {
                throw new ArgumentException(Messages.BasketItemQuantityMustBePositive, nameof(value));
            }

            this.quantity = value;
        }
    }

    /// <summary>
    ///  数量を <paramref name="quantity"/> 分加算します。
    /// </summary>
    /// <param name="quantity">加算する数量。</param>
    /// <exception cref="ArgumentException">加算後の数量が負になる場合。オーバーフローする場合も同様。</exception>
    public void AddQuantity(int quantity) => this.SetQuantity(this.Quantity + quantity);

    /// <summary>
    ///  数量を設定します。
    /// </summary>
    /// <param name="quantity">数量。</param>
    /// <exception cref="ArgumentException"><paramref name="quantity"/> が 0 未満の場合。</exception>
    public void SetQuantity(int quantity) => this.Quantity = quantity;

    /// <summary>
    ///  買い物かごアイテムの小計を計算して金額を返却します。
    /// </summary>
    /// <returns>買い物かごアイテムの小計額。</returns>
    public decimal GetSubTotal()
        => new AccountItem(this.Quantity, this.UnitPrice).GetSubTotal();
}
