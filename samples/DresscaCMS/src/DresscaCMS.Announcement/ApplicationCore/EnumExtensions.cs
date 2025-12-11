using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DresscaCMS.Announcement.ApplicationCore;

/// <summary>
/// Enum 値の拡張メソッドを提供します。
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Enum 値に関連付けられた <see cref="DisplayAttribute"/> の名前を取得します。
    /// 属性が存在しない場合は Enum の名前を返します。
    /// </summary>
    /// <typeparam name="TEnum">Enum 型。</typeparam>
    /// <param name="value">Enum 値。</param>
    /// <returns>Display 名または Enum 名。</returns>
    public static string GetDisplayName<TEnum>(this TEnum value)
        where TEnum : struct, Enum
    {
        var type = typeof(TEnum);
        var name = Enum.GetName(type, value);
        if (name is null)
        {
            return value.ToString();
        }

        var field = type.GetField(name);
        var attribute = field?.GetCustomAttribute<DisplayAttribute>();

        return attribute?.GetName() ?? name;
    }
}
