using System.ComponentModel.DataAnnotations;

namespace BodyBuilder.Infrastructure.Infrastructure.RabbitMq; 
public class RabbitMqSettings {
    [Required] public string HostName { get; set; } = default!;
    [Required] public int Port { get; set; } = default!;
    [Required] public string UserName { get; set; } = default!;
    [Required] public string Password { get; set; } = default!;
    [Required] public string ExchangeName { get; set; } = default!;
    [Required] public string QueueName { get; set; } = default!;
}
