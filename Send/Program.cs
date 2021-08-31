using RabbitMQ.Client;
using System;
using System.Text;

namespace Send
{
    class Program
    {
        static void Main(string[] args)
        {
            var mensagem = "";
            while (mensagem != "exit")
            {
                //Console.WriteLine("Digite a fila: ");
                //var fila = Console.ReadLine();

                Console.WriteLine("Digite a mensagem: ");
                mensagem = Console.ReadLine();
                Fila("", mensagem);
            }
        }

        public static void Fila(string fila, string mensagem)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {

                    channel.QueueDeclare(queue: "task",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                    var body = Encoding.UTF8.GetBytes(mensagem);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: "",
                                         routingKey: "task",
                                         basicProperties: properties,
                                         body: body);

                    Console.WriteLine("[x] Sent {0}", mensagem);
                }
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
