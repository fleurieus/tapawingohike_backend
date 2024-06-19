using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var sync_channel = connection.CreateModel();

        // Declare a fanout exchange for broadcasting messages
        sync_channel.ExchangeDeclare(exchange: "sync_broadcast_exchange", type: ExchangeType.Fanout);

        // Declare a queue for each client and bind it to the exchange
        var queueName = sync_channel.QueueDeclare().QueueName;
        sync_channel.QueueBind(queue: queueName,
                          exchange: "sync_broadcast_exchange",
                          routingKey: "");

        Console.WriteLine("Enter your team ID: ");
        int teamId = Convert.ToInt32(Console.ReadLine());
        int clientId = (int)new Random().NextInt64(1000, 9999);
        Console.WriteLine($"Client {clientId} (TeamID: {teamId}) is ready.");

        // Consumer setup to listen for messages
        var consumer = new EventingBasicConsumer(sync_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [Client {clientId} RECEIVED] {message}");
        };
        sync_channel.BasicConsume(queue: queueName,
                             autoAck: true,
                             consumer: consumer);

        // Loop to send messages
        while (true)
        {
            Console.WriteLine("press [enter] to send sync message or type 'log' to send locationlog or type 'exit' to quit: ");
            string message = Console.ReadLine();
            if (message.ToLower() == "exit")
                break;
            if (message.ToLower() == "log")
            {

            }
            else
            {
                var body = Encoding.UTF8.GetBytes($"{teamId};synchronize");
                sync_channel.BasicPublish(exchange: "sync_broadcast_exchange",
                                     routingKey: "", // Empty routing key for fanout exchange
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine($" [Client {clientId} SENT] {message}");
            }
        }
    }
}
