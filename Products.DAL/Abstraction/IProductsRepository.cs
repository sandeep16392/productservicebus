using Products.DAL.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Products.DAL.Abstraction
{
    public interface IProductsRepository
    {
        Task<List<string>> Add(ProductsPayloadDm payloadDm);
        Task<ProductDm> Get(string code);
        Task<ICollection<ProductDm>> RetrieveAll();
        Task<List<string>> ValidateProducts(ProductsPayloadDm payloadDm);

    }
}
