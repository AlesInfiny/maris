namespace Dressca.ApplicationCore.DisplayItems;

/// <summary>
///  陳列品ブランドの情報にアクセスするリポジトリです。
/// </summary>
public interface IDisplayItemBrandRepository
{
    /// <summary>
    ///  すべてのエンティティを取得します。
    /// </summary>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>陳列品ブランドのリストを返す非同期処理を表すタスク。</returns>
    Task<IReadOnlyList<DisplayItemBrand>> GetAllAsync(CancellationToken cancellationToken = default);
}
