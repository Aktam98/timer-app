using Common.AppConstants;
using Common.DTO;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Backend.Services
{
    public class TimerConsiumer:BackgroundService
    {
        private readonly IModel _channel;
        private readonly IConfigurationSection _config;
        private readonly IServiceScopeFactory _serviceProvider;
        public TimerConsiumer(IConfiguration configuration, IServiceScopeFactory serviceProvider)
        {
            _config = configuration.GetSection("RabbitMq");
            _serviceProvider = serviceProvider;
            var factory = new ConnectionFactory()
            {
                HostName = _config["Host"],
                Port = int.Parse(_config["Port"] ?? ""),
                UserName = _config["UserName"],
                Password = _config["Password"],
            };

            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _channel.QueueDeclare(AppConst.TimerQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consiumer = new EventingBasicConsumer(_channel);
            consiumer.Received += ReciveHandle;
            _channel.BasicConsume(AppConst.TimerQueue, false, consiumer);
        }

        private async void ReciveHandle(object sender, BasicDeliverEventArgs model)
        {
            using var scope = _serviceProvider.CreateScope();
            var _timerService = scope.ServiceProvider.GetRequiredService<TimerService>();
            var json = Encoding.UTF8.GetString(model.Body.ToArray());
            var messageModel = JsonConvert.DeserializeObject<TimerDTO>(json);
            if(messageModel is not null)
            {
                await _timerService.Create(messageModel);
                _channel.BasicAck(deliveryTag: model.DeliveryTag, multiple: false);
            }           
        }
    
}
}
