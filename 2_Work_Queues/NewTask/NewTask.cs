using System;
using RabbitMQ.Client;
using System.Text;

namespace NewTask
{
    class Newtask
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using(var connection = factory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "nata_queue",
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    string message = GetMessage(args);

                    var body = Encoding.UTF8.GetBytes(message);

                    var properies = channel.CreateBasicProperties();
                    properies.Persistent = true; 

                    channel.BasicPublish(exchange: "",
                                         routingKey: "nata_queue",
                                         basicProperties: properies,
                                         body: body);

                    Console.WriteLine(" [x] Sent {0}", message);
                }

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private static string GetMessage(string[] args)
        {
            return (args.Length > 0) ? string.Join(" ", args) : "Hello World!";
        }
    }
}
