namespace Dressca.ApplicationCore.Accounting;

/// <summary>
///  会計アイテムを表現する値オブジェクトです。
/// </summary>
public record AccountItem
{
    /// <summary>
    ///  <see cref="AccountItem"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="quantity">数量。</param>
    /// <param name="unitPrice">単価。</param>
    public AccountItem(int quantity, decimal unitPrice)
    {
        this.Quantity = quantity;
        this.UnitPrice = unitPrice;
    }

    /// <summary>
    ///  数量を取得します。
    /// </summary>
    public int Quantity { get; init; }

    /// <summary>
    ///  単価を取得します。
    /// </summary>
    public decimal UnitPrice { get; init; }
}
