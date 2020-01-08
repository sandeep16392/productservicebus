using Products.DAL.Abstraction;
using Products.DAL.Data;
using Products.DAL.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Products.DAL.EntityModels;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Products.DAL.Repositories
{
    public class VendorRepository : IVendorRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration configuration;
        private bool IsLocalObjectImplementation = false;
        public List<Vendor> Vendors { get; set; }
        public VendorRepository(DataContext context, IMapper mapper, IConfiguration configuration)
        {
            this._context = context;
            this._mapper = mapper;
            this.configuration = configuration;
            IsLocalObjectImplementation = Convert.ToBoolean(configuration.GetSection("ImplementWithLocalObject").Value);
        }
        /// <summary>
        /// Add Vendors to repository
        /// </summary>
        /// <param name="vendor"></param>
        /// <returns></returns>
        public async Task<bool> Add(VendorDm vendor)
        {
            if (IsLocalObjectImplementation)
            {
                var present = Vendors.Any(x => x.Code == vendor.Code);

                if (present)
                    throw new ArgumentException("Vendor Already Present.");

                var vendors = _mapper.Map<Vendor>(vendor);
                Vendors.Add(vendors);

                return true;
            }

            bool isPresent = await _context.Vendors.AnyAsync(x => x.Code == vendor.Code);

            if (isPresent)
                throw new ArgumentException("Vendor Already Present.");

            var vendorEm = _mapper.Map<Vendor>(vendor);
            _context.Vendors.Add(vendorEm);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(string code)
        {

            if (IsLocalObjectImplementation)
            {
                var localVendor = Vendors.FirstOrDefault(x => x.Code == code);

                if (localVendor == null)
                    throw new ArgumentNullException("Vendor not present.");

                Vendors.Remove(localVendor);

                return true;
            }

            var vendor = await _context.Vendors.FirstOrDefaultAsync(x => x.Code == code);

            if (vendor == null)
                throw new ArgumentNullException("Vendor not present.");

            _context.Vendors.Remove(vendor);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<VendorDm> Get(string code)
        {
            if (IsLocalObjectImplementation)
            {
                var localVendor = Vendors.FirstOrDefault(x => x.Code == code);
                var localvendorDm = _mapper.Map<VendorDm>(localVendor);

                return localvendorDm;
            }

            var vendor = await _context.Vendors.FirstOrDefaultAsync(x => x.Code == code);

            var vendorDm = _mapper.Map<VendorDm>(vendor);

            return vendorDm;
        }

        public async Task<ICollection<VendorDm>> RetrieveAll()
        {
            if (IsLocalObjectImplementation)
            {
                var localVendors = _mapper.Map<List<VendorDm>>(Vendors);
                return localVendors;
            }

            var vendors = await _context.Vendors.ToListAsync();

            var vendorsDm = _mapper.Map<List<VendorDm>>(vendors);

            return vendorsDm;
        }
    }
}
