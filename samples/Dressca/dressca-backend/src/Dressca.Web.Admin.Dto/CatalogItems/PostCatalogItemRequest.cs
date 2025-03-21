﻿using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Admin.Dto.CatalogItems;

/// <summary>
///  カタログにアイテムを追加する処理のリクエストデータを表します。
/// </summary>
public class PostCatalogItemRequest
{
    /// <summary>
    ///  アイテム名を取得または設定します。
    /// </summary>
    [Required]
    [StringLength(256)]
    public required string Name { get; set; }

    /// <summary>
    ///  説明を取得または設定します。
    /// </summary>
    [Required]
    [StringLength(1024)]
    public required string Description { get; set; }

    /// <summary>
    ///  単価を取得または設定します。
    /// </summary>
    [Required]
    [RegularExpression(@"^[1-9]\d{0,15}$")]
    public required long Price { get; set; }

    /// <summary>
    ///  商品コードを取得または設定します。
    /// </summary>
    [Required]
    [StringLength(128)]
    [RegularExpression(@"^[a-zA-Z0-9]+$")]
    public required string ProductCode { get; set; }

    /// <summary>
    ///  カタログカテゴリ ID を取得または設定します。
    /// </summary>
    [Required]
    [Range(1L, long.MaxValue)]
    public required long CatalogCategoryId { get; set; }

    /// <summary>
    ///  カタログブランド ID を取得または設定します。
    /// </summary>
    [Required]
    [Range(1L, long.MaxValue)]
    public required long CatalogBrandId { get; set; }
}
