using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.DisplayItems;

/// <summary>
///  陳列品アセットのエンティティです。
/// </summary>
public class DisplayItemAsset
{
    private string assetCode;

    /// <summary>
    ///  <see cref="DisplayItemAsset"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public DisplayItemAsset()
    {
    }

    /// <summary>
    ///  アセットコードを取得します。
    /// </summary>
    /// <exception cref="ArgumentException">アセットコードが <see langword="null"/> または空の文字列です。</exception>
    public required string AssetCode
    {
        get => this.assetCode;

        [MemberNotNull(nameof(assetCode))]
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(Messages.ArgumentIsNullOrWhiteSpace, nameof(value));
            }

            this.assetCode = value;
        }
    }

    /// <summary>
    ///  陳列品 Id を取得します。
    /// </summary>
    public required Guid DisplayItemId { get; init; }
}
