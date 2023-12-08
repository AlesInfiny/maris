using System.Runtime.Serialization;
using Dressca.ApplicationCore.Resources;

namespace Dressca.ApplicationCore.Baskets;

/// <summary>
///  買い物かごが存在しないことを表す例外クラスです。
/// </summary>
public class BasketNotFoundException : Exception
{
    /// <summary>
    ///  見つからなかった買い物かご Id を指定して
    ///  <see cref="BasketNotFoundException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="basketId">見つからなかった買い物かご Id 。</param>
    public BasketNotFoundException(long basketId)
        : base(string.Format(Messages.BasketNotFound, basketId))
    {
    }
}
