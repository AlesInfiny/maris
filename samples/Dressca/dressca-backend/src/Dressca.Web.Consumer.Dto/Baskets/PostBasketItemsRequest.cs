using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Consumer.Dto.Baskets;

/// <summary>
///  買い物かごに陳列品を追加する処理のリクエストデータを表します。
/// </summary>
public class PostBasketItemsRequest
{
    /// <summary>
    ///  陳列品 Id を取得または設定します。
    ///  1 以上の買い物かご、およびシステムに存在する陳列品 Id を指定してください。
    /// </summary>
    [Required]
    public Guid? DisplayItemId { get; set; }

    /// <summary>
    ///  数量を取得または設定します。
    ///  陳列品 Id に指定した商品が買い物かごに含まれている場合、負の値を指定すると買い物かごから指定した数だけ取り出します。
    ///  未指定の場合は 1 です。
    /// </summary>
    public int? AddedQuantity { get; set; } = 1;
}
