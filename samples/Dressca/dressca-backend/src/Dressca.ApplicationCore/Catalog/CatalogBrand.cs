using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Catalog;

/// <summary>
///  カタログブランドエンティティ。
///  カタログアイテムの製造元や企画元に基づいて定義されるブランドを表現します。
/// </summary>
public class CatalogBrand
{
    private readonly List<CatalogItem> items = new();

    /// <summary>
    ///  <see cref="CatalogBrand"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="name">ブランド名。</param>
    /// <exception cref="ArgumentException">
    ///  <paramref name="name"/> が <see langword="null"/> または空の文字列です。
    /// </exception>
    public CatalogBrand(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(ApplicationCoreMessages.ArgumentIsNullOrWhiteSpace, nameof(name));
        }

        this.Name = name;
    }

    /// <summary>
    ///  カタログブランド Id を取得します。
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    ///  ブランド名を取得します。
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    ///  カタログアイテムのリストを取得します。
    /// </summary>
    public IReadOnlyCollection<CatalogItem> Items => this.items.AsReadOnly();
}
