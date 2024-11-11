namespace Dressca.Web.Consumer.Baskets;

/// <summary>
///  BuyerId の値を入出力できる <see cref="HttpContext"/> の拡張機能を提供します。
/// </summary>
/// <remarks>
///  <para>
///   購入者 Id の採番と保存は <see cref="BuyerIdFilterAttribute"/> で実施します。
///  </para>
/// </remarks>
public static class HttpContextExtensions
{
    private const string BuyerIdHttpContextItemKey = "Dressca-BuyerId";

    /// <summary>
    ///  <paramref name="httpContext"/> に保存されている購入者 Id を取得します。
    ///  値が設定されていない場合は、新しい購入者 Id を採番し、 <paramref name="httpContext"/> に保存してから返却します。
    /// </summary>
    /// <param name="httpContext">購入者 Id を取得する <see cref="HttpContext"/> オブジェクト。</param>
    /// <returns>購入者 Id 。</returns>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="httpContext"/> が <see langword="null"/> です。
    /// </exception>
    public static string GetBuyerId(this HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        if (httpContext.Items.TryGetValue(BuyerIdHttpContextItemKey, out var value))
        {
            if (value is string buyerId && !string.IsNullOrWhiteSpace(buyerId) && Guid.TryParse(buyerId, out _))
            {
                return buyerId;
            }
        }

        return Guid.NewGuid().ToString("D");
    }

    /// <summary>
    ///  <paramref name="httpContext"/> に <paramref name="buyerId"/> に指定した購入者 Id を保存します。
    /// </summary>
    /// <param name="httpContext">購入者 Id を保存する <see cref="HttpContent"/> オブジェクト。</param>
    /// <param name="buyerId">購入者 Id 。</param>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="httpContext"/> が <see langword="null"/> です。
    /// </exception>
    public static void SetBuyerId(this HttpContext httpContext, string buyerId)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        if (httpContext.Items.ContainsKey(BuyerIdHttpContextItemKey))
        {
            httpContext.Items[BuyerIdHttpContextItemKey] = buyerId;
        }
        else
        {
            httpContext.Items.Add(BuyerIdHttpContextItemKey, buyerId);
        }
    }
}
