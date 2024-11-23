using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

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
        // Get all non-nullable properties from model
        var requiredProperties = model.Properties
            .Where(x => !x.Value.Nullable && !model.Required.Contains(x.Key))
            .Select(x => x.Key);
        
        // Mark property as required in schema json
        foreach (var propertyKey in requiredProperties)
        {
            model.Required.Add(propertyKey);
        }
    }
}
