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
    /// <exception cref="ArgumentNullException">
    ///  <paramref name="str"/> が <see langword="null"/> です。
    /// </exception>
    public static string RemoveNewLines(this string? target)
    {
        ArgumentNullException.ThrowIfNull(target);
        return target.Replace("\n", string.Empty).Replace("\r", string.Empty);
    }
}
