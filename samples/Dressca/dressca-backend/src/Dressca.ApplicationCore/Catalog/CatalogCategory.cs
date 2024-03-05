using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Catalog;

/// <summary>
///  カタログカテゴリエンティティ。
/// </summary>
public class CatalogCategory
{
    private readonly List<CatalogItem> items = new();
    private string name;

    /// <summary>
    ///  <see cref="CatalogCategory"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public CatalogCategory()
    {
    }

    /// <summary>
    ///  カタログカテゴリ Id を取得します。
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    ///  カテゴリ名を取得します。
    /// </summary>
    public required string Name
    {
        get => this.name;

        [MemberNotNull(nameof(name))]
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(Messages.ArgumentIsNullOrWhiteSpace, nameof(value));
            }

            this.name = value;
        }
    }

    /// <summary>
    ///  カタログアイテムのリストを取得します。
    /// </summary>
    public IReadOnlyCollection<CatalogItem> Items => this.items.AsReadOnly();
}
