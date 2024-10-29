using System.Text.Json;

namespace Mekatrol.Automatum.Middleware.Extensions;

public static class SerializationExtensions
{
    public static T? DeepCopy<T>(this T? obj) where T : class
    {
        if (obj == null) return null;

        var json = JsonSerializer.Serialize(obj);
        return JsonSerializer.Deserialize<T>(json);
    }

}
