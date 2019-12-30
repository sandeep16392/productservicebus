using AutoMapper;
using Products.DAL.Abstraction;
using Products.DAL.Data;
using Products.DAL.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Products.DAL.EntityModels;

namespace Products.DAL.Repositories
{
    public class ProductRepository : IProductsRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(DataContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<List<string>> Add(ProductsPayloadDm payloadDm)
        {
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

            return duplicateProdCodes;
        }
        public async Task<List<string>> ValidateProducts(ProductsPayloadDm payloadDm)
        {
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
            }

            return duplicateProdCodes;
        }

        public async Task<ProductDm> Get(string code)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Code == code);
            var prodDm = _mapper.Map<ProductDm>(product);

            return prodDm;
        }

        public async Task<ICollection<ProductDm>> RetrieveAll()
        {
            var products = await _context.Products.ToListAsync();

            var prodDms = _mapper.Map<List<ProductDm>>(products);

            return prodDms;
        }
    }
}
