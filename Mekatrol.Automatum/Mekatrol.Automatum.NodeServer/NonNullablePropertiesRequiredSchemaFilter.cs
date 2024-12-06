using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Mekatrol.Automatum.NodeServer;

/// <summary>
/// By default generated schema will allow properties to be nullable. This filter 
/// ensures that if the C# model property is non-nullable then the generated schema will 
/// mark the property as required so that the generated client API will also mark it
/// as required.
/// </summary>
public class NonNullablePropertiesRequiredSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        var typeProperties = context.Type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToList();

        foreach (var p in model.Properties)
        {
            // Given this is a public property there should be exactly one, and if not we want an exception ias it is unexpected
            var typeProperty = typeProperties.Single(tp => tp.Name.Equals(p.Key, StringComparison.OrdinalIgnoreCase));

            var isNullable = IsNullable(typeProperty);

            if (!isNullable && !model.Required.Contains(p.Key))
            {
                model.Required.Add(p.Key);
                p.Value.Nullable = false;
            }
        }
    }

    private static bool IsNullable(PropertyInfo pi)
    {
        return
            // Is nullable if has nullable attribute
            pi.CustomAttributes.Any(cc => cc.AttributeType.FullName!.StartsWith("System.Runtime.CompilerServices.NullableAttribute")) ||

            // Is nullable is undelying type is a nullable type
            Nullable.GetUnderlyingType(pi.PropertyType) != null;
    }
}
