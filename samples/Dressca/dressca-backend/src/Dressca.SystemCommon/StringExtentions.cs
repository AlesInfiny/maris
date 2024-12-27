using System.Diagnostics.CodeAnalysis;

namespace System;

/// <summary>
///  <see cref="string"/> クラスの拡張メソッドを提供します。
/// </summary>
public static class StringExtentions
{
    /// <summary>
    ///  対象の文字列から改行文字（\r、\n）を取り除きます。
    /// </summary>
    /// <param name="target">対象の文字列。</param>
    /// <returns>元の文字列から改行文字を取り除いた文字列。</returns>
    [return: NotNullIfNotNull(nameof(target))]
    public static string? RemoveNewlineCharacters(this string? target)
    {
        if (target == null)
        {
            return null;
        }

        return target.Replace("\n", string.Empty).Replace("\r", string.Empty);
    }
}
