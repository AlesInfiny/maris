using System.Diagnostics.CodeAnalysis;

namespace Dressca.SystemCommon;

/// <summary>
///  <see cref="object"/> クラスの拡張メソッドを提供します。
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    ///  指定した値が <see langword="null"/> であるか検証し、
    ///  <see langword="null"/> である場合は例外をスローします。
    /// </summary>
    /// <param name="obj"><see langword="null"/> か検査するオブジェクト。</param>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="obj"/> が <see langword="null"/> です。
    /// </exception>
    public static void ThrowIfNull([NotNull] this object? obj)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }
    }
}
