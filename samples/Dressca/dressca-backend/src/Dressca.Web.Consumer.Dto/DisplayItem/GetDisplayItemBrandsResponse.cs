using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Consumer.Dto.DisplayItem;

/// <summary>
///  陳列品ブランドのレスポンスデータを表します。
///  陳列品の製造元や企画元に基づいて定義されるブランドを表現します。
/// </summary>
public class GetDisplayItemBrandsResponse
{
    /// <summary>
    ///  陳列品ブランド Id を取得または設定します。
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    ///  ブランド名を取得または設定します。
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;
}
