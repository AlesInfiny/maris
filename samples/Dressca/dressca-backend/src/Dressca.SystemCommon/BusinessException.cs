﻿using Dressca.SystemCommon.Resources;

namespace Dressca.SystemCommon;

/// <summary>
///  業務エラーを表す例外クラスです。
/// </summary>
public class BusinessException : Exception
{
    private readonly BusinessErrorCollection businessErrors = new();

    /// <summary>
    ///  <see cref="BusinessException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public BusinessException()
        : base(SystemCommonMessages.BusinessError)
    {
    }

    /// <summary>
    ///  業務エラーのリストを指定して
    ///  <see cref="BusinessException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="businessErrors">業務エラーのリスト。</param>
    public BusinessException(params BusinessError[] businessErrors)
        : this(null, businessErrors)
    {
    }

    /// <summary>
    ///  内部例外と業務エラーのリストを指定して
    ///  <see cref="BusinessException"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="innerException">内部例外。</param>
    /// <param name="businessErrors">業務エラーのリスト。</param>
    public BusinessException(Exception? innerException, params BusinessError[] businessErrors)
        : base(SystemCommonMessages.BusinessError, innerException)
    {
        if (businessErrors is not null)
        {
            foreach (var businessError in businessErrors)
            {
                this.businessErrors.AddOrMerge(businessError);
            }
        }
    }

    /// <inheritdoc/>
    public override string Message => $"{base.Message}{Environment.NewLine}{this.businessErrors}";

    /// <summary>
    ///  業務エラーを追加します。
    /// </summary>
    /// <param name="businessError">業務エラー。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="businessError"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public void AddOrMergeError(BusinessError businessError)
    {
        ArgumentNullException.ThrowIfNull(businessError);
        this.businessErrors.AddOrMerge(businessError);
    }
}
