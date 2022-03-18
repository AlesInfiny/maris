using System.Runtime.Serialization;
using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Ordering;

/// <summary>
///  注文のチェックアウト処理開始時に買い物かごが空であることを表す例外クラスです。
/// </summary>
public class EmptyBasketOnCheckoutException : Exception
{
    /// <summary>
    ///  <see cref="EmptyBasketOnCheckoutException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public EmptyBasketOnCheckoutException()
        : base(ApplicationCoreMessages.BasketIsEmptyOnCheckout)
    {
    }

    /// <summary>
    ///  <see cref="EmptyBasketOnCheckoutException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="info">オブジェクトをシリアライズまたはデシリアライズするために必要なすべてのデータを格納するオブジェクト。</param>
    /// <param name="context">ストリーミングコンテキスト。</param>
    protected EmptyBasketOnCheckoutException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
