namespace Dressca.Shopping.Infrastructures.Entities;

/// <summary>
///  陳列品の情報源となるカタログアイテムを読み取るためのエンティティです。
///  テーブルはカタログ管理コンテキストが所有します。
/// </summary>
internal class CatalogItemEntity
{
    /// <summary>
    ///  カタログアイテム ID を取得します。
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///  商品名を取得します。
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    ///  説明を取得します。
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    ///  単価を取得します。
    /// </summary>
    public decimal Price { get; init; }

    /// <summary>
    ///  商品コードを取得します。
    /// </summary>
    public string ProductCode { get; init; } = string.Empty;

    /// <summary>
    ///  カタログブランド ID を取得します。
    /// </summary>
    public Guid CatalogBrandId { get; init; }

    /// <summary>
    ///  カタログカテゴリ ID を取得します。
    /// </summary>
    public Guid CatalogCategoryId { get; init; }

    /// <summary>
    ///  削除済みかどうかを取得します。
    /// </summary>
    public bool IsDeleted { get; init; }

    /// <summary>
    ///  カタログアイテムアセットのリストを取得します。
    /// </summary>
    public IReadOnlyCollection<CatalogItemAssetEntity> Assets { get; init; } = [];
}
