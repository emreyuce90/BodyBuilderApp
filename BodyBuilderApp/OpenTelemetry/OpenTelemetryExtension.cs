using Microsoft.Extensions.Options;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace BodyBuilderApp.OpenTelemetry {
    public static class OpenTelemetryExtension {
        public static IServiceCollection AddOpenTelemetryExt(this IServiceCollection services,IConfiguration configuration) {

            //mapped class
            services.AddOptions<OpenTelemetrySettings>().BindConfiguration(nameof(OpenTelemetrySettings)).ValidateDataAnnotations();

            //değerlerini singleton olarak di a ekledim
            services.AddSingleton<OpenTelemetrySettings>(sp => {
                return sp.GetRequiredService<IOptions<OpenTelemetrySettings>>().Value;
            });


            //di dan servisin çağırılması 

            using var serviceProvider = services.BuildServiceProvider();
            var openTelemetrySettings = serviceProvider.GetRequiredService<OpenTelemetrySettings>();


            services.AddOpenTelemetry().WithTracing(configure => {
                //source name ve servis name i verdik
                configure.AddSource(openTelemetrySettings.ActivitySourceName)
                .ConfigureResource(resource => {
                    resource.AddService(serviceName: openTelemetrySettings.ServiceName, serviceVersion: openTelemetrySettings.ServiceVersion);
                });

                //api ye gelen HTTP isteklerini trace e ekler
                configure.AddAspNetCoreInstrumentation(opt => {
                    opt.RecordException = true;
                    opt.Filter = (context) => {
                        if (!String.IsNullOrEmpty(context.Request.Path.Value)) {
                            return context.Request.Path.Value.Contains("api", StringComparison.InvariantCulture);
                        }
                        return false;
                    };
                });

                configure.AddOtlpExporter(); //Verileri jaegarda görüntülemek için
                //Veritabanı isteklerini görebilmek için efcore 
                configure.AddEntityFrameworkCoreInstrumentation(efCoreOptions => {
                    efCoreOptions.SetDbStatementForText = true;
                    efCoreOptions.SetDbStatementForStoredProcedure = true;
                    efCoreOptions.EnrichWithIDbCommand = (action, dbCommand) => {
                        //
                    };
                });
            });

            return services;
        }
    }
}
