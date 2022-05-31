using System.ComponentModel.DataAnnotations;
using Dressca.Web.Dto.Accounting;

namespace Dressca.Web.Dto.Baskets;

/// <summary>
///  買い物かごのレスポンスデータを表します。
/// </summary>
public class BasketResponse
{
    /// <summary>
    ///  購入者 Id を取得または設定します。
    /// </summary>
    [Required]
    public string BuyerId { get; set; } = string.Empty;

    /// <summary>
    ///  会計情報を取得または設定します。
    /// </summary>
    public AccountResponse? Account { get; set; }

    /// <summary>
    ///  買い物かごアイテムのリストを取得または設定します。
    /// </summary>
    public IList<BasketItemResponse> BasketItems { get; set; } = new List<BasketItemResponse>();
}
