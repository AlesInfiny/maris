using System.Collections;

namespace DresscaCMS.Web.ViewModels;

/// <summary>
///  言語の優先順位を表すリストを提供します。
/// </summary>
public class LanguagePriorities : IList<string>
{
    private readonly List<string> languages = ["ja", "en", "zh", "es"];

    /// <inheritdoc/>
    public int Count => this.languages.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => true;

    /// <inheritdoc/>
    public string this[int index]
    {
        get => this.languages[index];
        set => throw new NotSupportedException();
    }

    /// <inheritdoc/>
    public void Add(string item) => throw new NotSupportedException();

    /// <inheritdoc/>
    public void Clear() => throw new NotSupportedException();

    /// <inheritdoc/>
    public bool Contains(string item) => this.languages.Contains(item);

    /// <inheritdoc/>
    public void CopyTo(string[] array, int arrayIndex) => this.languages.CopyTo(array, arrayIndex);

    /// <inheritdoc/>
    public IEnumerator<string> GetEnumerator() => this.languages.GetEnumerator();

    /// <inheritdoc/>
    public int IndexOf(string item) => this.languages.IndexOf(item);

    /// <inheritdoc/>
    public void Insert(int index, string item) => throw new NotSupportedException();

    /// <inheritdoc/>
    public bool Remove(string item) => throw new NotSupportedException();

    /// <inheritdoc/>
    public void RemoveAt(int index) => throw new NotSupportedException();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => this.languages.GetEnumerator();
}
