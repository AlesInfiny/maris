using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Dressca.SystemCommon.Text.Json;

/// <summary>
///  システム内で単一の <see cref="JsonSerializerOptions"/> のインスタンスを提供します。
/// </summary>
public static class DefaultJsonSerializerOptions
{
    private static JsonSerializerOptions options = new JsonSerializerOptions
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
    };

    /// <summary>
    ///  すべての Unicode をエスケープせずにシリアライズする <see cref="JsonSerializerOptions"/> を取得します。
    /// </summary>
    /// <returns><see cref="JsonSerializerOptions"/> のインスタンス。</returns>
    public static JsonSerializerOptions GetInstance()
    {
        return options;
    }
}
