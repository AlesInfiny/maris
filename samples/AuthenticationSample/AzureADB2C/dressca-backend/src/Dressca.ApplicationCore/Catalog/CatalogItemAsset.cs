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
    /// <param name="assetCode">アセットコード。</param>
    /// <param name="catalogItemId">カタログアイテム Id 。</param>
    /// <exception cref="ArgumentException">
    ///  <list type="bullet">
    ///   <item><paramref name="assetCode"/> が <see langword="null"/> または空の文字列です。</item>
    ///  </list>
    /// </exception>
    public CatalogItemAsset(string assetCode, long catalogItemId)
    {
        this.AssetCode = assetCode;
        this.CatalogItemId = catalogItemId;
    }

    private CatalogItemAsset()
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
    public string AssetCode
    {
        get => this.assetCode ?? throw new InvalidOperationException(string.Format(Messages.PropertyNotInitialized, nameof(this.AssetCode)));
        private set
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
    public long CatalogItemId { get; private set; }

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
