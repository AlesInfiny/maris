using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Ordering;

/// <summary>
///  注文された陳列品を管理する値オブジェクトです。
/// </summary>
/// <remarks>
///  <para>
///   この値オブジェクトは、注文時点での陳列品エンティティのスナップショットです。
///   これは、注文確定後に陳列品情報が変更されたとしても、注文情報は変更されるべきではないためです。
///  </para>
/// </remarks>
public record DisplayItemOrdered
{
    private Guid displayItemId;
    private string productName;
    private string productCode;

    /// <summary>
    ///  <see cref="DisplayItemOrdered"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="displayItemId">陳列品 Id 。</param>
    /// <param name="productName">商品名。</param>
    /// <param name="productCode">商品コード。</param>
    /// <exception cref="ArgumentException">
    ///  <paramref name="displayItemId"/> に空の Guid は設定できません。
    /// </exception>
    /// <exception cref="ArgumentException">
    ///  <list type="bullet">
    ///   <item><paramref name="productName"/> が <see langword="null"/> または空の文字列です。</item>
    ///   <item><paramref name="productCode"/> が <see langword="null"/> または空の文字列です。</item>
    ///  </list>
    /// </exception>
    public DisplayItemOrdered(Guid displayItemId, string productName, string productCode)
    {
        this.DisplayItemId = displayItemId;
        this.ProductName = productName;
        this.ProductCode = productCode;
    }

    /// <summary>
    ///  陳列品 Id を取得します。
    /// </summary>
    /// <exception cref="ArgumentException">空の Guid は設定できません。</exception>
    public Guid DisplayItemId
    {
        get => this.displayItemId;
        init
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException(Messages.DisplayItemIdMustNotBeEmpty, nameof(value));
            }

            this.displayItemId = value;
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
                throw new ArgumentException(Messages.ArgumentIsNullOrWhiteSpace, nameof(value));
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
                throw new ArgumentException(Messages.ArgumentIsNullOrWhiteSpace, nameof(value));
            }

            this.productCode = value;
        }
    }
}
