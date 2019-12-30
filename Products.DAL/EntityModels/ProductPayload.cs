using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Products.DAL.EntityModels
{
    public class ProductPayload
    {
        public int Id { get; set; }
        [Required]
        public Vendor Vendor { get; set; }
        [Required]
        public List<Product> Products { get; set; }
    }
}
