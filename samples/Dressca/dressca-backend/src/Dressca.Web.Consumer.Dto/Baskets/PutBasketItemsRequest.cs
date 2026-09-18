using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Consumer.Dto.Baskets;

/// <summary>
///  買い物かごの陳列品数量を変更する処理のリクエストデータを表します。
/// </summary>
public class PutBasketItemsRequest
{
    /// <summary>
    ///  陳列品 Id を取得または設定します。
    ///  1 以上の買い物かご、およびシステムに存在する陳列品 Id を指定してください。
    /// </summary>
    [Required]
    public Guid? DisplayItemId { get; set; }

    /// <summary>
    ///  数量を取得または設定します。
    ///  0 以上の値を設定してください。
    /// </summary>
    [Required]
    [Range(0, int.MaxValue)]
    public int? Quantity { get; set; }
}
