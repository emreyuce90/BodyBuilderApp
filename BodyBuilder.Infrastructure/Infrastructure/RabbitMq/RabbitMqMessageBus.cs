
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace BodyBuilder.Infrastructure.Infrastructure.RabbitMq {

    public class RabbitMqMessageBus : IMessageBus {
        private readonly ILogger<RabbitMqMessageBus> _logger;
        private readonly RabbitMqSettings _rabbitMqSettings;
        private readonly IChannel _channel;
        
        public RabbitMqMessageBus(RabbitMqSettings rabbitMqSettings, ILogger<RabbitMqMessageBus> logger) {
            _logger = logger;
            _rabbitMqSettings = rabbitMqSettings;
           
            try {
                var factory = new ConnectionFactory();
                factory.HostName = _rabbitMqSettings.HostName;
                factory.Port = _rabbitMqSettings.Port;
                factory.UserName = _rabbitMqSettings.UserName;
                factory.Password = _rabbitMqSettings.Password;

                //bağlantı oluştur
                var connection = factory.CreateConnectionAsync().Result;
                //kanal oluştur
                _channel = connection.CreateChannelAsync().Result;

                //Kuyruk oluştur 
                _channel.ExchangeDeclareAsync(exchange: _rabbitMqSettings.ExchangeName, type: ExchangeType.Direct, durable: true, autoDelete: false, cancellationToken: default);
                _channel.QueueDeclareAsync(queue: _rabbitMqSettings.QueueName,durable:true,exclusive:false,autoDelete:false,arguments:null,cancellationToken:default);
                _channel.QueueBindAsync(
                   queue: _rabbitMqSettings.QueueName,
                   exchange: _rabbitMqSettings.ExchangeName,
                   routingKey: _rabbitMqSettings.QueueName
               );
            } catch (Exception ex) {
                _logger.LogError($"RabbitMQ bağlantı hatası: {ex.Message}", ex);
                throw;
            }

        }


        public async Task PublishAsync<T>(T message, string routingKey) where T : class {

            try {
            var messageBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            await _channel.BasicPublishAsync(_rabbitMqSettings.ExchangeName, routingKey, messageBody);
                _logger.LogInformation("Mesaj başarıyla yayınlandı.");
            } catch (Exception ex) {
                _logger.LogError($"RabbitMQ mesaj gönderim hatası: {ex.Message}", ex);
                throw;
            }
        }
    }
}
