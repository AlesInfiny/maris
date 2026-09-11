namespace Dressca.ApplicationCore.DisplayItems;

/// <summary>
///  陳列品カテゴリの情報にアクセスするリポジトリです。
/// </summary>
public interface IDisplayItemCategoryRepository
{
    /// <summary>
    ///  すべてのエンティティを取得します。
    /// </summary>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>陳列品カテゴリのリストを返す非同期処理を表すタスク。</returns>
    Task<IReadOnlyList<DisplayItemCategory>> GetAllAsync(CancellationToken cancellationToken = default);
}
