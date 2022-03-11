using System.Diagnostics.CodeAnalysis;

namespace Dressca.SystemCommon.Mapper;

/// <summary>
///  クラス間のマッピングを行うインターフェースです。
/// </summary>
/// <typeparam name="T1">マッピングを行う型 1 。</typeparam>
/// <typeparam name="T2">マッピングを行う型 2 。</typeparam>
public interface IObjectMapper<T1, T2>
{
    /// <summary>
    ///  <typeparamref name="T2"/> のオブジェクトを
    ///  <typeparamref name="T1"/> のオブジェクトに変換します。
    /// </summary>
    /// <param name="value">変換するオブジェクト。</param>
    /// <returns>変換後のオブジェクト。</returns>
    [return: NotNullIfNotNull("value")]
    T1? Convert(T2? value);

    /// <summary>
    ///  <typeparamref name="T1"/> のオブジェクトを
    ///  <typeparamref name="T2"/> のオブジェクトに変換します。
    /// </summary>
    /// <param name="value">変換するオブジェクト。</param>
    /// <returns>変換後のオブジェクト。</returns>
    [return: NotNullIfNotNull("value")]
    T2? Convert(T1? value);
}
