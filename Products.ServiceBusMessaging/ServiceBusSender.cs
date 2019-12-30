using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Products.DAL.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Products.ServiceBusMessaging
{
    public class ServiceBusSender
    {
        private readonly QueueClient _queueClient;
        private readonly IConfiguration _configuration;

        public ServiceBusSender(IConfiguration configuration)
        {
            _configuration = configuration;
            var connectStr = _configuration.GetSection("SeviceBusSetting").GetSection("ServiceBusConnectionString").Value;
            var queue = _configuration.GetSection("SeviceBusSetting").GetSection("QueueName").Value;
            _queueClient = new QueueClient(connectStr, queue);
        }

        public async Task SendMessage(ProductsPayloadDm payload)
        {
            string data = JsonConvert.SerializeObject(payload);
            Message message = new Message(Encoding.UTF8.GetBytes(data));

            await _queueClient.SendAsync(message);
        }
    }
}
