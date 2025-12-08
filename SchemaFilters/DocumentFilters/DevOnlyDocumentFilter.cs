using Microsoft.OpenApi.Models;
using ProfileApi.SchemaFilters.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

public class DevOnlyDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        if (isDev) return; // Show everything in development

        // Remove controllers with [DevOnly] attribute
        var devOnlyControllers = context.ApiDescriptions
            .Where(desc => desc.ActionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor cad &&
                           cad.ControllerTypeInfo.GetCustomAttribute<DevOnlyAttribute>() != null)
            .Select(desc => desc.RelativePath)
            .ToList();

        foreach (var path in devOnlyControllers)
        {
            swaggerDoc.Paths.Remove("/" + path);
        }
    }
}