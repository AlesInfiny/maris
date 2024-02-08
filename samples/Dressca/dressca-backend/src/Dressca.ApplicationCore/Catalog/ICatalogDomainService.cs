using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dressca.ApplicationCore.Catalog;

/// <summary>
///  カタログドメインサービス。
/// </summary>
public interface ICatalogDomainService
{
    /// <summary>
    ///  指定したカタログアイテム Id がリポジトリ内にすべて存在するか示す値を取得します。
    ///  また存在したカタログアイテムの一覧を返却します。
    /// </summary>
    /// <param name="catalogItemIds">存在することを確認するカタログアイテム Id 。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>
    ///  ExistsAll : すべて存在する場合は <see langword="true"/> 、一部でも不在の場合は <see langword="false"/> 。
    ///  CatalogItems : 存在したカタログアイテムの一覧。
    /// </returns>
    Task<(bool ExistsAll, IReadOnlyList<CatalogItem> CatalogItems)> ExistsAllAsync(IEnumerable<long> catalogItemIds, CancellationToken cancellationToken = default);
}
