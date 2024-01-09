using Microsoft.AspNetCore.Diagnostics;
using System.Net.Mime;
using System.Net;
using System.Text.Json;

namespace BodyBuilderApp.Extensions {
    public static class ConfigureExceptionHandlerExtension {
        public static void ConfigureExceptionHandler(this WebApplication application) {
            application.UseExceptionHandler(builder => {
                builder.Run(async context => {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    var contextFeatures = context.Features.Get<IExceptionHandlerFeature>();
                    //if (contextFeatures != null) {
                    //    Log.Error($"Message: {contextFeatures.Error.Message}\n" +
                    //        $"Endpoint:{contextFeatures.Endpoint}\n" +
                    //        $"Path:{contextFeatures.Path}");
                    //}
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new {
                        StatusCode = context.Response.StatusCode,
                        ContentType = MediaTypeNames.Application.Json,
                        Message = contextFeatures.Error.Message,
                        Title = "Global Hata Yakalayıcısı ile yakalandı"

                    }));
                });
            });
        }
    }
}
