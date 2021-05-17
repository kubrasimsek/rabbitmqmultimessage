using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMq.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("linkinizi buraya yazınız");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();
            //yoksa sıfırdan oluşturur varsa bir işlem yapmaz.
            channel.QueueDeclare("hello-queue", true, false, false);

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                string message = $"Message {x}";

                var messageBody = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);

                Console.WriteLine($"mesaj gönderilmiştir : {message}");
            });

            Console.ReadLine();
        }
    }
}
