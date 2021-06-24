using ApplicationCore.Interfaces.RabbitMQ;
using ApplicationCore.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly RabbitMQSettings _rabbitMQSettings;

        private const string ExchangeName = "direct_logs";
        private static IConnection connection;
        private static IModel channel;

        public RabbitMQService(RabbitMQSettings rabbitMQSettings)
        {
            _rabbitMQSettings = rabbitMQSettings;
        }

        public async Task<bool> SendMessageAsync(RabbitMqMessageModel messageModel)
        {
            try
            {

                //Create connection factory
                var conFactory = new ConnectionFactory()
                {
                    HostName = _rabbitMQSettings.HostName,
                    UserName = _rabbitMQSettings.UserName,
                    Password = _rabbitMQSettings.Password,
                    Port = _rabbitMQSettings.Port,
                    VirtualHost = _rabbitMQSettings.VirtualHost
                };

                //Message
                string printerId = messageModel.PrinterId.ToString();
                string messageString = JsonConvert.SerializeObject(messageModel);
                byte[] msg = Encoding.UTF8.GetBytes(messageString);
                //string routeKey = messageModel.PrinterId.ToString();
                string routeKey = messageModel.ComputerId.ToString();
                string queueName = messageModel.ComputerId.ToString();


                //Create connection
                if (connection == null)
                {
                    connection = conFactory.CreateConnection();
                }
                if (channel == null)
                {
                    channel = connection.CreateModel();
                }


                //Declare exchange method
                channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Direct);

                //Declare queue
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind(queue: queueName, exchange: ExchangeName, routingKey: routeKey, arguments: null);

                //Push msg to RabbitMQ server
                channel.BasicPublish(exchange: ExchangeName, routingKey: routeKey, basicProperties: null, body: msg);

                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
