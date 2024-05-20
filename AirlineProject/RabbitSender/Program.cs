using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
//factory.Uri = new Uri(uriString: "amqp://user:mypass@localhost:5672");
factory.Uri = new Uri(uriString: "amqps://student:XYR4yqc.cxh4zug6vje@rabbitmq-exam.rmq3.cloudamqp.com/mxifnklj");
factory.ClientProvidedName = "RabbitMQ Practical";

IConnection connection = factory.CreateConnection();

IModel channel = connection.CreateModel();

string exchangeName = "exchange.ed69607f-c51a-4bc2-ab6a-222d75a60ef5";
string routingKey = "ed69607f-c51a-4bc2-ab6a-222d75a60ef5";
string queueName = "exam";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable: true);
channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
channel.QueueBind(queueName, exchangeName, routingKey, arguments: null);

var properties = channel.CreateBasicProperties();
properties.DeliveryMode = 2;

byte[] messageBodyBytes = Encoding.UTF8.GetBytes(s: "Hi CloudAMQP, this was fun!");
channel.BasicPublish(exchangeName, routingKey, basicProperties: properties, messageBodyBytes);

channel.Close();
connection.Close();