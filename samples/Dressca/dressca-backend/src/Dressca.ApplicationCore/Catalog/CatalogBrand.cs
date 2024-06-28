using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Catalog;

/// <summary>
///  カタログブランドエンティティ。
///  カタログアイテムの製造元や企画元に基づいて定義されるブランドを表現します。
/// </summary>
public class CatalogBrand
{
    private readonly List<CatalogItem> items = [];
    private string name;

    /// <summary>
    ///  <see cref="CatalogBrand"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public CatalogBrand()
    {
    }

    /// <summary>
    ///  カタログブランド Id を取得します。
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    ///  ブランド名を取得します。
    /// </summary>
    /// <exception cref="ArgumentException">null または空の文字列を設定できません。</exception>
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
