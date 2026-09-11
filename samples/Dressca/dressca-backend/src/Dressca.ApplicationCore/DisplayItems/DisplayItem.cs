using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.DisplayItems;

/// <summary>
///  陳列品 エンティティです。
/// </summary>
public class DisplayItem
{
    private string name;
    private string description;
    private decimal price;
    private string productCode;
    private Guid displayItemCategoryId;
    private Guid displayItemBrandId;

    /// <summary>
    ///  <see cref="DisplayItem"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public DisplayItem()
    {
    }

    /// <summary>
    ///  陳列品 Id を取得します。
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///  商品名を取得します。
    /// </summary>
    /// <exception cref="ArgumentException">商品名が <see langword="null"/> または空の文字列です。</exception>
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
    ///  説明を取得します。
    /// </summary>
    /// <exception cref="ArgumentException">説明が <see langword="null"/> または空の文字列です。</exception>
    public required string Description
    {
        get => this.description;

        [MemberNotNull(nameof(description))]
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(Messages.ArgumentIsNullOrWhiteSpace, nameof(value));
            }

            this.description = value;
        }
    }

    /// <summary>
    ///  単価を取得します。
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">単価は負の値に設定できません。</exception>
    public required decimal Price
    {
        get => this.price;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(value),
                    actualValue: value,
                    message: Messages.PriceMustBeZeroOrHigher);
            }

            this.price = value;
        }
    }

    /// <summary>
    ///  商品コードを取得します。
    /// </summary>
    /// <remarks>
    ///  本来は存在するであろう在庫管理系のコンテキストで識別子として使用されるコードです。
    ///  買い物かごコンテキストとは DisplayItem.Id で連携するため、注意してください。
    /// </remarks>
    /// <exception cref="ArgumentException">商品コードには半角英数字を設定してください。</exception>
    public required string ProductCode
    {
        get => this.productCode;

        [MemberNotNull(nameof(productCode))]
        init
        {
            if (!Regex.IsMatch(value, @"^[a-zA-Z0-9]+$"))
            {
                throw new ArgumentException(Messages.ArgumentsMustBeAlphanumeric, nameof(value));
            }

            this.productCode = value;
        }
    }

    /// <summary>
    ///  陳列品カテゴリ Id を取得します。
    /// </summary>
    /// <exception cref="ArgumentException">陳列品カテゴリ ID に空の Guid は設定できません。</exception>
    public required Guid DisplayItemCategoryId
    {
        get => this.displayItemCategoryId;
        init
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException(Messages.DisplayItemCategoryIdMustNotBeEmpty, nameof(value));
            }

            this.displayItemCategoryId = value;
        }
    }

    /// <summary>
    ///  陳列品ブランド Id を取得します。
    /// </summary>
    /// <exception cref="ArgumentException">陳列品ブランド ID に空の Guid は設定できません。</exception>
    public required Guid DisplayItemBrandId
    {
        get => this.displayItemBrandId;
        init
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException(Messages.DisplayItemBrandIdMustNotBeEmpty, nameof(value));
            }

            this.displayItemBrandId = value;
        }
    }

    /// <summary>
    ///  陳列品のアセットリストを取得します。
    /// </summary>
    public IReadOnlyList<DisplayItemAsset> Assets { get; init; } = [];

    /// <summary>
    ///  論理削除フラグを取得します。
    /// </summary>
    public bool IsDeleted { get; init; } = false;
}
