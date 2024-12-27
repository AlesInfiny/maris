using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System;

/// <summary>
///  <see cref="object"/> クラスの拡張メソッドを提供します。
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    ///  指定した値が <see langword="null"/> であるか検証し、
    ///  <see langword="null"/> である場合は例外をスローします。
    /// </summary>
    /// <param name="argument"><see langword="null"/> か検査するオブジェクト。</param>
    /// <param name="paramName">パラメーターの名前。</param>
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="argument"/> が <see langword="null"/> です。
    /// </exception>
    public static void ThrowIfNull(
        [NotNull] this object? argument,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument is null)
        {
            throw new ArgumentNullException(paramName);
        }
    }
}
