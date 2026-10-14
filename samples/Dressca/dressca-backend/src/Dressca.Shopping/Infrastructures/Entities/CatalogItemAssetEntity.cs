namespace Dressca.Shopping.Infrastructures.Entities;

/// <summary>
///  陳列品の情報源となるカタログアイテムアセットを読み取るためのエンティティです。
///  テーブルはカタログ管理コンテキストが所有します。
/// </summary>
internal class CatalogItemAssetEntity
{
    /// <summary>
    ///  カタログアイテムアセット ID を取得します。
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///  アセットコードを取得します。
    /// </summary>
    public string AssetCode { get; init; } = string.Empty;

    /// <summary>
    ///  カタログアイテム ID を取得します。
    /// </summary>
    public Guid CatalogItemId { get; init; }
}
