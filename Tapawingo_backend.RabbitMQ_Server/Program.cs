using System;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var locationlogChannel = connection.CreateModel();
using var syncChannel = connection.CreateModel();

locationlogChannel.QueueDeclare(queue: "locationlogs",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
syncChannel.QueueDeclare(queue: "sync",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

Console.WriteLine("Format: team_id;longtitude;latitude");
Console.WriteLine("You're placed in team: 1");

while(true)
{
    //Create random longtitude-latitude
    Random random = new Random();
    double latitude = random.NextDouble() * 180 - 90;
    double longitude = random.NextDouble() * 360 - 180;
    int team_id = 1;

    //Console.WriteLine(" Press [enter] to send new locationlog.");
    Console.WriteLine(" Press [enter] to send new sync.");
    Console.ReadLine();

    string message = $"{team_id};synchronize";
    var body = Encoding.UTF8.GetBytes(message);

    syncChannel.BasicPublish(exchange: string.Empty,
                         routingKey: "sync", //queue name!
                         basicProperties: null,
                         body: body);
    Console.WriteLine($" [x] Sent {message}");

    // // USE TO SEND LOCATIONLOG
    //string message = $"{team_id};{longitude};{latitude}";
    //var body = Encoding.UTF8.GetBytes(message);

    //channel.BasicPublish(exchange: string.Empty,
    //                     routingKey: "locationlogs", //queue name!
    //                     basicProperties: null,
    //                     body: body);
    //Console.WriteLine($" [x] Sent {message}");
}

