namespace Dressca.ApplicationCore.Ordering;

/// <summary>
///  注文リポジトリ。
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    ///  エンティティを追加します。
    /// </summary>
    /// <param name="entity">エンティティ。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>追加された注文情報を返す非同期処理を表すタスク。</returns>
    Task<Order> AddAsync(Order entity, CancellationToken cancellationToken = default);
}
