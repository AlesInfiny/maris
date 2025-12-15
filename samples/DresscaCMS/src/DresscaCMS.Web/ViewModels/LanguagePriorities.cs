using System.Collections;

namespace DresscaCMS.Web.ViewModels;

/// <summary>
///  言語の優先順位を表すリストを提供します。
/// </summary>
public class LanguagePriorities : IList<Language>
{
    private readonly List<Language> languages = [Language.Japanese, Language.English, Language.Chinese, Language.Spanish];

    /// <inheritdoc/>
    public int Count => this.languages.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => true;

    /// <inheritdoc/>
    public Language this[int index]
    {
        get => this.languages[index];
        set => throw new NotSupportedException();
    }

    /// <inheritdoc/>
    public void Add(Language item) => throw new NotSupportedException();

    /// <inheritdoc/>
    public void Clear() => throw new NotSupportedException();

    /// <inheritdoc/>
    public bool Contains(Language item) => this.languages.Contains(item);

    /// <inheritdoc/>
    public void CopyTo(Language[] array, int arrayIndex) => this.languages.CopyTo(array, arrayIndex);

    /// <inheritdoc/>
    public IEnumerator<Language> GetEnumerator() => this.languages.GetEnumerator();

    /// <inheritdoc/>
    public int IndexOf(Language item) => this.languages.IndexOf(item);

    /// <inheritdoc/>
    public void Insert(int index, Language item) => throw new NotSupportedException();

    /// <inheritdoc/>
    public bool Remove(Language item) => throw new NotSupportedException();

    /// <inheritdoc/>
    public void RemoveAt(int index) => throw new NotSupportedException();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => this.languages.GetEnumerator();
}
