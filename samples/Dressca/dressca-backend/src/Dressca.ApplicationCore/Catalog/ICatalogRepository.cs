using System.Linq.Expressions;

namespace Dressca.ApplicationCore.Catalog;

/// <summary>
///  カタログリポジトリ。
/// </summary>
public interface ICatalogRepository
{
    /// <summary>
    ///  指定した識別子のエンティティを取得します。
    /// </summary>
    /// <param name="id">ID 。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>カタログアイテムを返す非同期処理を表すタスク。</returns>
    Task<CatalogItem?> GetAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    ///  仕様を満たすエンティティのリストを取得します。
    /// </summary>
    /// <param name="specification">検索対象のエンティティが満たすべき仕様。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>
    ///  カタログアイテムを返す非同期処理を表すタスク。
    ///  Task の結果は仕様を満たすエンティティのリストです。
    ///  仕様を満たすエンティティが存在しない場合、それは空のリストになります。
    /// </returns>
    Task<IReadOnlyList<CatalogItem>> FindAsync(Expression<Func<CatalogItem, bool>> specification, CancellationToken cancellationToken = default);

    /// <summary>
    ///  仕様を満たすエンティティのリストを取得します。
    /// </summary>
    /// <param name="specification">検索対象のエンティティが満たすべき仕様。</param>
    /// <param name="skip">読み飛ばす項目数。</param>
    /// <param name="take">最大取得数。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>
    ///  カタログアイテムを返す非同期処理を表すタスク。
    ///  Task の結果は仕様を満たすエンティティのリストです。
    ///  仕様を満たすエンティティが存在しない場合、それは空のリストになります。
    /// </returns>
    Task<IReadOnlyList<CatalogItem>> FindAsync(Expression<Func<CatalogItem, bool>> specification, int skip, int take, CancellationToken cancellationToken = default);

    /// <summary>
    ///  条件を満たすカタログアイテムの数を返却します。
    /// </summary>
    /// <param name="specification">条件式。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>条件を満たすカタログアイテム数を返す非同期処理を表すタスク。</returns>
    Task<int> CountAsync(Expression<Func<CatalogItem, bool>> specification, CancellationToken cancellationToken = default);

    /// <summary>
    ///  複数のエンティティを追加します。
    /// </summary>
    /// <param name="entities">複数のエンティティ。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>カタログアイテムを返す非同期処理を表すタスク。</returns>
    Task<IReadOnlyList<CatalogItem>> AddRangeAsync(IEnumerable<CatalogItem> entities, CancellationToken cancellationToken = default);
}
