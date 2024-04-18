using Common.AppConstants;
using Common.DTO;
using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Timer.Services
{
    public class TimerPublisher
    {
        private readonly IModel _channel;
        private readonly IConfigurationSection _config;
        public TimerPublisher(IConfiguration configuration)
        {
            _config = configuration.GetSection("RabbitMq");
            var factory = new ConnectionFactory()
            {
                HostName = _config["Host"],
                Port = int.Parse(_config["Port"]),
                UserName = _config["UserName"],
                Password = _config["Password"],
            };

            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _channel.QueueDeclare(AppConst.TimerQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
        }

        public void SendMessage(TimerDTO message)
        {
            var jsonMessage = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            _channel.BasicPublish(exchange: "", routingKey: AppConst.TimerQueue, basicProperties: null, body: body);
        }
    }
}
