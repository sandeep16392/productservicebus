using System.Threading.Tasks;

namespace Products.ServiceBusMessaging
{
    public interface IServiceBusConsumer
    {
        void RegisterOnMessageHandlerAndReceiveMessages();
        Task CloseQueueAsync();
    }
}
