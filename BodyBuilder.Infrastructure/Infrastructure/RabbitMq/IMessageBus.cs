namespace BodyBuilder.Infrastructure.Infrastructure.RabbitMq; 
public interface IMessageBus {
    Task PublishAsync<T>(T message,string routingKey) where T:class;
}
