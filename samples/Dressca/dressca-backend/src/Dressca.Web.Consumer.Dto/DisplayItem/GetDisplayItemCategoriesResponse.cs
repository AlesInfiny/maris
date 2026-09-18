using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Consumer.Dto.DisplayItem;

/// <summary>
///  陳列品カテゴリのレスポンスデータを表します。
/// </summary>
public class GetDisplayItemCategoriesResponse
{
    /// <summary>
    ///  陳列品カテゴリ Id を取得または設定します。
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    ///  カテゴリ名を取得または設定します。
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;
}
