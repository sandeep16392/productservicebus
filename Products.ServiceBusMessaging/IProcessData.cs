using Products.DAL.DomainModels;
using System.Threading.Tasks;

namespace Products.ServiceBusMessaging
{
    public interface IProcessData
    {
        Task Process(ProductsPayloadDm myPayload);
    }
}
