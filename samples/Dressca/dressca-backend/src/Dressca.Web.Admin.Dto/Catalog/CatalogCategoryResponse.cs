﻿using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Admin.Dto.Catalog;

/// <summary>
///  カタログカテゴリのレスポンスデータを表します。
/// </summary>
public class CatalogCategoryResponse
{
    /// <summary>
    ///  カタログカテゴリ Id を取得または設定します。
    /// </summary>
    [Required]
    public long Id { get; set; }

    /// <summary>
    ///  カテゴリ名を取得または設定します。
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;
}
