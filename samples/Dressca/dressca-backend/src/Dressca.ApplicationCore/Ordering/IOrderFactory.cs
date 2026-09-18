using Dressca.ApplicationCore.Baskets;
using Dressca.ApplicationCore.DisplayItems;

namespace Dressca.ApplicationCore.Ordering;

/// <summary>
///  注文エンティティを生成するファクトリーです。
/// </summary>
public interface IOrderFactory
{
    /// <summary>
    ///  注文エンティティを生成します。
    /// </summary>
    /// <param name="basket">買い物かご。</param>
    /// <param name="displayItems">買い物かご内に存在する陳列品の一覧。</param>
    /// <param name="shipToAddress">お届け先。</param>
    /// <returns>注文エンティティ。</returns>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="basket"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="displayItems"/> が <see langword="null"/> です。</item>
    ///   <item><paramref name="shipToAddress"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    Order CreateOrder(Basket basket, IReadOnlyList<DisplayItem> displayItems, ShipTo shipToAddress);
}
