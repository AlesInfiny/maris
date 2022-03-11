namespace Dressca.ApplicationCore.Catalog;

/// <summary>
///  カタログブランドリポジトリ。
/// </summary>
public interface ICatalogCategoryRepository
{
    /// <summary>
    ///  すべてのエンティティを取得します。
    /// </summary>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>カタログカテゴリのリストを返す非同期処理を表すタスク。</returns>
    Task<IReadOnlyList<CatalogCategory>> GetAllAsync(CancellationToken cancellationToken = default);
}
