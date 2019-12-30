using Microsoft.AspNetCore.Builder;
using Products.DAL.Abstraction;
using Products.DAL.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Products.ServiceBusMessaging
{
    public class ProcessData : IProcessData
    {
        private readonly IProductsRepository _productRepository;

        public ProcessData(IProductsRepository productRepository)
        {
            this._productRepository = productRepository;
        }
        public async Task Process(ProductsPayloadDm productsPayload)
        {
            await _productRepository.Add(productsPayload);
        }
    }
}
