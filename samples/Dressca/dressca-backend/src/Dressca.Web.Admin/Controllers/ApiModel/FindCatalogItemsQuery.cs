using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Dressca.Web.Admin.Controllers.ApiModel;

/// <summary>
///  カタログアイテムの検索クエリを表します。
/// </summary>
public class FindCatalogItemsQuery
{
    /// <summary>
    ///  カタログブランド ID です。
    ///  未設定の場合は全カタログブランドを対象にします。
    /// </summary>
    [FromQuery(Name = "brandId")]
    public long? BrandId { get; set; }

    /// <summary>
    ///  カタログカテゴリ ID です。
    ///  未設定の場合は全カタログカテゴリを対象にします。
    /// </summary>
    [FromQuery(Name = "categoryId")]
    public long? CategoryId { get; set; }

    /// <summary>
    ///  ページ番号です。
    ///  未設定の場合は 1 ページ目として扱います。
    ///  1 以上の整数値を指定できます。
    /// </summary>
    [FromQuery(Name = "page")]
    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    /// <summary>
    ///  1 ページに収めるアイテムの数です。
    ///  未設定の場合は 20 個です。
    ///  1 以上 50 以下の整数値を指定できます。
    /// </summary>
    [FromQuery(Name = "pageSize")]
    [Range(1, 50)]
    public int PageSize { get; set; } = 20;

    /// <summary>
    ///  読み飛ばす項目数を取得します。
    /// </summary>
    /// <returns>読み飛ばす項目数。</returns>
    public int GetSkipCount() => (this.Page - 1) * this.PageSize;
}
