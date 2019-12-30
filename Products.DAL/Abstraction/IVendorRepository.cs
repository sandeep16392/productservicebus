using Products.DAL.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Products.DAL.Abstraction
{
    public interface IVendorRepository
    {
        Task<bool> Add(VendorDm vendor);
        Task<bool> Delete(string code);
        Task<ICollection<VendorDm>> RetrieveAll();
        Task<VendorDm> Get(string code);
    }
}
