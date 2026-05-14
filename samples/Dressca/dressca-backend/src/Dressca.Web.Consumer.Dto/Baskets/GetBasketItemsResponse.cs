using System.ComponentModel.DataAnnotations;
using Dressca.Web.Consumer.Dto.Accounting;

namespace Dressca.Web.Consumer.Dto.Baskets;

/// <summary>
///  買い物かごのレスポンスデータを表します。
/// </summary>
public class GetBasketItemsResponse
{
    /// <summary>
    ///  購入者 Id を取得または設定します。
    /// </summary>
    [Required]
    public string BuyerId { get; set; } = string.Empty;

    /// <summary>
    ///  会計情報を取得または設定します。
    /// </summary>
    public AccountApiModel? Account { get; set; }

    /// <summary>
    ///  買い物かごアイテムのリストを取得または設定します。
    /// </summary>
    public IList<BasketItemApiModel> BasketItems { get; set; } = [];

    /// <summary>
    ///  削除済みカタログアイテムの Id のリストを取得または設定します。
    /// </summary>
    public IList<long> DeletedItemIds { get; set; } = [];
}
