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

namespace Products.DAL.Repositories
{
    public class VendorRepository : IVendorRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public VendorRepository(DataContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }
        public async Task<bool> Add(VendorDm vendor)
        {
            bool isPresent = await _context.Vendors.AnyAsync(x => x.Code == vendor.Code);

            if (isPresent)
                throw new ArgumentException("Vendor Already Present.");

            var vendorEm = _mapper.Map<Vendor>(vendor);
            _context.Vendors.Add(vendorEm);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(string code)
        {
            var vendor = await _context.Vendors.FirstOrDefaultAsync(x => x.Code == code);

            if (vendor == null)
                throw new ArgumentNullException("Vendor not present.");

            _context.Vendors.Remove(vendor);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<VendorDm> Get(string code)
        {
            var vendor = await _context.Vendors.FirstOrDefaultAsync(x => x.Code == code);

            var vendorDm = _mapper.Map<VendorDm>(vendor);

            return vendorDm;
        }

        public async Task<ICollection<VendorDm>> RetrieveAll()
        {
            var vendors = await _context.Vendors.ToListAsync();

            var vendorsDm = _mapper.Map<List<VendorDm>>(vendors);

            return vendorsDm;
        }
    }
}
