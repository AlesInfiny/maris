﻿using System.Runtime.Serialization;
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
        : base(string.Format(ApplicationCoreMessages.BasketNotFound, basketId))
    {
    }

    /// <summary>
    ///  メッセージを指定して
    ///  <see cref="BasketNotFoundException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="message">メッセージ。</param>
    public BasketNotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    ///  メッセージと内部例外を指定して
    ///  <see cref="BasketNotFoundException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="message">メッセージ。</param>
    /// <param name="innerException">内部例外。</param>
    public BasketNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    ///  <see cref="BasketNotFoundException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="info">オブジェクトをシリアライズまたはデシリアライズするために必要なすべてのデータを格納するオブジェクト。</param>
    /// <param name="context">ストリーミングコンテキスト。</param>
    protected BasketNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
