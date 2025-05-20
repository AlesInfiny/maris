using Dressca.ApplicationCore.Resources;
using Dressca.SystemCommon;

namespace Dressca.ApplicationCore.Ordering;

/// <summary>
///  注文情報が存在しないことを表す例外クラスです。
/// </summary>
public class OrderNotFoundException : BusinessException
{
    private const string ExceptionId = "orderNotFound";

    /// <summary>
    ///  見つからなかった注文 Id と購入者 Id を指定して
    ///  <see cref="OrderNotFoundException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="orderId">見つからなかった注文 Id 。</param>
    /// <param name="buyerId">見つからなかった購入者 Id 。</param>
    public OrderNotFoundException(long orderId, string buyerId)
        : base(new BusinessError(ExceptionId, new ErrorMessage(string.Format(Messages.OrderNotFound, orderId, buyerId), [orderId.ToString(), buyerId])))
    {
    }
}
