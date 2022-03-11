using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Ordering;

/// <summary>
///  注文されたカタログアイテムを管理する値オブジェクトです。
/// </summary>
/// <remarks>
///  <para>
///   この値オブジェクトは、注文時点でのカタログアイテムエンティティのスナップショットです。
///   これは、注文確定後にカタログ情報が変更されたとしても、注文情報は変更されるべきではないためです。
///  </para>
/// </remarks>
public record CatalogItemOrdered
{
    private long catalogItemId;
    private string productName;
    private string productCode;

    /// <summary>
    ///  <see cref="CatalogItemOrdered"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="catalogItemId">カタログアイテム Id 。</param>
    /// <param name="productName">商品名。</param>
    /// <param name="productCode">商品コード。</param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///  <paramref name="catalogItemId"/> は 0 以下に設定できません。
    /// </exception>
    /// <exception cref="ArgumentException">
    ///  <list type="bullet">
    ///   <item><paramref name="productName"/> が <see langword="null"/> または空の文字列です。</item>
    ///   <item><paramref name="productCode"/> が <see langword="null"/> または空の文字列です。</item>
    ///  </list>
    /// </exception>
    public CatalogItemOrdered(long catalogItemId, string productName, string productCode)
    {
        this.CatalogItemId = catalogItemId;
        this.ProductName = productName;
        this.ProductCode = productCode;
    }

    /// <summary>
    ///  カタログアイテム Id を取得します。
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">0 以下に設定できません。</exception>
    public long CatalogItemId
    {
        get => this.catalogItemId;
        init
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(value),
                    actualValue: value,
                    message: ApplicationCoreMessages.CatalogItemIdMustBePositive);
            }

            this.catalogItemId = value;
        }
    }

    /// <summary>
    ///  商品名を取得します。
    /// </summary>
    /// <exception cref="ArgumentException"><see langword="null"/> または空の文字列を設定できません。</exception>
    public string ProductName
    {
        get => this.productName;

        [MemberNotNull(nameof(productName))]
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(ApplicationCoreMessages.ArgumentIsNullOrWhiteSpace, nameof(value));
            }

            this.productName = value;
        }
    }

    /// <summary>
    ///  商品コードを取得します。
    /// </summary>
    /// <exception cref="ArgumentException"><see langword="null"/> または空の文字列を設定できません。</exception>
    public string ProductCode
    {
        get => this.productCode;

        [MemberNotNull(nameof(productCode))]
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(ApplicationCoreMessages.ArgumentIsNullOrWhiteSpace, nameof(value));
            }

            this.productCode = value;
        }
    }
}
