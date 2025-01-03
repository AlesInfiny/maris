﻿using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Consumer.Dto.Baskets;

/// <summary>
///  買い物かごにカタログアイテムを追加する処理のリクエストデータを表します。
/// </summary>
public class PostBasketItemsRequest
{
    /// <summary>
    ///  カタログアイテム Id を取得または設定します。
    ///  1 以上の買い物かご、およびシステムに存在するカタログアイテム Id を指定してください。
    /// </summary>
    [Required]
    [Range(1L, long.MaxValue)]
    public long? CatalogItemId { get; set; }

    /// <summary>
    ///  数量を取得または設定します。
    ///  カタログアイテム Id に指定した商品が買い物かごに含まれている場合、負の値を指定すると買い物かごから指定した数だけ取り出します。
    ///  未指定の場合は 1 です。
    /// </summary>
    public int? AddedQuantity { get; set; } = 1;
}
