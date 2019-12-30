using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Products.DAL.DomainModels
{
    public class VendorDm
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string Contact { get; set; }
    }
}
