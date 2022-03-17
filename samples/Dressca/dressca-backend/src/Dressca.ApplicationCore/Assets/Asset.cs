using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Assets;

/// <summary>
///  アセットエンティティ。
/// </summary>
public class Asset
{
    private string? assetCode;
    private string? assetType;

    /// <summary>
    ///  <see cref="Asset"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="assetCode">アセットコード。</param>
    /// <param name="assetType">アセットタイプ。</param>
    /// <exception cref="ArgumentException">
    ///  <paramref name="assetCode"/> が <see langword="null"/> または空の文字列です。
    /// </exception>
    /// <exception cref="NotSupportedException">
    ///  <paramref name="assetType"/> はサポートされていない文字列です。
    /// </exception>
    public Asset(string assetCode, string assetType)
    {
        this.AssetCode = assetCode;
        this.AssetType = assetType;
    }

    private Asset()
    {
    }

    /// <summary>
    ///  アセット Id を取得します。
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    ///  アセットコードを取得します。
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="AssetCode"/> が設定されていません。</exception>
    public string AssetCode
    {
        get => this.assetCode ?? throw new InvalidOperationException(string.Format(ApplicationCoreMessages.PropertyNotInitialized, nameof(this.AssetCode)));
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(ApplicationCoreMessages.ArgumentIsNullOrWhiteSpace, nameof(value));
            }

            this.assetCode = value;
        }
    }

    /// <summary>
    ///  アセットのタイプを取得します。
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="AssetType"/> が設定されていません。</exception>
    /// <exception cref="NotSupportedException">サポートされていないアセットタイプが指定されました。</exception>
    public string AssetType
    {
        get => this.assetType ?? throw new InvalidOperationException(string.Format(ApplicationCoreMessages.PropertyNotInitialized, nameof(this.AssetType)));
        private set
        {
            if (!AssetTypes.IsSupportedAssetType(value))
            {
                throw new NotSupportedException(string.Format(ApplicationCoreMessages.InvalidAssetType, value));
            }

            this.assetType = value;
        }
    }
}
