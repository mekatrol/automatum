using Mekatrol.Automatum.Middleware.Extensions;
using Mekatrol.Automatum.Models;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mekatrol.Automatum.Middleware.Converters;

public class ServiceErrorConverter : JsonConverter<ServiceError>
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public override ServiceError? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (JsonSerializer.Deserialize(ref reader, typeToConvert, _options) is not ServiceError model) { return null; }

        if (!string.IsNullOrEmpty(model.Property))
        {
            // Convert first chracter of property name to uppercase
            model.Property = char.ToUpperInvariant(model.Property[0]) + model.Property[1..];
        }

        return model;
    }

    public override void Write(Utf8JsonWriter writer, ServiceError value, JsonSerializerOptions options)
    {
        // Make a deep copy so we don't alter original model
        var copy = value.DeepCopy();

        if (copy != null && !string.IsNullOrWhiteSpace(copy.Property))
        {
            // Convert first chracter of property name to lowercase
            copy.Property = char.ToLowerInvariant(copy.Property[0]) + copy.Property[1..];
        }

        // Use copy to serialize
        JsonSerializer.Serialize(writer, copy, typeof(ServiceError), _options);
    }
}
