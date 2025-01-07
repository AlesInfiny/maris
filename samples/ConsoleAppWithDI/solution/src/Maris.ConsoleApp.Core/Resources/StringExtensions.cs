namespace Maris.ConsoleApp.Core.Resources;

/// <summary>
///  <see cref="string"/> クラスの拡張メソッドを提供します。
/// </summary>
internal static class StringExtensions
{
    /// <summary>
    ///  <paramref name="value"/> に指定した文字列のプレースホルダーを
    ///  <paramref name="args"/> の値で埋めた文字列を返却します。
    /// </summary>
    /// <param name="value">プレースホルダーを含む文字列。</param>
    /// <param name="args">プレースホルダーを埋める値。</param>
    /// <returns>プレースホルダーを埋めた文字列。</returns>
    internal static string Embed(this string value, params object?[] args)
        => string.Format(value, args);
}
