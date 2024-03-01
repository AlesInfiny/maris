using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Ordering;

/// <summary>
///  注文アイテムアセットのエンティティです。
/// </summary>
public class OrderItemAsset
{
    private OrderItem? orderItem;
    private string? assetCode;

    /// <summary>
    ///  <see cref="OrderItemAsset"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="assetCode">アセットコード。</param>
    /// <param name="orderItemId">注文アイテム Id 。</param>
    /// <exception cref="ArgumentException">
    ///  <list type="bullet">
    ///   <item><paramref name="assetCode"/> が <see langword="null"/> または空の文字列です。</item>
    ///  </list>
    /// </exception>
    public OrderItemAsset(string assetCode, long orderItemId)
    {
        this.AssetCode = assetCode;
        this.OrderItemId = orderItemId;
    }

    private OrderItemAsset()
    {
    }

    /// <summary>
    ///  注文アイテムアセット Id を取得します。
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
    ///  注文アイテム Id を取得します。
    /// </summary>
    public long OrderItemId { get; private set; }

    /// <summary>
    ///  注文アイテムを取得します。
    /// </summary>
    /// <exception cref="InvalidOperationException"><see cref="OrderItem"/> が設定されていません。</exception>
    public OrderItem OrderItem
    {
        get => this.orderItem ?? throw new InvalidOperationException(string.Format(Messages.PropertyNotInitialized, nameof(this.OrderItem)));
        private set => this.orderItem = value;
    }
}
