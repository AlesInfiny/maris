using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Dressca.SystemCommon;

/// <summary>
///  システム内で単一の <see cref="JsonSerializerOptions"/> のインスタンスを提供します。
/// </summary>
public static class DefaultJsonSerializerOptions
{
    private static JsonSerializerOptions? options;

    /// <summary>
    ///  すべての Unicode をエスケープせずにシリアライズする <see cref="JsonSerializerOptions"/> を取得します。
    /// </summary>
    public static JsonSerializerOptions Options
    {
        get
        {
            if (options == null)
            {
                options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                };
            }

            return options;
        }
    }
}
