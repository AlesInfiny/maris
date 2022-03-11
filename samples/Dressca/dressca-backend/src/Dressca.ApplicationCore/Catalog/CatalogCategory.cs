using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Catalog;

/// <summary>
///  カタログカテゴリエンティティ。
/// </summary>
public class CatalogCategory
{
    private readonly List<CatalogItem> items = new();

    /// <summary>
    ///  <see cref="CatalogCategory"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="name">カテゴリ名。</param>
    /// <exception cref="ArgumentException">
    ///  <paramref name="name"/> が <see langword="null"/> または空の文字列です。
    /// </exception>
    public CatalogCategory(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(ApplicationCoreMessages.ArgumentIsNullOrWhiteSpace, nameof(name));
        }

        this.Name = name;
    }

    /// <summary>
    ///  カタログカテゴリ Id を取得します。
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    ///  カテゴリ名を取得します。
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    ///  カタログアイテムのリストを取得します。
    /// </summary>
    public IReadOnlyCollection<CatalogItem> Items => this.items.AsReadOnly();
}
