using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Products.DAL.DomainModels
{
    public class ProductsPayloadDm
    {
        [Required]
        public VendorDm Vendor { get; set; }
        [Required]
        public List<ProductDm> Products { get; set; }
    }
}
