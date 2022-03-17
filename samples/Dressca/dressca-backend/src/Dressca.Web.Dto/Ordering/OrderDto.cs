using Dressca.Web.Dto.Accounting;

namespace Dressca.Web.Dto.Ordering;

/// <summary>
///  注文情報の DTO です。
/// </summary>
public class OrderDto
{
    /// <summary>
    ///  注文 Id を取得または設定します。
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    ///  購入者 Id を取得または設定します。
    /// </summary>
    public string BuyerId { get; set; } = string.Empty;

    /// <summary>
    ///  注文日を取得または設定します。
    /// </summary>
    public DateTimeOffset OrderDate { get; set; }

    /// <summary>
    ///  お届け先氏名を取得または設定します。
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    ///  お届け先郵便番号を取得または設定します。
    /// </summary>
    public string PostalCode { get; set; } = string.Empty;

    /// <summary>
    ///  お届け先都道府県を取得または設定します。
    /// </summary>
    public string Todofuken { get; set; } = string.Empty;

    /// <summary>
    ///  お届け先市区町村を取得または設定します。
    /// </summary>
    public string Shikuchoson { get; set; } = string.Empty;

    /// <summary>
    ///  お届け先字／番地／建物名／部屋番号を取得または設定します。
    /// </summary>
    public string AzanaAndOthers { get; set; } = string.Empty;

    /// <summary>
    ///  会計情報を取得または設定します。
    /// </summary>
    public AccountDto? Account { get; set; }

    /// <summary>
    ///  注文アイテムのリストを取得または設定します。
    /// </summary>
    public IList<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
}
