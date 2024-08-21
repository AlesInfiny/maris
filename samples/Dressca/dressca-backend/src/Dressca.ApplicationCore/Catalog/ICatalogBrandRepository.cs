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

    /// <summary>
    ///  指定した識別子のエンティティを取得します。
    /// </summary>
    /// <param name="id">ID 。</param>
    /// <param name="cancellationToken">キャンセルトークン 。</param>
    /// <returns>買い物かご情報を返す非同期処理を表すタスク。</returns>
    Task<CatalogBrand?> GetAsync(long id, CancellationToken cancellationToken = default);
}
