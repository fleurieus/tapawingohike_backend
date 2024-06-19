using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

class Program
{
    private static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var locationLogChannel = connection.CreateModel();
        using var syncChannel = connection.CreateModel();

        locationLogChannel.QueueDeclare(queue: "locationlogs",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
        locationLogChannel.QueueDeclare(queue: "sync",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        Console.WriteLine(" [*] Waiting for messages.");

        var syncConsumer = new EventingBasicConsumer(syncChannel);
        var locationlogConsumer = new EventingBasicConsumer(locationLogChannel);
        locationlogConsumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [LOCATIONLOG] Received {message}");

            await SendApiCallAsync(message);
        };
        locationLogChannel.BasicConsume(queue: "locationlogs",
                             autoAck: true,
                             consumer: locationlogConsumer);

        syncConsumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [SYNC] Received {message}");
        };
        syncChannel.BasicConsume(queue: "sync",
            autoAck: true,
            consumer: syncConsumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }

    private static async Task SendApiCallAsync(string message)
    {
        var parts = message.Split(';');
        if (parts.Length != 3)
        {
            Console.WriteLine(" [x] Invalid message format");
            return;
        }

        if (!int.TryParse(parts[0], out int teamId) ||
            !double.TryParse(parts[1], out double longitude) ||
            !double.TryParse(parts[2], out double latitude))
        {
            Console.WriteLine(" [x] Invalid data in message");
            return;
        }

        var url = $"https://localhost:7061/teams/{teamId}/locationlogs";
        var jsonPayload = new
        {
            latitude,
            longitude
        };

        var content = new StringContent(
            System.Text.Json.JsonSerializer.Serialize(jsonPayload),
            Encoding.UTF8,
            "application/json"
        );

        try
        {
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(" [x] API call successful");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($" [x] API call failed: {e.Message}");
        }
    }

}
