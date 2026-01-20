using DresscaCMS.Web.Resources;

namespace DresscaCMS.Web.ViewModels;

/// <summary>
///  言語を表します。
/// </summary>
public class Language
{
    /// <summary>
    ///  英語。
    /// </summary>
    public static readonly Language English = new("en", Messages.English);

    /// <summary>
    ///  日本語。
    /// </summary>
    public static readonly Language Japanese = new("ja", Messages.Japanese);

    /// <summary>
    ///  中国語。
    /// </summary>
    public static readonly Language Chinese = new("zh", Messages.Chinese);

    /// <summary>
    ///  スペイン語。
    /// </summary>
    public static readonly Language Spanish = new("es", Messages.Spanish);

    private Language(string code, string displayName)
    {
        this.Code = code;
        this.DisplayName = displayName;
    }

    /// <summary>
    ///  言語コードを取得します。（例: "ja", "en" 等）
    /// </summary>
    public string Code { get; private init; }

    /// <summary>
    ///  表示名を取得します。（例: "日本語", "英語" 等）
    /// </summary>
    public string DisplayName { get; private init; }

    /// <summary>
    ///  指定した言語コードから <see cref="Language"/> オブジェクトを取得します。
    /// </summary>
    /// <param name="code">言語コード。</param>
    /// <returns><see cref="Language"/> オブジェクト。</returns>
    /// <exception cref="ArgumentException">
    ///  <list type="bullet">
    ///   <item><paramref name="code"/> がサポートされていない言語コードの場合。</item>
    ///  </list>
    /// </exception>
    public static Language GetLanguageFrom(string code)
    {
        return code switch
        {
            "en" => English,
            "ja" => Japanese,
            "zh" => Chinese,
            "es" => Spanish,
            _ => throw new ArgumentException(string.Format(Messages.UnsupportedLanguageCode, code), nameof(code)),
        };
    }

    /// <summary>
    ///  言語コードで等価性を比較します。
    /// </summary>
    /// <param name="obj">比較対象の値。</param>
    /// <returns>等しい場合は <see langword="true"/> 、そうでない場合は <see langword="false"/> 。</returns>
    public override bool Equals(object? obj)
    {
        if (obj is Language other)
        {
            return this.Code.Equals(other.Code, StringComparison.Ordinal);
        }

        return false;
    }

    /// <inheritdoc/>
    public override int GetHashCode() => this.Code.GetHashCode(StringComparison.Ordinal);
}
