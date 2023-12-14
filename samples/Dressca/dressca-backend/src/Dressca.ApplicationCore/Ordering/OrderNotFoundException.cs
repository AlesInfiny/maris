using System.Runtime.Serialization;
using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Ordering;

/// <summary>
///  注文情報が存在しないことを表す例外クラスです。
/// </summary>
public class OrderNotFoundException : Exception
{
    /// <summary>
    ///  見つからなかった注文 Id と購入者 Id を指定して
    ///  <see cref="OrderNotFoundException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="orderId">見つからなかった注文 Id 。</param>
    /// <param name="buyerId">見つからなかった購入者 Id 。</param>
    public OrderNotFoundException(long orderId, string buyerId)
        : base(string.Format(Messages.OrderNotFound, orderId, buyerId))
    {
    }
}
