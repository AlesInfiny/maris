using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Xunit.Sdk;

namespace Maris.ConsoleApp.UnitTests.XunitSerializer;

public class JsonXunitSerializer<T> : IXunitSerializer
{
    public object Deserialize(Type type, string serializedValue)
    {
        if (type == typeof(T))
        {
            var context = JsonSerializer.Deserialize<T>(serializedValue);
            if (context is not null)
            {
                return context;
            }
        }

        throw new NotSupportedException("デシリアライズに失敗しました。");
    }

    public bool IsSerializable(Type type, object? value, [NotNullWhen(false)] out string? failureReason)
    {
        if (type == typeof(T))
        {
            failureReason = null;
            return true;
        }

        failureReason = "サポートされていない型です。";
        return false;
    }

    public string Serialize(object value)
        => JsonSerializer.Serialize(value);
}
