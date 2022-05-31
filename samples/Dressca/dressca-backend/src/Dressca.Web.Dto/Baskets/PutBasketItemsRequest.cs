using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Dto.Baskets;

/// <summary>
///  買い物かごのカタログアイテム数量を変更する処理のリクエストデータを表します。
/// </summary>
public class PutBasketItemsRequest
{
    /// <summary>
    ///  カタログアイテム Id を取得または設定します。
    ///  1 以上の買い物かご、およびシステムに存在するカタログアイテム Id を指定してください。
    /// </summary>
    [Required]
    [Range(1L, long.MaxValue)]
    public long? CatalogItemId { get; set; }

    /// <summary>
    ///  数量を取得または設定します。
    ///  0 以上の値を設定してください。
    /// </summary>
    [Required]
    [Range(0, int.MaxValue)]
    public int? Quantity { get; set; }
}
