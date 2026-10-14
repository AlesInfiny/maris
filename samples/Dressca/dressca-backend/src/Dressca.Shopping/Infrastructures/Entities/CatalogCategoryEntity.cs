namespace Dressca.Shopping.Infrastructures.Entities;

/// <summary>
///  陳列品の情報源となるカタログカテゴリを読み取るためのエンティティです。
///  テーブルはカタログ管理コンテキストが所有します。
/// </summary>
internal class CatalogCategoryEntity
{
    /// <summary>
    ///  カタログカテゴリ ID を取得します。
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///  カテゴリ名を取得します。
    /// </summary>
    public string Name { get; init; } = string.Empty;
}
