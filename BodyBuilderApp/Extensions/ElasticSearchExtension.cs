using Microsoft.Extensions.Options;

namespace BodyBuilderApp.Extensions {
    public static class ElasticSearchExtension {
        public static IServiceCollection AddElasticExt(this IServiceCollection services) {

            services.AddOptions<ElasticSearchSettings>().BindConfiguration(nameof(ElasticSearchSettings)).ValidateDataAnnotations();

            services.AddSingleton<ElasticSearchSettings>(sp => sp.GetRequiredService<IOptions<ElasticSearchSettings>>().Value);

            return services;
        }
    }
}
