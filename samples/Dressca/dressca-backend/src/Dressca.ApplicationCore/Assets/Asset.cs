using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Assets;

/// <summary>
///  アセットを表現するエンティティです。
/// </summary>
public class Asset
{
    private string assetCode;
    private string assetType;

    /// <summary>
    ///  <see cref="Asset"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public Asset()
    {
    }

    /// <summary>
    ///  アセット Id を取得します。
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    ///  アセットコードを取得します。
    /// </summary>
    /// <exception cref="ArgumentException">null または空の文字列を設定できません。</exception>
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
    ///  アセットのタイプを取得します。
    /// </summary>
    /// <exception cref="NotSupportedException">サポートされていないアセットタイプが指定されました。</exception>
    public required string AssetType
    {
        get => this.assetType;

        [MemberNotNull(nameof(assetType))]
        init
        {
            if (!AssetTypes.IsSupportedAssetType(value))
            {
                throw new NotSupportedException(string.Format(Messages.InvalidAssetType, value));
            }

            this.assetType = value;
        }
    }
}
