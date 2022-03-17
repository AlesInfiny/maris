using Dressca.Web.Dto.Accounting;

namespace Dressca.Web.Dto.Baskets;

/// <summary>
///  買い物かごを表す DTO です。
/// </summary>
public class BasketDto
{
    /// <summary>
    ///  購入者 Id を取得または設定します。
    /// </summary>
    public string BuyerId { get; set; } = string.Empty;

    /// <summary>
    ///  会計情報を取得または設定します。
    /// </summary>
    public AccountDto? Account { get; set; }

    /// <summary>
    ///  買い物かごアイテムのリストを取得または設定します。
    /// </summary>
    public IList<BasketItemDto> BasketItems { get; set; } = new List<BasketItemDto>();
}
