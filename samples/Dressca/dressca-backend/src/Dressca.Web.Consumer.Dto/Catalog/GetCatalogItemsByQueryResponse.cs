using Dressca.SystemCommon;

namespace Dressca.Web.Consumer.Dto.Catalog;

/// <summary>
///  カタログアイテムの検索クエリに対するレスポンスデータを表します。
/// </summary>
public class GetCatalogItemsByQueryResponse
{
    /// <summary>
    ///  カタログアイテムのリストを取得または設定します。
    /// </summary>
    public required PagedList<CatalogItemApiModel> CatalogItems { get; set; }
}
