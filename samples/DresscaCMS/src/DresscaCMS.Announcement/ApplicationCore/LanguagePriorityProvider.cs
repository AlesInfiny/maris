namespace DresscaCMS.Announcement.ApplicationCore;

/// <summary>
///  言語の優先順位を提供する静的クラスです。
/// </summary>
public static class LanguagePriorityProvider
{
    /// <summary>
    ///  言語コードの優先順位を定義します。
    ///  配列の先頭から順に優先度が高くなります。
    /// </summary>
    private static readonly string[] DefaultLanguagePriority =
        [
            "ja", // 日本語
            "en", // 英語
            "zh", // 中国語
            "es", // スペイン語
        ];

    /// <summary>
    ///  言語コードから優先順位へのマッピングを取得します。
    /// </summary>
    /// <returns>言語コード → 優先順位のディクショナリ。</returns>
    public static IReadOnlyDictionary<string, int> GetLanguageOrderMap()
    {
        return DefaultLanguagePriority
            .Distinct(StringComparer.Ordinal)
            .Select((code, index) => new { code, index })
            .ToDictionary(x => x.code, x => x.index);
    }
}
