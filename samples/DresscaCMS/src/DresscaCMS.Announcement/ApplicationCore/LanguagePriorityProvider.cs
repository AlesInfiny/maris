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
    private static readonly LanguageOrder[] DefaultLanguagePriorities =
        [
            new("ja", 0), // 日本語
            new("en", 1), // 英語
            new("zh", 2), // 中国語
            new("es", 3), // スペイン語
        ];

    /// <summary>
    ///  指定した言語コードの優先順位を取得します。
    ///  存在しない場合は int.MaxValue を返します。
    /// </summary>
    /// <param name="code">言語コード。</param>
    /// <returns>優先順位。</returns>
    public static int GetLanguageOrder(string code)
    {
        var languageOrder = DefaultLanguagePriorities
            .FirstOrDefault(lo => string.Equals(lo.Code, code, StringComparison.OrdinalIgnoreCase));
        return languageOrder is not null ? languageOrder.Order : int.MaxValue;
    }

    private record LanguageOrder(string Code, int Order);
}
