using Microsoft.Extensions.Caching.Memory;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
namespace Instance1.Consumer
{
    public class CacheConsumer(IConnectionFactory _connectionFactory, IMemoryCache cache) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            try
            {
                using var connection = _connectionFactory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.ExchangeDeclare(exchange: "cache_exchange", type: "fanout");

                var instanceId = Guid.NewGuid().ToString();
                var queueName = $"cache_queue_{instanceId}";

                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind(queue: queueName, exchange: "cache_exchange", routingKey: "");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    //mesajı split edip key value olarak cache'e yaz
                    var keyValue = message.Split(':');
                    cache.Set(keyValue[0], keyValue[1]);

                    Console.WriteLine($"Instance {instanceId} mesaj aldı: {message}");

                    //işlemi yapabilirsen başarılı fırlat
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };

                channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

                Console.WriteLine(" [*] Waiting for cache updates.");

                await Task.Delay(-1, stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }
        }

    }
}
