using RabbitMQ.Client;
using System.Text;

namespace Instance1.Reciver
{
    public class Reciver(IConnectionFactory _connectionFactory) : IReciver
    {
        public async Task ReciveKeyValue(string key, string value)
        {

            try
            {

                using var connection = _connectionFactory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.ExchangeDeclare(exchange: "cache_exchange", type: "fanout");

                var message = $"{key}:{value}";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "cache_exchange", routingKey: "", basicProperties: null, body: body);

                Console.WriteLine($"[x] Mesaj gönderildi: {message}");


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
            }
        }
    }
}
