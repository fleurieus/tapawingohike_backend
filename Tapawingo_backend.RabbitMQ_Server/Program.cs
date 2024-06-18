using System;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "locationlogs",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

Console.WriteLine("Format: team_id;longtitude;latitude");
Console.WriteLine("You're placed in team: 3");

while(true)
{
    //Create random longtitude-latitude
    Random random = new Random();
    double latitude = random.NextDouble() * 180 - 90;
    double longitude = random.NextDouble() * 360 - 180;
    int team_id = 3;

    Console.WriteLine(" Press [enter] to send new locationlog.");
    Console.ReadLine();

    string message = $"{team_id};{longitude};{latitude}";
    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: string.Empty,
                         routingKey: "locationlogs", //queue name!
                         basicProperties: null,
                         body: body);
    Console.WriteLine($" [x] Sent {message}");
}

