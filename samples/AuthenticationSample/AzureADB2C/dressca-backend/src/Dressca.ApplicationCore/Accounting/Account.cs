namespace Dressca.ApplicationCore.Accounting;

/// <summary>
///  会計情報を表現する値オブジェクトです。
/// </summary>
public record Account
{
    private readonly IEnumerable<AccountItem> accountItems;

    /// <summary>
    ///  <see cref="Account"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="accountItems">会計アイテムのリスト。</param>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="accountItems"/> が <see langword="null"/> です。
    /// </exception>
    public Account(IEnumerable<AccountItem> accountItems)
        => this.accountItems = accountItems ?? throw new ArgumentNullException(nameof(accountItems));

    /// <summary>
    ///  消費税率を取得します。
    ///  簡単化のため、本サンプルでは 10% 固定です。
    /// </summary>
    public static decimal ConsumptionTaxRate => 0.1m;

    /// <summary>
    ///  会計アイテムの合計金額を取得します。
    ///  この計算結果には、消費税や送料は含まれません。
    /// </summary>
    /// <returns>会計アイテムの合計金額。</returns>
    public decimal GetItemsTotalPrice()
        => this.accountItems.Sum(item => item.GetSubTotal());

    /// <summary>
    ///  税抜きの送料を取得します。
    ///  送料は会計アイテムの合計金額が 5,000 円以上で無料になります。
    ///  それ以外の場合 500 円です。
    ///  ただし、会計アイテムが登録されていない場合は 0 円を返します。
    /// </summary>
    /// <returns>送料。</returns>
    public decimal GetDeliveryCharge()
    {
        if (this.accountItems.Any())
        {
            return this.GetItemsTotalPrice() >= 5000m ? 0m : 500m;
        }
        else
        {
            return 0m;
        }
    }

    /// <summary>
    ///  消費税額の合計を取得します。
    ///  会計アイテムの合計金額に送料を加えた額に、消費税率を乗じて計算します。
    ///  0 円未満の端数は切り捨てます。
    /// </summary>
    /// <returns>消費税額。</returns>
    public decimal GetConsumptionTax()
        => Math.Truncate((this.GetItemsTotalPrice() + this.GetDeliveryCharge()) * ConsumptionTaxRate);

    /// <summary>
    ///  税込みの合計金額を取得します。
    /// </summary>
    /// <returns>合計金額。</returns>
    public decimal GetTotalPrice()
        => this.GetItemsTotalPrice() + this.GetDeliveryCharge() + this.GetConsumptionTax();
}
