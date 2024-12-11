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

    /// <summary>
    ///  指定した Id のカタログアイテムがリポジトリ内に存在するかどうかを示す真理値を取得します。
    /// </summary>
    /// <param name="catalogItemId">カタログアイテム Id 。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>指定したアイテムがリポジトリ内に存在する場合は <see langword="true"/>、存在しない場合は <see langword="false"/>。</returns>
    Task<bool> ItemExistsAsync(long catalogItemId, CancellationToken cancellationToken = default);

    /// <summary>
    ///  指定した Id のカタログブランドがリポジトリ内に存在するかどうかを示す真理値を取得します。
    /// </summary>
    /// <param name="catalogBrandId">カタログブランド Id。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>指定したカタログブランドがリポジトリ内に存在する場合は <see langword="true"/>、存在しない場合は <see langword="false"/>。</returns>
    Task<bool> BrandExistsAsync(long catalogBrandId, CancellationToken cancellationToken = default);

    /// <summary>
    ///  指定した Id のカタログカテゴリがリポジトリ内に存在するかどうかを示す真理値を取得します。
    /// </summary>
    /// <param name="catalogCategoryId">カタログカテゴリ Id。</param>
    /// <param name="cancellationToken">キャンセルトークン。</param>
    /// <returns>指定したカタログカテゴリがリポジトリ内に存在する場合は <see langword="true"/>、存在しない場合は <see langword="false"/>。</returns>
    Task<bool> CategoryExistsAsync(long catalogCategoryId, CancellationToken cancellationToken = default);
}
