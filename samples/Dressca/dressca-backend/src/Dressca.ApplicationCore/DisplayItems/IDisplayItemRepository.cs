using System.Linq.Expressions;

namespace Dressca.ApplicationCore.DisplayItems;

/// <summary>
///  陳列品の情報にアクセスするリポジトリです。
/// </summary>
public interface IDisplayItemRepository
{
    /// <summary>
    ///  仕様を満たすエンティティのリストを取得します。
    /// </summary>
    /// <param name="specification">検索対象のエンティティが満たすべき仕様。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>
    ///  陳列品を返す非同期処理を表すタスク。
    ///  Task の結果は仕様を満たすエンティティのリストです。
    ///  仕様を満たすエンティティが存在しない場合、それは空のリストになります。
    /// </returns>
    Task<IReadOnlyList<DisplayItem>> FindAsync(Expression<Func<DisplayItem, bool>> specification, CancellationToken cancellationToken = default);

    /// <summary>
    ///  仕様を満たすエンティティのリストを取得します。
    /// </summary>
    /// <param name="specification">検索対象のエンティティが満たすべき仕様。</param>
    /// <param name="skip">読み飛ばす項目数。</param>
    /// <param name="take">最大取得数。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>
    ///  陳列品を返す非同期処理を表すタスク。
    ///  Task の結果は仕様を満たすエンティティのリストです。
    ///  仕様を満たすエンティティが存在しない場合、それは空のリストになります。
    /// </returns>
    Task<IReadOnlyList<DisplayItem>> FindAsync(Expression<Func<DisplayItem, bool>> specification, int skip, int take, CancellationToken cancellationToken = default);

    /// <summary>
    ///  条件を満たす陳列品の数を返却します。
    /// </summary>
    /// <param name="specification">条件式。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>条件を満たす陳列品数を返す非同期処理を表すタスク。</returns>
    Task<int> CountAsync(Expression<Func<DisplayItem, bool>> specification, CancellationToken cancellationToken = default);
}
