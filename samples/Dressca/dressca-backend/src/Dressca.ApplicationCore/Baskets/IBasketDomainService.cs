using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dressca.ApplicationCore.Baskets;

public interface IBasketDomainService
{
    /// <summary>
    ///  アイテムを買い物かごに追加します。
    /// </summary>
    /// <param name="basketId">買い物かご Id。</param>
    /// <param name="catalogItemId">カタログアイテム Id 。</param>
    /// <param name="price">単価。</param>
    /// <param name="quantity">数量。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    /// <exception cref="BasketNotFoundException">買い物かごが見つからない場合。</exception>
    Task AddItemToBasketAsync(long basketId, long catalogItemId, decimal price, int quantity = 1, CancellationToken cancellationToken = default);

    /// <summary>
    ///  買い物かごを削除します。
    /// </summary>
    /// <param name="basketId">買い物かご Id 。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    /// <exception cref="BasketNotFoundException">買い物かごが見つからない場合。</exception>
    Task DeleteBasketAsync(long basketId, CancellationToken cancellationToken = default);

    /// <summary>
    ///  買い物かごの各アイテムの数量を一括で設定します。
    /// </summary>
    /// <param name="basketId">買い物かご Id 。</param>
    /// <param name="quantities">カタログアイテム Id ごとの数量。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="quantities"/> が <see langword="null"/> です。</exception>
    /// <exception cref="InvalidOperationException">買い物かご内のいずれかのアイテムの数量が 0 未満になる場合。</exception>
    /// <exception cref="BasketNotFoundException">買い物かごが見つからない場合。</exception>
    Task SetQuantitiesAsync(long basketId, Dictionary<long, int> quantities, CancellationToken cancellationToken = default);

    /// <summary>
    ///  <paramref name="buyerId"/> に対応する買い物かご情報を取得します。
    ///  対応する買い物かご情報がない場合は、作成します。
    /// </summary>
    /// <param name="buyerId">購入者 Id 。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>買い物かご情報を返す非同期処理を表すタスク。</returns>
    /// <exception cref="ArgumentException"><paramref name="buyerId"/> が <see langword="null"/> または空白の場合.</exception>
    Task<Basket> GetOrCreateBasketForUserAsync(string buyerId, CancellationToken cancellationToken = default);
}
