namespace Dressca.ApplicationCore.Catalog;

/// <summary>
///  カタログブランドリポジトリ。
/// </summary>
public interface ICatalogBrandRepository
{
    /// <summary>
    ///  すべてのエンティティを取得します。
    /// </summary>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>カタログブランドのリストを返す非同期処理を表すタスク。</returns>
    Task<IReadOnlyList<CatalogBrand>> GetAllAsync(CancellationToken cancellationToken = default);
}
