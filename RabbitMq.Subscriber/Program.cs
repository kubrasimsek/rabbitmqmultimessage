using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMq.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("linkinizi buraya yazınız");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            //mesaj boyutu , mesajlar kaç kaç gelsin (her subscribera), true olunca her birine 5 er gönderir (false olunca toplam subscribera toplam sayıyı gönderir)
            channel.BasicQos(0, 1, false);

            //Eğer publisher.2ın bunu tanımladığına eminseniz bunu silebilirsiniz.
            //Eğer publisher da tanımlanmadıysa burda tanımlama yapılır.
            //Her iki taraf için de tanımlama yapılacaksa parametreler aynı olmalı
            ////channel.QueueDeclare("hello-queue", true, false, false);

            var consumer = new EventingBasicConsumer(channel);

            //AutoAck false olunca mesajları otomatik silmez.
            channel.BasicConsume("hello-queue", false, consumer);

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Thread.Sleep(1500);

                Console.WriteLine("Gelen Mesaj:" + message);

                //gelen mesajı bulup kuyruktan siler.
                channel.BasicAck(e.DeliveryTag, false);
            };


            Console.ReadLine();
        }

    }
}
