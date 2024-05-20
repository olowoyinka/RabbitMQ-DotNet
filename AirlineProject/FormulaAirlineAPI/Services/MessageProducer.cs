using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace FormulaAirlineAPI.Services
{
    public class MessageProducer : IMessageProducer
    {
        public void SendingMessage<T>(T message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "user",
                Password = "mypass",
                VirtualHost = "/"
            };

            var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.QueueDeclare("bookings", durable: true, exclusive: false);

            var jsonString = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonString);

            channel.BasicPublish("", "bookings", body: body);
        }
    }
}
