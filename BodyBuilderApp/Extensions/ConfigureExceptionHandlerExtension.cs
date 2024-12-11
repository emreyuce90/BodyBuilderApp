using Microsoft.AspNetCore.Diagnostics;
using System.Net.Mime;
using System.Net;
using System.Text.Json;
using Serilog;
using Microsoft.AspNetCore.Http.Features;

namespace BodyBuilderApp.Extensions {
    public static class ConfigureExceptionHandlerExtension {
        public static void ConfigureExceptionHandler(this WebApplication application) {
            application.UseExceptionHandler(builder => {
                builder.Run(async context => {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    var contextFeatures = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeatures != null) {
                        // Loglama işlemi için gerekli bilgileri alıyoruz
                        var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint?.DisplayName;
                        var path = context.Request.Path;
                        var method = context.Request.Method;
                        var queryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : "Yok";
                        var requestBody = string.Empty;

                        // Eğer POST/PUT ise body'yi okuyabiliriz
                        if (method == "POST" || method == "PUT") {
                            context.Request.EnableBuffering();
                            using var reader = new StreamReader(context.Request.Body);
                            requestBody = await reader.ReadToEndAsync();
                            context.Request.Body.Position = 0;
                        }

                        // Hata detaylarını loglama
                        Log.Error($"Hata Mesajı: {contextFeatures.Error.Message}\n" +
                            $"Endpoint: {endpoint}\n" +
                            $"Yöntem: {method}\n" +
                            $"Path: {path}\n" +
                            $"Query String: {queryString}\n" +
                            $"Body: {requestBody}\n" +
                            $"Hata: {contextFeatures.Error.StackTrace}");

                        // Hata cevabı
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new {
                            StatusCode = context.Response.StatusCode,
                            ContentType = MediaTypeNames.Application.Json,
                            Message = contextFeatures.Error.Message,
                            Title = "Global Hata Yakalayıcısı ile yakalandı"
                        }));
                    }
                });
            });
        }
    }
}
