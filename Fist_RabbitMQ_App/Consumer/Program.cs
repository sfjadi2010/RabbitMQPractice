using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.QueueDeclare(
    queue: "letterbox",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
    byte[] body = ea.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);
    Console.WriteLine(" [x] Received {0}", message);
};

channel.BasicConsume(
    queue: "letterbox",
    autoAck: true,
    consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();