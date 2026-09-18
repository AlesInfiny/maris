namespace Dressca.ApplicationCore.DisplayItems;

/// <summary>
///  陳列品に関するドメインサービスを提供します。
/// </summary>
public interface IDisplayItemDomainService
{
    /// <summary>
    ///  指定した陳列品 Id がリポジトリ内にすべて存在するか示す値を取得します。
    ///  また存在した陳列品の一覧を返却します。
    /// </summary>
    /// <param name="displayItemIds">存在することを確認する陳列品 Id 。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>
    ///  ExistsAll : すべて存在する場合は <see langword="true"/> 、一部でも不在の場合は <see langword="false"/> 。
    ///  DisplayItems : 存在した陳列品の一覧。
    /// </returns>
    Task<(bool ExistsAll, IReadOnlyList<DisplayItem> DisplayItems)> ExistsAllAsync(IEnumerable<Guid> displayItemIds, CancellationToken cancellationToken = default);
}
