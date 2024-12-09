﻿using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Admin.Dto.Catalog;

/// <summary>
/// カタログにアイテムを追加する処理のリクエストデータを表します。
/// </summary>
public class PostCatalogItemRequest
{
    /// <summary>
    ///  アイテム名を取得または設定します。
    /// </summary>
    [Required]
    [StringLength(256)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///  説明を取得または設定します。
    /// </summary>
    [Required]
    [StringLength(1024)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 単価を取得または設定します。
    /// </summary>
    [Required]
    [RegularExpression(@"^[1-9]\d{0,15}$")]
    public long Price { get; set; }

    /// <summary>
    ///  商品コードを取得または設定します。
    /// </summary>
    [Required]
    [StringLength(128)]
    [RegularExpression(@"^[a-zA-Z0-9]+$")]
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    ///  カタログカテゴリIDを取得または設定します。
    /// </summary>
    [Required]
    [Range(1L, long.MaxValue)]
    public long CatalogCategoryId { get; set; }

    /// <summary>
    ///  カタログブランドIDを取得または設定します。
    /// </summary>
    [Required]
    [Range(1L, long.MaxValue)]
    public long CatalogBrandId { get; set; }

}
