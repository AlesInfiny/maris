using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Catalog;

/// <summary>
///  カタログアイテムアセットのエンティティです。
/// </summary>
public class CatalogItemAsset
{
    private CatalogItem? catalogItem;
    private string? assetCode;

    /// <summary>
    ///  <see cref="CatalogItemAsset"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public CatalogItemAsset()
    {
    }

    /// <summary>
    ///  カタログアイテムアセット Id を取得します。
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    ///  アセットコードを取得します。
    /// </summary>
    /// <exception cref="ArgumentException">アセットコードが <see langword="null"/> または空の文字列です。</exception>
    /// <exception cref="InvalidOperationException"><see cref="AssetCode"/> が設定されていません。</exception>
    public required string AssetCode
    {
        get => this.assetCode ?? throw new InvalidOperationException(string.Format(Messages.PropertyNotInitialized, nameof(this.AssetCode)));
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
    ///  カタログアイテム Id を取得します。
    /// </summary>
    public required long CatalogItemId { get; init; }

    /// <summary>
    ///  カタログアイテムを取得します。
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="CatalogItem"/> が設定されていません。</exception>
    public CatalogItem CatalogItem
    {
        get => this.catalogItem ?? throw new InvalidOperationException(string.Format(Messages.PropertyNotInitialized, nameof(this.CatalogItem)));
        private set => this.catalogItem = value;
    }
}
