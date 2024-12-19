
using BodyBuilder.Infrastructure.Infrastructure.RabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class RabbitMqExtension {
    public static IServiceCollection AddRabbitMqExt(this IServiceCollection services ) {

       services.AddSingleton<IMessageBus,RabbitMqMessageBus>();

        return services;
    }
}

