using AutoMapper;
using Products.DAL.DomainModels;
using Products.DAL.EntityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.DAL.Mapper
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            //Dm to Em
            CreateMap<ProductDm, Product>();
            CreateMap<VendorDm, Vendor>();
            CreateMap<ProductsPayloadDm, ProductPayload>();

            //Em to Dm
            CreateMap<Product, ProductDm>();
            CreateMap<Vendor, VendorDm>();
            CreateMap<ProductPayload, ProductsPayloadDm>();
        }
    }
}
