namespace Dressca.ApplicationCore.Ordering;

/// <summary>
///  注文の情報にアクセスするリポジトリです。
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

    /// <summary>
    ///  指定した Id のエンティティを取得します。
    ///  存在しない場合、タスクの結果は <see langword="null"/> を返します。
    /// </summary>
    /// <param name="id">Id 。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>注文情報を返す非同期処理を表すタスク。</returns>
    Task<Order?> FindAsync(long id, CancellationToken cancellationToken = default);
}
