using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Products.DAL.Abstraction;
using Products.DAL.Data;
using Products.DAL.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AutoMapper;
using Products.DAL.EntityModels;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Products.ServiceBusMessaging
{
    public class ProcessData : IProcessData
    {
        private readonly IProductsRepository _productRepository;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;
        private readonly IConfiguration configuration;
        private bool IsLocalObjectImplementation = false;
        public List<Product> Products { get; set; }
        public ProcessData(IProductsRepository productRepository, IServiceScopeFactory serviceScopeFactory, IMapper mapper, IConfiguration configuration)
        {
            this._productRepository = productRepository;
            this._serviceScopeFactory = serviceScopeFactory;
            this._mapper = mapper;
            this.configuration = configuration;
            IsLocalObjectImplementation = Convert.ToBoolean(configuration.GetSection("ImplementWithLocalObject").Value);
        }
        /// <summary>
        /// Implementaion with both local object and DB. 
        /// </summary>
        /// <param name="payloadDm"></param>
        /// <returns></returns>
        public async Task Process(ProductsPayloadDm payloadDm)
        {
            //Implementation with local object.
            if (IsLocalObjectImplementation)
            {
                foreach (var product in payloadDm.Products)
                {

                    var prodEm = _mapper.Map<Product>(product);
                    prodEm.CreatedOn = DateTime.UtcNow;
                    Products.Add(prodEm);
                }

                return;
            }



            //Implementation with DB
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetService<DataContext>();

                bool isVendorPresent = await _context.Vendors.AnyAsync(x => x.Code == payloadDm.Vendor.Code);

                if (!isVendorPresent)
                    throw new ArgumentException("Invalid Vendor.");

                var duplicateProdCodes = new List<string>();
                foreach (var product in payloadDm.Products)
                {
                    bool isProductPresent = await _context.Products.AnyAsync(x => x.Code == product.Code);

                    if (isProductPresent)
                    {
                        duplicateProdCodes.Add(product.Code);
                    }
                    else
                    {
                        var prodEm = _mapper.Map<Product>(product);
                        prodEm.CreatedOn = DateTime.UtcNow;
                        _context.Products.Add(prodEm);
                    }
                }

                await _context.SaveChangesAsync();
            }
            //await _productRepository.Add(payloadDm);

        }
    }
}
