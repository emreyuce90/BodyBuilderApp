using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BodyBuilderApp.Middlewares {
    public class OpenTelemetryTraceIdMiddleware {
       
        private readonly RequestDelegate _next;

        public OpenTelemetryTraceIdMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext) {
            //getting logger from httpContext request
            var logger = httpContext.RequestServices.GetService<ILogger<OpenTelemetryTraceIdMiddleware>>();
            var traceId = Activity.Current?.TraceId.ToString();
            //TraceId Activityden alınarak loglandı
            using (logger?.BeginScope("{@traceId}",traceId)) { //@ koymamızın sebebi Elasticsearchin bu datayı indekslemesini istememizden dolayıdır
                await _next(httpContext);
            }
        }
    }
}
