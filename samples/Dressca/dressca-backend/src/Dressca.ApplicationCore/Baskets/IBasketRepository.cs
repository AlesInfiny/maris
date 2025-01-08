namespace Dressca.ApplicationCore.Baskets;

/// <summary>
///  買い物かごの情報にアクセスするリポジトリです。
/// </summary>
public interface IBasketRepository
{
    /// <summary>
    ///  指定した識別子のエンティティを取得します。
    /// </summary>
    /// <param name="id">ID 。</param>
    /// <param name="cancellationToken">キャンセルトークン 。</param>
    /// <returns>買い物かご情報を返す非同期処理を表すタスク。</returns>
    Task<Basket?> GetAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    ///  エンティティを追加します。
    /// </summary>
    /// <param name="entity">エンティティ。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>買い物かご情報を返す非同期処理を表すタスク。</returns>
    Task<Basket> AddAsync(Basket entity, CancellationToken cancellationToken = default);

    /// <summary>
    ///  エンティティを削除します。
    /// </summary>
    /// <param name="entity">エンティティ。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    Task RemoveAsync(Basket entity, CancellationToken cancellationToken = default);

    /// <summary>
    ///  エンティティを更新します。
    /// </summary>
    /// <param name="entity">エンティティ。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>非同期処理を表すタスク。</returns>
    Task UpdateAsync(Basket entity, CancellationToken cancellationToken = default);

    /// <summary>
    ///  Id が <paramref name="basketId"/> に一致する買い物かご情報を、明細情報と共に取得します。
    /// </summary>
    /// <param name="basketId">買い物かご Id 。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>買い物かご情報を返す非同期処理を表すタスク。</returns>
    Task<Basket?> GetWithBasketItemsAsync(long basketId, CancellationToken cancellationToken = default);

    /// <summary>
    ///  購入者 Id が <paramref name="buyerId"/> に一致する買い物かご情報を、明細情報と共に取得します。
    /// </summary>
    /// <param name="buyerId">購入者 Id 。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>買い物かご情報を返す非同期処理を表すタスク。</returns>
    Task<Basket?> GetWithBasketItemsAsync(string buyerId, CancellationToken cancellationToken = default);
}
