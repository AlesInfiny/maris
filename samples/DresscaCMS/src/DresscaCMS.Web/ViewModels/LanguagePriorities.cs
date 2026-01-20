using System.Collections;

namespace DresscaCMS.Web.ViewModels;

/// <summary>
///  言語の優先順位を表すリストを提供します。
/// </summary>
public class LanguagePriorities : IReadOnlyList<Language>
{
    private readonly List<Language> languages = [Language.Japanese, Language.English, Language.Chinese, Language.Spanish];

    /// <inheritdoc/>
    public int Count => this.languages.Count;

    /// <inheritdoc/>
    public Language this[int index]
    {
        get => this.languages[index];
        set => throw new NotSupportedException();
    }

    /// <summary>
    ///  指定した言語の表示順を取得します。
    ///  対象外の言語の場合は <see cref="int.MaxValue"/> を返します。
    /// </summary>
    /// <param name="language">言語。</param>
    /// <returns>表示順。</returns>
    public int GetDisplayOrder(Language language)
    {
        var index = this.languages.IndexOf(language);
        return index >= 0 ? index : int.MaxValue;
    }

    /// <inheritdoc/>
    public IEnumerator<Language> GetEnumerator() => this.languages.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
