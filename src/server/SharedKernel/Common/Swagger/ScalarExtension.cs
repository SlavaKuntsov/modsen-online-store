using Microsoft.AspNetCore.Builder;
using Scalar.AspNetCore;

namespace Common.Swagger;

public static class ScalarExtension
{
    public static WebApplication UseScalar(this WebApplication app)
    {
        app.UseSwagger(c => c.RouteTemplate = "openapi/{documentName}.json");

        app.MapScalarApiReference(options =>
        {
            options.WithTheme(ScalarTheme.BluePlanet);
        });

        return app;
    }
}
