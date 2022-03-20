using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Catalog;

/// <summary>
///  カタログアイテム エンティティです。
/// </summary>
public class CatalogItem
{
    private readonly List<CatalogItemAsset> assets = new();
    private CatalogCategory? catalogCategory;
    private CatalogBrand? catalogBrand;
    private string name;
    private string description;
    private decimal price;
    private string productCode;
    private long catalogCategoryId;
    private long catalogBrandId;

    /// <summary>
    ///  <see cref="CatalogItem"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="catalogCategoryId">カタログカテゴリ Id 。</param>
    /// <param name="catalogBrandId">カタログブランド Id 。</param>
    /// <param name="description">説明。</param>
    /// <param name="name">商品名。</param>
    /// <param name="price">単価。</param>
    /// <param name="productCode">商品コード。</param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///  <list type="bullet">
    ///   <item><paramref name="catalogCategoryId"/> は 0 以下に設定できません。</item>
    ///   <item><paramref name="catalogBrandId"/> は 0 以下に設定できません。</item>
    ///   <item><paramref name="price"/> は負の値に設定できません。</item>
    ///  </list>
    /// </exception>
    /// <exception cref="ArgumentException">
    ///  <list type="bullet">
    ///   <item><paramref name="description"/> が <see langword="null"/> または空の文字列です。</item>
    ///   <item><paramref name="name"/> が <see langword="null"/> または空の文字列です。</item>
    ///   <item><paramref name="productCode"/> が <see langword="null"/> または空の文字列です。</item>
    ///  </list>
    /// </exception>
    public CatalogItem(long catalogCategoryId, long catalogBrandId, string description, string name, decimal price, string productCode)
    {
        this.CatalogCategoryId = catalogCategoryId;
        this.CatalogBrandId = catalogBrandId;
        this.Description = description;
        this.Name = name;
        this.Price = price;
        this.ProductCode = productCode;
    }

    /// <summary>
    ///  カタログアイテム Id を取得します。
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    ///  商品名を取得します。
    /// </summary>
    /// <exception cref="ArgumentException">商品名が <see langword="null"/> または空の文字列です。</exception>
    public string Name
    {
        get => this.name;

        [MemberNotNull(nameof(name))]
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(ApplicationCoreMessages.ArgumentIsNullOrWhiteSpace, nameof(value));
            }

            this.name = value;
        }
    }

    /// <summary>
    ///  説明を取得します。
    /// </summary>
    /// <exception cref="ArgumentException">説明が <see langword="null"/> または空の文字列です。</exception>
    public string Description
    {
        get => this.description;

        [MemberNotNull(nameof(description))]
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(ApplicationCoreMessages.ArgumentIsNullOrWhiteSpace, nameof(value));
            }

            this.description = value;
        }
    }

    /// <summary>
    ///  単価を取得します。
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">単価は負の値に設定できません。</exception>
    public decimal Price
    {
        get => this.price;
        private set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(value),
                    actualValue: value,
                    message: ApplicationCoreMessages.PriceMustBeZeroOrHigher);
            }

            this.price = value;
        }
    }

    /// <summary>
    ///  商品コードを取得します。
    /// </summary>
    /// <remarks>
    ///  本来は存在するであろう在庫管理系のコンテキストで識別子として使用されるコードです。
    ///  買い物かごコンテキストとは CatalogItem.Id で連携するため、注意してください。
    /// </remarks>
    /// <exception cref="ArgumentException">商品コードが <see langword="null"/> または空の文字列です。</exception>
    public string ProductCode
    {
        get => this.productCode;

        [MemberNotNull(nameof(productCode))]
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(ApplicationCoreMessages.ArgumentIsNullOrWhiteSpace, nameof(value));
            }

            this.productCode = value;
        }
    }

    /// <summary>
    ///  カタログカテゴリ Id を取得します。
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">カタログカテゴリ ID は 0 以下に設定できません。</exception>
    public long CatalogCategoryId
    {
        get => this.catalogCategoryId;
        private set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(value),
                    actualValue: value,
                    message: ApplicationCoreMessages.CatalogCategoryIdMustBePositive);
            }

            this.catalogCategoryId = value;
        }
    }

    /// <summary>
    ///  カタログカテゴリを取得します。
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="CatalogCategory"/> が設定されていません。</exception>
    public CatalogCategory CatalogCategory
    {
        get => this.catalogCategory ?? throw new InvalidOperationException(string.Format(ApplicationCoreMessages.PropertyNotInitialized, nameof(this.CatalogCategory)));
        private set => this.catalogCategory = value;
    }

    /// <summary>
    ///  カタログブランド Id を取得します。
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">カタログブランド ID は 0 以下に設定できません。</exception>
    public long CatalogBrandId
    {
        get => this.catalogBrandId;
        private set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(value),
                    actualValue: value,
                    message: ApplicationCoreMessages.CatalogBrandIdMustBePositive);
            }

            this.catalogBrandId = value;
        }
    }

    /// <summary>
    ///  カタログブランドを取得します。
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="CatalogBrand"/> が設定されていません。</exception>
    public CatalogBrand CatalogBrand
    {
        get => this.catalogBrand ?? throw new InvalidOperationException(string.Format(ApplicationCoreMessages.PropertyNotInitialized, nameof(this.CatalogBrand)));
        private set => this.catalogBrand = value;
    }

    /// <summary>
    ///  カタログアイテムのアセットリストを取得します。
    /// </summary>
    public IReadOnlyCollection<CatalogItemAsset> Assets => this.assets.AsReadOnly();
}
