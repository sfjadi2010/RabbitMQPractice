// See https://aka.ms/new-console-template for more information
using System;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "letterbox",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

while (true)
{
    Console.WriteLine("Enter a message to send to the letterbox:");
    var message = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(message) || message.ToLower() == "exit")
    {
        break;
    }
    
    var encodedMessage = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "",
                         routingKey: "letterbox",
                         basicProperties: null,
                         body: encodedMessage);
    Console.WriteLine(" [x] Sent {0}", message);
}
