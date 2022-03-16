using System.Diagnostics.CodeAnalysis;

namespace Dressca.SystemCommon.Mapper;

/// <summary>
///  <typeparamref name="TFrom"/> から <typeparamref name="TTo"/> へのマッピングを行うインターフェースです。
/// </summary>
/// <typeparam name="TFrom">元の型。</typeparam>
/// <typeparam name="TTo">マッピング先の型。</typeparam>
public interface IObjectMapper<TFrom, TTo>
{
    /// <summary>
    ///  <typeparamref name="TFrom"/> のオブジェクトを
    ///  <typeparamref name="TTo"/> のオブジェクトに変換します。
    /// </summary>
    /// <param name="value">変換するオブジェクト。</param>
    /// <returns>変換後のオブジェクト。</returns>
    [return: NotNullIfNotNull("value")]
    TTo? Convert(TFrom? value);
}
